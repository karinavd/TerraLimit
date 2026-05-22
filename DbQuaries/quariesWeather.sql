
-- Analyze raw data

SELECT column_name, data_type 
FROM information_schema.columns 
WHERE table_name = 'WeatherRecords';

SELECT * FROM "WeatherRecords" LIMIT 10;
SELECT COUNT(*) as total, COUNT(DISTINCT "Id") as unique_ids
FROM "WaterRecords";

SELECT 
    COUNT(*) AS total_records,
    (COUNT(*) - COUNT("Current_TempC")) AS temp_nulls,
    (COUNT(*) - COUNT("Current_AirQuality_Pm25")) AS pm25_nulls,
    (COUNT(*) - COUNT("Current_AirQuality_UsEpaIndex")) AS epa_index_nulls
FROM "WeatherRecords";

SELECT * FROM "WeatherRecords"
WHERE "Current_Humidity" NOT BETWEEN 0 AND 100
   OR "Current_TempC" NOT BETWEEN -60 AND 60
   OR "Current_WindDegree" NOT BETWEEN 0 AND 360
   OR "Location_Lat" = 0.0 AND "Location_Lon" = 0.0;

SELECT "Location_Name", "Location_Localtime", COUNT(*)
FROM "WeatherRecords"
GROUP BY "Location_Name", "Location_Localtime"
HAVING COUNT(*) > 1;

-- Cleaning from duplicated records

DELETE FROM "WaterRecords"
WHERE "Id" NOT IN (
    SELECT MIN("Id")
    FROM "WaterRecords"
    GROUP BY "StationId", "Value", "SampleDate"
);

DELETE FROM "WeatherRecords" a
WHERE a.ctid <> (
    SELECT min(b.ctid)
    FROM "WeatherRecords" b
    WHERE b."Location_Name" = a."Location_Name"
      AND b."Location_Localtime" = a."Location_Localtime"
);

SELECT COUNT(*) FROM "WaterRecords";
SELECT 
    COUNT(DISTINCT "StationId") as unique_stations,
    MIN("SampleDate") as date_from,
    MAX("SampleDate") as date_to
FROM "WaterRecords";

-- Create new tables for weather data

CREATE TABLE IF NOT EXISTS "Weather_Locations" AS
SELECT DISTINCT
    "Id" AS "LocationId", 
    "Location_Name" AS "City",
    "Location_Region" AS "Region",
    "Location_Country" AS "Country",
    "Location_Lat" AS "Latitude",
    "Location_Lon" AS "Longitude",
    "Location_TzId" AS "Timezone"
FROM "WeatherRecords";

ALTER TABLE "Weather_Locations" ADD PRIMARY KEY ("LocationId");

DROP TABLE IF EXISTS "Weather_Observations";

CREATE TABLE IF NOT EXISTS "Weather_Observations" (
    "RecordId" INT PRIMARY KEY,
    "LocationId" INT,
    "Localtime" TIMESTAMP,
    "LastUpdated" TIMESTAMP,
    "IsDay" INT,
    "Condition_Text" TEXT,
    "Condition_Code" INT
);

INSERT INTO "Weather_Observations"
SELECT DISTINCT ON ("Id", "Location_Localtime") 
    "Id" AS "RecordId",
    "Id" AS "LocationId",
    "Location_Localtime"::TIMESTAMP,
    "Current_LastUpdated"::TIMESTAMP,
    "Current_IsDay",
    "Current_Condition_Text",
    "Current_Condition_Code"
FROM "WeatherRecords"
WHERE TRIM("Current_Condition_Text") <> '';

DROP TABLE IF EXISTS "Atmosphere_Metrics";

CREATE TABLE IF NOT EXISTS "Atmosphere_Metrics" (
    "RecordId" INT PRIMARY KEY,
    "Temperature_C" NUMERIC,
    "FeelsLike_C" NUMERIC,
    "Humidity_pct" INT,
    "Wind_Speed_kph" NUMERIC,
    "Wind_Degree" INT,
    "Wind_Direction" TEXT,
    "Pressure_mb" NUMERIC,
    "Precipitation_mm" NUMERIC,
    "Cloud_Cover_pct" INT,
    "Uv_Index" NUMERIC
);

INSERT INTO "Atmosphere_Metrics"
SELECT DISTINCT
    "Id" AS "RecordId",
    "Current_TempC", "Current_FeelslikeC", "Current_Humidity", 
    "Current_WindKph", "Current_WindDegree", "Current_WindDir", 
    "Current_PressureMb", "Current_PrecipMm", "Current_Cloud", "Current_Uv"
FROM "WeatherRecords"
WHERE "Current_TempC" BETWEEN -60 AND 60 
  AND "Current_Humidity" BETWEEN 0 AND 100;

DROP TABLE IF EXISTS "AirQuality_Indexes";

CREATE TABLE IF NOT EXISTS "AirQuality_Indexes" (
    "RecordId" INT PRIMARY KEY,
    "CO" NUMERIC, "NO2" NUMERIC, "O3" NUMERIC, "SO2" NUMERIC, 
    "PM25" NUMERIC, "PM10" NUMERIC, 
    "US_EPA_Index" INT, "GB_DEFRA_Index" INT
);

INSERT INTO "AirQuality_Indexes"
SELECT DISTINCT
    "Id" AS "RecordId",
    "Current_AirQuality_Co", "Current_AirQuality_No2", "Current_AirQuality_O3", "Current_AirQuality_So2",
    "Current_AirQuality_Pm25", "Current_AirQuality_Pm10",
    "Current_AirQuality_UsEpaIndex", "Current_AirQuality_GbDefraIndex"
FROM "WeatherRecords"
WHERE "Current_AirQuality_UsEpaIndex" >= 0;

-- Foreign key constraints

ALTER TABLE "Weather_Observations"
ADD CONSTRAINT fk_obs_location 
FOREIGN KEY ("LocationId") 
REFERENCES "Weather_Locations" ("LocationId")
ON DELETE CASCADE;

ALTER TABLE "Atmosphere_Metrics"
ADD CONSTRAINT fk_metrics_obs 
FOREIGN KEY ("RecordId") 
REFERENCES "Weather_Observations" ("RecordId")
ON DELETE CASCADE;

ALTER TABLE "AirQuality_Indexes"
ADD CONSTRAINT fk_aq_obs 
FOREIGN KEY ("RecordId") 
REFERENCES "Weather_Observations" ("RecordId")
ON DELETE CASCADE;

-- Quality control 

SELECT COUNT(*) FROM "Atmosphere_Metrics" WHERE "RecordId" NOT IN (SELECT "RecordId" FROM "Weather_Observations");
SELECT COUNT(*) FROM "AirQuality_Indexes" WHERE "RecordId" NOT IN (SELECT "RecordId" FROM "Weather_Observations");

SELECT * FROM "Weather_Observations" WHERE "Localtime" IS NULL OR TRIM("Condition_Text") = '';

SELECT "LocationId", "Localtime", COUNT(*)
FROM "Weather_Observations"
GROUP BY "LocationId", "Localtime"
HAVING COUNT(*) > 1;

SELECT * FROM "Atmosphere_Metrics" WHERE "Temperature_C" < -90 OR "Temperature_C" > 60;
SELECT * FROM "Atmosphere_Metrics" WHERE "Humidity_pct" < 0 OR "Humidity_pct" > 100;
SELECT * FROM "AirQuality_Indexes" WHERE "US_EPA_Index" < 0 OR "GB_DEFRA_Index" < 0;

SELECT * FROM "Atmosphere_Metrics" WHERE "Temperature_C" IS NULL AND "Humidity_pct" IS NULL;
SELECT * FROM "AirQuality_Indexes" WHERE "CO" IS NULL AND "NO2" IS NULL AND "O3" IS NULL AND "SO2" IS NULL AND "PM25" IS NULL AND "PM10" IS NULL;