
-- Check for null values and find anomalies

SELECT COUNT(*) as total FROM "WaterStations";

SELECT * FROM "WaterStations";
SELECT
  COUNT(*) - COUNT("CountryName") as country_nulls,
  COUNT(*) - COUNT("WaterType") as watertype_nulls,
  COUNT(*) - COUNT("StationIdentifier") as station_nulls,
  COUNT(*) - COUNT("WaterBodyName") as waterbody_nulls
FROM "WaterStations";

SELECT "Id", COUNT(*)
FROM "WaterStations"
GROUP BY "Id"
HAVING COUNT(*) > 1;

SELECT "StationIdentifier", COUNT(*)
FROM "WaterStations"
GROUP BY "StationIdentifier"
HAVING COUNT(*) > 1;

SELECT "Id", "StationIdentifier", "CountryName", "WaterBodyName"
FROM "WaterStations"
WHERE "StationIdentifier" = 'SEE NAMETEXT'
LIMIT 10;

SELECT COUNT(*) 
FROM "WaterStations"
WHERE "WaterBodyName" IS NULL OR "WaterBodyName" = '';

SELECT COUNT(*)
FROM "WaterStations"
WHERE ("WaterBodyName" IS NULL OR "WaterBodyName" = '')
AND "StationIdentifier" NOT IN ('SEE NAMETEXT', 'UNKNOWN', 'NOTAPPLICABLE');

-- Encoding correction

UPDATE "WaterStations"
SET "StationIdentifier" = NULL
WHERE "StationIdentifier" IN ('SEE NAMETEXT', 'UNKNOWN', 'NOTAPPLICABLE');

UPDATE "WaterStations"
SET "WaterBodyName" = NULL
WHERE "WaterBodyName" IN ('SEE NAMETEXT', 'UNKNOWN', 'NOTAPPLICABLE', ' ');

UPDATE "WaterStations"
SET "CountryName" = 'Netherlands'
WHERE "CountryName" = 'Netherlands (-the )';

SELECT "Id", "StationIdentifier"
FROM "WaterStations"
WHERE "StationIdentifier" LIKE '%?%' 
   OR "StationIdentifier" LIKE '%\u00%';

SELECT "Id", "StationIdentifier"
FROM "WaterStations"
WHERE "StationIdentifier" LIKE '%\u00%'
   OR "StationIdentifier" ~ '[^\x00-\x7F]';

UPDATE "WaterStations"
SET "StationIdentifier" = regexp_replace("StationIdentifier", '[^\x20-\x7E]', '', 'g')
WHERE "StationIdentifier" LIKE '%\u00%'   
   OR "StationIdentifier" ~ '[^\x00-\x7F]';

-- Create a cleaned version of the "WaterStations" table

SELECT "Id", "StationIdentifier", "CountryName", "WaterBodyName", "WaterType"
FROM "WaterStations"
LIMIT 200;

CREATE TABLE IF NOT EXISTS "WaterStations_Clean" AS
SELECT 
    "Id",
    "CountryName",
    "WaterType",
    "StationIdentifier"
FROM "WaterStations"
WHERE "StationIdentifier" NOT IN ('SEE NAMETEXT', 'UNKNOWN', 'NOTAPPLICABLE')
AND "StationIdentifier" IS NOT NULL;

-- Analyze the cleaned "WaterStations" table

SELECT * FROM "WaterStations_Clean";

SELECT 
  COUNT(CASE WHEN "StationIdentifier" ~ '^[A-Z]{2}[A-Z0-9]+[0-9]{4,}' THEN 1 END) AS technical_codes,
  COUNT(CASE WHEN "StationIdentifier" ~ '^[0-9]' THEN 1 END) AS numeric_codes,
  COUNT(CASE WHEN "StationIdentifier" ~ '.*(River|Lake|Dam|Creek|Brook).*' THEN 1 END) AS descriptive_names,
  COUNT(*) AS total
FROM "WaterStations_Clean";

ALTER TABLE "WaterStations_Clean" ADD COLUMN "IdentifierType" text;

UPDATE "WaterStations_Clean"
SET "IdentifierType" = CASE
  WHEN "StationIdentifier" ~ '^[A-Z]{2}[A-Z0-9]+[0-9]{4,}' THEN 'technical_code'
  WHEN "StationIdentifier" ~ '^[0-9]' THEN 'numeric_code'
  WHEN "StationIdentifier" ~ '.*(River|Lake|Dam|Creek|Brook).*' THEN 'descriptive_name'
  ELSE 'short_name'
END;

SELECT DISTINCT "WaterType", COUNT(*) 
FROM "WaterStations_Clean"
GROUP BY "WaterType";

UPDATE "WaterStations_Clean"
SET "WaterType" = CASE
  WHEN "WaterType" = 'Groundwater station' THEN 'Groundwater'
  WHEN "WaterType" = 'Lake station' THEN 'Lake'
  WHEN "WaterType" = 'Reservoir station' THEN 'Reservoir'
  WHEN "WaterType" = 'River station' THEN 'River'
  WHEN "WaterType" = 'Wetland station' THEN 'Wetland'
END;

SELECT DISTINCT LEFT("Id", 3) AS code, "CountryName"
FROM "WaterStations_Clean"
ORDER BY code;