import json
from django.shortcuts import render
from django.http import JsonResponse
from django.views.decorators.http import require_GET
from django.conf import settings
import urllib.request
import urllib.parse
import urllib.error
from collections import defaultdict

POLLUTION_PARAMS = {
    "BOD":      30,
    "COD":      100,
    "NH4":      5,
    "NO3":      50,
    "PO4":      10,
    "ATRAZINE": 0.1,
    "TL-TOT":   0.002,
}


def _get(d: dict, key: str, default=None):
    if not isinstance(d, dict):
        return default
    for k, v in d.items():
        if k.lower() == key.lower():
            return v
    return default


def _fetch_page(path: str, offset: int, limit: int) -> list:
    base = getattr(settings, "TERRALIMIT_API_BASE", "http://127.0.0.1:5088")
    qs   = urllib.parse.urlencode({"offset": offset, "limit": limit})
    url  = f"{base}/{path.lstrip('/')}?{qs}"
    req  = urllib.request.Request(url, headers={"Accept": "application/json"})
    try:
        with urllib.request.urlopen(req, timeout=10) as resp:
            raw = resp.read()
            return json.loads(raw.decode("utf-8")) if raw else []
    except urllib.error.HTTPError as e:
        if e.code in [204, 404]:
            return []
        raise


def _fetch_all(path: str, page_size: int = 500) -> list:
    result = []
    offset = 0
    while True:
        try:
            page = _fetch_page(path, offset=offset, limit=page_size)
            if not page:
                break
            result.extend(page)
            if len(page) < page_size:
                break
            offset += page_size
        except urllib.error.HTTPError as e:
            if e.code == 404:
                break
            raise
    return result


def _calc_top20(stations: list, records: list, water_type: str = "") -> list:
    """
    Calculate top-20 most polluted stations.
    If water_type is provided (non-empty), filter stations by that type.
    """
    filtered_stations = stations
    if water_type:
        filtered_stations = [
            s for s in stations
            if (_get(s, "waterType") or "").lower() == water_type.lower()
        ]

    station_map = {_get(s, "id", ""): s for s in filtered_stations if _get(s, "id")}
    by_station = {}

    for r in records:
        sid = _get(r, "stationId")
        if sid not in station_map:
            continue
        code = str(_get(r, "parameterCode") or "").upper()
        if code not in POLLUTION_PARAMS:
            continue
        val = _get(r, "value")
        if val is None:
            continue
        thr        = POLLUTION_PARAMS[code]
        normalized = min(float(val) / thr * 100, 100)
        by_station.setdefault(sid, []).append(normalized)

    result = []
    for sid, scores in by_station.items():
        info = station_map.get(sid, {})
        result.append({
            "station_id":         sid,
            "station_identifier": _get(info, "stationIdentifier") or sid,
            "country_name":       _get(info, "countryName") or "—",
            "water_type":         _get(info, "waterType") or "—",
            "avg_pollution":      round(sum(scores) / len(scores), 1),
            "record_count":       len(scores),
        })
    return sorted(result, key=lambda x: x["avg_pollution"], reverse=True)[:20]


def _calc_depth_profile_all(records: list, station_id: str = "") -> list:
    """
    Return depth profile for ALL pollution parameters combined into one dataset.
    Each depth bucket gets avg values per parameter.
    Returns list of { depth, params: {CODE: avg_value, ...} }
    """
    # Structure: depth -> param_code -> [values]
    buckets: dict = defaultdict(lambda: defaultdict(list))

    for r in records:
        code = str(_get(r, "parameterCode") or "").upper()
        if code not in POLLUTION_PARAMS:
            continue
        if station_id and _get(r, "stationId") != station_id:
            continue
        depth = round(float(_get(r, "depth", 0) or 0), 1)
        val   = _get(r, "value")
        if val is not None:
            buckets[depth][code].append(float(val))

    result = []
    for depth in sorted(buckets.keys()):
        params = {
            code: round(sum(vals) / len(vals), 4)
            for code, vals in buckets[depth].items()
        }
        result.append({"depth": depth, "params": params})
    return result


def _calc_param_summary(records: list, parameters: list) -> list:
    param_name = {_get(p, "id", ""): _get(p, "name", "") for p in parameters if _get(p, "id")}
    agg = defaultdict(list)

    for r in records:
        code = _get(r, "parameterCode")
        val  = _get(r, "value")
        if code and val is not None:
            agg[code].append(float(val))

    result = [
        {
            "code":         code,
            "name":         param_name.get(code) or code,
            "avg_value":    round(sum(vs) / len(vs), 3),
            "record_count": len(vs),
        }
        for code, vs in agg.items()
    ]
    return sorted(result, key=lambda x: x["record_count"], reverse=True)


def _get_water_types(stations: list) -> list:
    """Return sorted unique water types from stations list."""
    types = set()
    for s in stations:
        wt = _get(s, "waterType")
        if wt:
            types.add(wt)
    return sorted(types)


def dashboard(request):
    try:
        stations   = _fetch_all("water/stations")
        records    = _fetch_all("water/records")
        parameters = _fetch_all("water/parameters")
    except Exception as e:
        return render(request, "dashboard/index.html", {
            "error":         str(e),
            "top20":         "[]",
            "param_summary": "[]",
            "depth_profile": "[]",
            "poll_codes":    "[]",
            "water_types":   "[]",
            "stats":         "{}",
        })

    top20         = _calc_top20(stations, records)
    depth_profile = _calc_depth_profile_all(records)
    param_summary = _calc_param_summary(records, parameters)
    poll_codes    = list(POLLUTION_PARAMS.keys())
    water_types   = _get_water_types(stations)

    poll_scores = [s["avg_pollution"] for s in top20]
    stats = {
        "total_stations":      len(stations),
        "total_records":       len(records),
        "total_parameters":    len(parameters),
        "avg_pollution_index": round(sum(poll_scores) / len(poll_scores), 1) if poll_scores else 0,
    }

    return render(request, "dashboard/index.html", {
        "top20":         json.dumps(top20),
        "param_summary": json.dumps(param_summary),
        "depth_profile": json.dumps(depth_profile),
        "poll_codes":    json.dumps(poll_codes),
        "water_types":   json.dumps(water_types),
        "stats":         json.dumps(stats),
    })


@require_GET
def api_depth_profile(request):
    station_id = request.GET.get("station_id", "")

    try:
        records = _fetch_all("water/records")
        data = _calc_depth_profile_all(records, station_id)
        return JsonResponse({"data": data, "station_id": station_id})
    except Exception as e:
        return JsonResponse({"error": str(e)}, status=502)


@require_GET
def api_top20(request):
    """Returns top-20 filtered by water_type query param."""
    water_type = request.GET.get("water_type", "")
    try:
        stations = _fetch_all("water/stations")
        records  = _fetch_all("water/records")
        top20    = _calc_top20(stations, records, water_type)
        return JsonResponse({"data": top20, "water_type": water_type})
    except Exception as e:
        return JsonResponse({"error": str(e)}, status=502)


@require_GET
def api_station_detail(request, station_id):
    try:
        base = getattr(settings, "TERRALIMIT_API_BASE", "http://127.0.0.1:5088")
        url  = f"{base}/water/stations/{urllib.parse.quote(station_id)}"
        req  = urllib.request.Request(url, headers={"Accept": "application/json"})
        with urllib.request.urlopen(req, timeout=8) as resp:
            station = json.loads(resp.read())

        records = _fetch_all("water/records")
        station["parameters"] = _calc_param_summary(
            [r for r in records if _get(r, "stationId") == station_id],
            [],
        )
        return JsonResponse(station)
    except Exception as e:
        return JsonResponse({"error": str(e)}, status=404)


@require_GET
def api_analytics(request):
    """Returns all data for JS dashboard in one request."""
    water_type = request.GET.get("water_type", "")
    try:
        stations   = _fetch_all("water/stations")
        records    = _fetch_all("water/records")
        parameters = _fetch_all("water/parameters")
    except Exception as e:
        return JsonResponse({"error": str(e)}, status=502)

    top20         = _calc_top20(stations, records, water_type)
    depth_profile = _calc_depth_profile_all(records)
    param_summary = _calc_param_summary(records, parameters)
    poll_codes    = list(POLLUTION_PARAMS.keys())
    water_types   = _get_water_types(stations)
    poll_scores   = [s["avg_pollution"] for s in top20]

    return JsonResponse({
        "top20":         top20,
        "param_summary": param_summary,
        "depth_profile": depth_profile,
        "poll_codes":    poll_codes,
        "water_types":   water_types,
        "stats": {
            "total_stations":      len(stations),
            "total_records":       len(records),
            "total_parameters":    len(parameters),
            "avg_pollution_index": round(sum(poll_scores) / len(poll_scores), 1) if poll_scores else 0,
        },
    })