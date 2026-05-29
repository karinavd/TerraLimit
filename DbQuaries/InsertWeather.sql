INSERT INTO "Weather_Locations" (
    "LocationId",
    "City",
    "Region",
    "Country",
    "Latitude",
    "Longitude",
    "Timezone"
)
SELECT DISTINCT ON ("Id") 
    "Id" AS "LocationId",
    "Location_Name" AS "City",
    "Location_Region" AS "Region",
    "Location_Country" AS "Country",
    "Location_Lat" AS "Latitude",
    "Location_Lon" AS "Longitude",
    "Location_TzId" AS "Timezone"
FROM "WeatherRecords"
ORDER BY "Id"
ON CONFLICT ("LocationId") DO NOTHING;

INSERT INTO "Weather_Observations" (
    "RecordId", 
    "LocationId", 
    "Localtime", 
    "LastUpdated", 
    "Condition_Text", 
    "Condition_Code",
    "Current_Condition_Icon"
)
SELECT DISTINCT ON ("Id", "Location_Localtime") 
    "Id" AS "RecordId",
    "Id" AS "LocationId",
    "Location_Localtime"::TIMESTAMP AS "Localtime",
    "Current_LastUpdated"::TIMESTAMP AS "LastUpdated",                
    "Current_Condition_Text" AS "Condition_Text",       
    "Current_Condition_Code" AS "Condition_Code",
    "Current_Condition_Icon"
FROM "WeatherRecords"
WHERE TRIM("Current_Condition_Text") <> ''
ORDER BY "Id", "Location_Localtime"
ON CONFLICT ("RecordId") DO NOTHING;

INSERT INTO "Atmosphere_Metrics" (
    "RecordId",
    "Temperature_C",
    "FeelsLike_C",
    "Humidity_pct",
    "Wind_Speed_kph",
    "Wind_Degree",
    "Wind_Direction",
    "Pressure_mb",
    "Precipitation_mm",
    "Cloud_Cover_pct",
    "Uv_Index"
)
SELECT 
    "Id" AS "RecordId",                               
    "Current_TempC" AS "Temperature_C",                  
    "Current_FeelslikeC" AS "FeelsLike_C",              
    "Current_Humidity" AS "Humidity_pct",                
    "Current_WindKph" AS "Wind_Speed_kph",                
    "Current_WindDegree" AS "Wind_Degree",              
    "Current_WindDir" AS "Wind_Direction",                
    "Current_PressureMb" AS "Pressure_mb",              
    "Current_PrecipMm" AS "Precipitation_mm",              
    "Current_Cloud" AS "Cloud_Cover_pct",                  
    "Current_Uv" AS "Uv_Index"                      
FROM "WeatherRecords"
ON CONFLICT ("RecordId") DO NOTHING;

INSERT INTO "AirQuality_Indexes" (
    "RecordId",
    "CO",
    "NO2",
    "O3",
    "SO2",
    "PM25",
    "PM10",
    "US_EPA_Index",
    "GB_DEFRA_Index"
)
SELECT DISTINCT ON ("Id") 
    "Id" AS "RecordId",
    "Current_AirQuality_Co" AS "CO",
    "Current_AirQuality_No2" AS "NO2",
    "Current_AirQuality_O3" AS "O3",
    "Current_AirQuality_So2" AS "SO2",
    "Current_AirQuality_Pm25" AS "PM25",
    "Current_AirQuality_Pm10" AS "PM10",
    "Current_AirQuality_UsEpaIndex" AS "US_EPA_Index",
    "Current_AirQuality_GbDefraIndex" AS "GB_DEFRA_Index"
FROM "WeatherRecords"
WHERE "Current_AirQuality_UsEpaIndex" >= 0
ORDER BY "Id"
ON CONFLICT ("RecordId") DO NOTHING;