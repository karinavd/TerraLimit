import json
import urllib.request
import urllib.parse
import urllib.error
from concurrent.futures import ThreadPoolExecutor
from collections import defaultdict
from django.shortcuts import render
from django.http import JsonResponse
from django.views.decorators.http import require_GET
from django.conf import settings


def _get(d: dict, key: str, default=None):
    if not isinstance(d, dict):
        return default
    for k, v in d.items():
        if k.lower() == key.lower():
            return v
    return default


def _fetch_page(path: str, offset: int, limit: int) -> list:
    base = getattr(settings, "TERRALIMIT_API_BASE", "http://127.0.0.1:5088")
    qs = urllib.parse.urlencode({"offset": offset, "limit": limit})
    url = f"{base}/{path.lstrip('/')}?{qs}"
    req = urllib.request.Request(url, headers={"Accept": "application/json"})
    try:
        with urllib.request.urlopen(req, timeout=10) as resp:
            raw = resp.read()
            return json.loads(raw.decode("utf-8")) if raw else []
    except urllib.error.HTTPError as e:
        if e.code in [204, 404]:
            return []
        raise


def _fetch_all(path: str, page_size: int = 2500) -> list:
    first_page = _fetch_page(path, offset=0, limit=page_size)
    if not first_page or len(first_page) < page_size:
        return first_page or []

    result = list(first_page)
    offsets = range(page_size, page_size * 20, page_size)

    with ThreadPoolExecutor(max_workers=5) as executor:
        futures = [executor.submit(_fetch_page, path, offset, page_size) for offset in offsets]
        for fut in futures:
            page = fut.result()
            if not page:
                break
            result.extend(page)
            if len(page) < page_size:
                break
    return result


def _calc_top20(stations: list, records: list, parameters: list, water_type: str = "") -> list:
    param_limits = {
        str(_get(p, "id")).upper(): float(_get(p, "limitValue"))
        for p in parameters
        if _get(p, "id") and _get(p, "limitValue") is not None
    }

    if water_type:
        wt_lower = water_type.lower()
        filtered_stations = [s for s in stations if (_get(s, "waterType") or "").lower() == wt_lower]
    else:
        filtered_stations = stations

    station_map = {_get(s, "id", ""): s for s in filtered_stations if _get(s, "id")}
    by_station = defaultdict(list)

    for r in records:
        sid = _get(r, "stationId")
        if sid not in station_map:
            continue
        code = str(_get(r, "parameterCode") or "").upper()
        limit = param_limits.get(code)
        if not limit:
            continue
        val = _get(r, "value")
        if val is None:
            continue
        normalized = min(float(val) / limit * 100, 100)
        by_station[sid].append(normalized)

    result = []
    for sid, scores in by_station.items():
        info = station_map.get(sid, {})
        result.append({
            "station_id": sid,
            "station_identifier": _get(info, "stationIdentifier") or sid,
            "country_name": _get(info, "countryName") or "—",
            "water_type": _get(info, "waterType") or "—",
            "avg_pollution": round(sum(scores) / len(scores), 1),
            "record_count": len(scores), 
        })
    return sorted(result, key=lambda x: x["avg_pollution"], reverse=True)[:20]


def _calc_depth_profile_all(records: list, parameters: list, station_id: str = "") -> list:
    param_info = {
        str(_get(p, "id")).upper(): {
            "name": _get(p, "name", ""),
            "limit": _get(p, "limitValue")
        }
        for p in parameters if _get(p, "id")
    }

    buckets = defaultdict(lambda: defaultdict(list))
    for r in records:
        if station_id and _get(r, "stationId") != station_id:
            continue
        code = str(_get(r, "parameterCode") or "").upper()
        info = param_info.get(code)
        if not info:
            continue
        val = _get(r, "value")
        if val is not None:
            depth = round(float(_get(r, "depth", 0) or 0), 1)
            buckets[depth][info["name"]].append(float(val))

    result = []
    for depth in sorted(buckets.keys()):
        params = {name: round(sum(vals) / len(vals), 4) for name, vals in buckets[depth].items()}
        result.append({"depth": depth, "params": params})
    return result
def _calc_param_summary(records: list, parameters: list) -> list:
    param_info = {
        str(_get(p, "id")).upper(): {
            "name": _get(p, "name", ""),
            "description": _get(p, "description", ""),
            "limit": _get(p, "limitValue"),
            "unit": _get(p, "unit", "")
        }
        for p in parameters if _get(p, "id")
    }

    agg = defaultdict(list)
    for r in records:
        code = str(_get(r, "parameterCode") or "").upper()
        val = _get(r, "value")
        if code and val is not None:
            agg[code].append(float(val))

    result = []
    for code, vs in agg.items():
        info = param_info.get(code, {})
        result.append({
            "code": code,
            "name": info.get("name") or code,
            "description": info.get("description"),
            "limit_value": info.get("limit"),
            "unit": info.get("unit"),
            "avg_value": round(sum(vs) / len(vs), 3),
        })
    return sorted(result, key=lambda x: x["avg_value"], reverse=True)




def dashboard(request):
    return render(request, "dashboard/index.html")


@require_GET
def api_analytics(request):
    try:
        with ThreadPoolExecutor(max_workers=3) as executor:
            f_stations = executor.submit(_fetch_all, "water/stations")
            f_records = executor.submit(_fetch_all, "water/records")
            f_parameters = executor.submit(_fetch_all, "water/parameters")
            stations = f_stations.result()
            records = f_records.result()
            parameters = f_parameters.result()
    except Exception as e:
        return JsonResponse({"error": str(e)}, status=502)

    top20 = _calc_top20(stations, records, parameters)
    depth_profile = _calc_depth_profile_all(records, parameters)  
    param_summary = _calc_param_summary(records, parameters)
    water_types = sorted({wt for s in stations if (wt := _get(s, "waterType"))})

    return JsonResponse({
        "top20": top20,
        "param_summary": param_summary,
        "depth_profile": depth_profile,
        "water_types": water_types,
        "raw_stations": stations,
        "raw_records": records,
        "raw_parameters": parameters
    })


@require_GET
def api_depth_profile(request):
    station_id = request.GET.get("station_id", "")
    try:
        records = _fetch_all("water/records")
        parameters = _fetch_all("water/parameters")
        data = _calc_depth_profile_all(records, parameters, station_id) 
        return JsonResponse({"data": data, "station_id": station_id})
    except Exception as e:
        return JsonResponse({"error": str(e)}, status=502)


@require_GET
def api_station_detail(request, station_id):
    try:
        base = getattr(settings, "TERRALIMIT_API_BASE", "http://127.0.0.1:5088")
        url = f"{base}/water/stations/{urllib.parse.quote(station_id)}"
        req = urllib.request.Request(url, headers={"Accept": "application/json"})
        with urllib.request.urlopen(req, timeout=8) as resp:
            station = json.loads(resp.read())

        records = _fetch_all("water/records")
        parameters = _fetch_all("water/parameters")
        station["parameters"] = _calc_param_summary(
            [r for r in records if _get(r, "stationId") == station_id],
            parameters,
        )
        return JsonResponse(station)
    except Exception as e:
        return JsonResponse({"error": str(e)}, status=404)
