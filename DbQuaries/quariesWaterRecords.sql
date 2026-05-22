
-- Check null values and duplicates

SELECT COUNT(*) AS total_rows,
       COUNT("Value") AS value_not_null,
       COUNT("ParameterCode") AS parameter_not_null,
       COUNT("StationId") AS station_not_null
FROM "WaterRecords";

SELECT 
    COUNT(*) AS total_rows,
    COUNT(*) - COUNT("Id") AS id_nulls,
    COUNT(*) - COUNT("Value") AS value_nulls,
    COUNT(*) - COUNT("Depth") AS depth_nulls,
    COUNT(*) - COUNT("ParameterCode") AS parameter_nulls,
    COUNT(*) - COUNT("SampleDate") AS date_nulls,
    COUNT(*) - COUNT("StationId") AS station_nulls,
    COUNT(*) - COUNT("Unit") AS unit_nulls
FROM "WaterRecords";

SELECT COUNT(*) FROM "WaterRecords" WHERE "Value" IS NULL;

SELECT "StationId", "ParameterCode", "SampleDate", "Depth", COUNT(*)
FROM "WaterRecords"
GROUP BY "StationId", "ParameterCode", "SampleDate", "Depth"
HAVING COUNT(*) > 1;

SELECT "Id", "Value", "Depth", "ParameterCode", "SampleDate", "StationId", "Unit", COUNT(*)
FROM "WaterRecords"
GROUP BY "Id", "Value", "Depth", "ParameterCode", "SampleDate", "StationId", "Unit"
HAVING COUNT(*) > 1;

-- Data Cleaning and Validation

SELECT * FROM "WaterRecords"
WHERE "SampleDate" = '-infinity' 
   OR "SampleDate" > NOW()
   OR "SampleDate" < '1900-01-01';

SELECT * FROM "WaterRecords" WHERE "Value" < 0 OR "Value" > 1000000;
SELECT * FROM "WaterRecords" WHERE "Value" < 0 OR "Value" = 0;

SELECT "Id", "Value", "ParameterCode", "StationId", "SampleDate"
FROM "WaterRecords"
WHERE "Value" < 0 OR "Value" > 1000000
ORDER BY "Value" DESC;

SELECT "Value", "StationId", "SampleDate"
FROM "WaterRecords"
WHERE "ParameterCode" = 'TRANS'
ORDER BY "Value" DESC
LIMIT 100;

SELECT 
     COUNT(DISTINCT "StationId") AS raw_stations,
     COUNT(DISTINCT LOWER(TRIM("StationId"))) AS cleaned_stations,
     COUNT(DISTINCT "ParameterCode") AS raw_parameters,
     COUNT(DISTINCT UPPER(TRIM("ParameterCode"))) AS cleaned_parameters
FROM "WaterRecords";

SELECT * FROM "WaterRecords" WHERE "ParameterCode" = 'TRANS' AND "StationId" LIKE 'NLD%' AND "Value" > 1000000;
UPDATE "WaterRecords" SET "Value" = 0.6 WHERE "Id" = 392421;

-- Unit Standardization

SELECT "Unit", COUNT(*) AS records_count FROM "WaterRecords" GROUP BY "Unit";

SELECT "ParameterCode", COUNT(DISTINCT "Unit") AS unique_units
FROM "WaterRecords"
GROUP BY "ParameterCode"
HAVING COUNT(DISTINCT "Unit") > 1;
SELECT "ParameterCode", "Unit", COUNT(*) AS records_count
FROM "WaterRecords"
WHERE "ParameterCode" IN ('245TP', '2MNPT', '36DCP', '44DDT', 'ACETAMIPRID', 'Al-Tot', 'As-Dis', 'As-Tot', 'B-Tot', 'Ba-Tot')
GROUP BY "ParameterCode", "Unit"
ORDER BY "ParameterCode", "Unit";

UPDATE "WaterRecords" SET "Unit" = 'ng/l' WHERE "Unit" LIKE '%g/l' AND "Unit" NOT IN ('ng/l', 'mg/l');
UPDATE "WaterRecords" SET "Unit" = '°C' WHERE "Unit" LIKE '%C';
UPDATE "WaterRecords" SET "Unit" = 'm³/s' WHERE "Unit" LIKE 'm%/s';
UPDATE "WaterRecords" SET "Unit" = 'mg/m³' WHERE "Unit" LIKE 'mg/m%';
UPDATE "WaterRecords" SET "Unit" = 'µS/cm' WHERE "Unit" LIKE '%S/cm';

UPDATE "WaterRecords" SET "Value" = "Value" / 1000000.0, "Unit" = 'mg/l' WHERE "Unit" = 'ng/l';

SELECT "Unit", COUNT(*) AS records_count FROM "WaterRecords" WHERE "Unit" IN ('1/ml', '1/100 ml', 'ml', '100 ml') GROUP BY "Unit";
UPDATE "WaterRecords" SET "Value" = "Value" * 100.0, "Unit" = 'ml' WHERE "Unit" = '1/100 ml';
UPDATE "WaterRecords" SET "Unit" = 'ml' WHERE "Unit" = '1/ml';

UPDATE "WaterRecords" SET "Unit" = 'dimensionless' WHERE "Unit" = '---' AND "ParameterCode" IN ('pH', 'A340', 'SAR', 'COL-App', 'Sal');

-- Schema Refactoring

ALTER TABLE "WaterRecords" ALTER COLUMN "Value" TYPE NUMERIC;

ALTER TABLE "WaterRecords" ALTER COLUMN "SampleDate" TYPE DATE USING "SampleDate"::DATE;

-- Creation of Physics, Chemistry, and Biology tables

SELECT "Unit", "ParameterCode", COUNT(*) AS records_count
FROM "WaterRecords"
WHERE "Unit" IN ('°C', 'µS/cm', 'dimensionless', 'm', 'm³/s', 'NTU', 'Hu', 'JCU', 'TCU', 'ppt', 'psu')
GROUP BY "Unit", "ParameterCode"
ORDER BY "Unit", "ParameterCode";

CREATE TABLE IF NOT EXISTS "Water_Records_PhysicsUnits" AS
SELECT 
    "StationId",
    "SampleDate"::DATE AS "Date",
    MAX(CASE WHEN "ParameterCode" = 'pH' THEN "Value" END) AS "pH",
    MAX(CASE WHEN "ParameterCode" = 'TEMP' THEN "Value" END) AS "Temperature_Water_C",
    MAX(CASE WHEN "ParameterCode" = 'TEMP-Air' THEN "Value" END) AS "Temperature_Air_C",
    MAX(CASE WHEN "ParameterCode" = 'EC' THEN "Value" END) AS "Conductivity_EC_uS_cm",
    MAX(CASE WHEN "ParameterCode" = 'Sal' THEN "Value" END) AS "Salinity",
    MAX(CASE WHEN "ParameterCode" = 'TURB' THEN "Value" END) AS "Turbidity_NTU",
    MAX(CASE WHEN "ParameterCode" = 'TRANS' THEN "Value" END) AS "Transparency_m",
    MAX(CASE WHEN "ParameterCode" = 'COL-App' THEN "Value" END) AS "Color_Apparent",
    MAX(CASE WHEN "ParameterCode" = 'COL-True' THEN "Value" END) AS "Color_True_TCU",
    MAX(CASE WHEN "ParameterCode" = 'Q-Inst' THEN "Value" END) AS "Discharge_Instant_m3_s",
    MAX(CASE WHEN "ParameterCode" = 'SAR' THEN "Value" END) AS "SAR_index",
    MAX(CASE WHEN "ParameterCode" = 'A340' THEN "Value" END) AS "Absorbance_A340"
FROM "WaterRecords"
GROUP BY "StationId", "SampleDate"::DATE;

CREATE TABLE IF NOT EXISTS "Water_Records_ChemistryUnits" AS
SELECT 
    "StationId",
    "SampleDate"::DATE AS "Date",
    MAX(CASE WHEN "ParameterCode" = 'O2-Dis-Sat' THEN "Value" END) AS "Oxygen_Saturation_pct",
    MAX(CASE WHEN "ParameterCode" = 'O2-Dis' THEN "Value" END) AS "Dissolved_Oxygen_mg_l",
    MAX(CASE WHEN "ParameterCode" = 'BOD' THEN "Value" END) AS "BOD_mg_l",
    MAX(CASE WHEN "ParameterCode" = 'COD' THEN "Value" END) AS "COD_mg_l",
    MAX(CASE WHEN "ParameterCode" = 'TOC' THEN "Value" END) AS "Total_Organic_Carbon_mg_l",
    MAX(CASE WHEN "ParameterCode" = 'NO3N' THEN "Value" END) AS "Nitrate_Nitrogen_mg_l",
    MAX(CASE WHEN "ParameterCode" = 'NH4N' THEN "Value" END) AS "Ammonium_Nitrogen_mg_l",
    MAX(CASE WHEN "ParameterCode" = 'TP' THEN "Value" END) AS "Total_Phosphorus_mg_l",
    MAX(CASE WHEN "ParameterCode" = 'TSS' THEN "Value" END) AS "Total_Suspended_Solids_mg_l",
    MAX(CASE WHEN "ParameterCode" = 'TDS' THEN "Value" END) AS "Total_Dissolved_Solids_mg_l",
    MAX(CASE WHEN "ParameterCode" = 'Cl-Tot' THEN "Value" END) AS "Chlorides_mg_l",
    MAX(CASE WHEN "ParameterCode" = 'SO4-Tot' THEN "Value" END) AS "Sulfates_mg_l",
    MAX(CASE WHEN "ParameterCode" = 'Pb-Tot' THEN "Value" END) AS "Lead_Total_mg_l",
    MAX(CASE WHEN "ParameterCode" = 'Cd-Tot' THEN "Value" END) AS "Cadmium_Total_mg_l",
    MAX(CASE WHEN "ParameterCode" = 'As-Tot' THEN "Value" END) AS "Arsenic_Total_mg_l",
    MAX(CASE WHEN "ParameterCode" = 'Hg-Tot' THEN "Value" END) AS "Mercury_Total_mg_l",
    MAX(CASE WHEN "ParameterCode" = 'Cr-Tot' THEN "Value" END) AS "Chromium_Total_mg_l",
    MAX(CASE WHEN "ParameterCode" = 'Ni-Tot' THEN "Value" END) AS "Nickel_Total_mg_l",
    MAX(CASE WHEN "ParameterCode" = 'Fe-Tot' THEN "Value" END) AS "Iron_Total_mg_l",
    MAX(CASE WHEN "ParameterCode" = 'Cu-Tot' THEN "Value" END) AS "Copper_Total_mg_l",
    MAX(CASE WHEN "ParameterCode" = 'Zn-Tot' THEN "Value" END) AS "Zinc_Total_mg_l"
FROM "WaterRecords"
GROUP BY "StationId", "SampleDate"::DATE;

CREATE TABLE IF NOT EXISTS "Water_Records_Biology" AS
SELECT 
    "StationId",
    "SampleDate"::DATE AS "Date",
    MAX(CASE WHEN "ParameterCode" = 'ECOLI' THEN "Value" END) AS "EColi_per_ml",
    MAX(CASE WHEN "ParameterCode" = 'ENTCOCC' THEN "Value" END) AS "Enterococci_per_ml",
    MAX(CASE WHEN "ParameterCode" = 'FECALCOLI' THEN "Value" END) AS "Fecal_Coliforms_per_ml",
    MAX(CASE WHEN "ParameterCode" = 'FECALSTREP' THEN "Value" END) AS "Fecal_Streptococci_per_ml",
    MAX(CASE WHEN "ParameterCode" = 'TOTCOLI' THEN "Value" END) AS "Total_Coliforms_per_ml",
    MAX(CASE WHEN "ParameterCode" = 'TOTMESO' THEN "Value" END) AS "Total_Mesophilic_per_ml",
    MAX(CASE WHEN "ParameterCode" = 'Phyt-B' THEN "Value" END) AS "Phytoplankton_Biomass_mg_m3"
FROM "WaterRecords"
GROUP BY "StationId", "SampleDate"::DATE;

-- Post-processing & Quality Control

DELETE FROM "Water_Records_PhysicsUnits"
WHERE "pH" IS NULL 
  AND "Temperature_Water_C" IS NULL 
  AND "Conductivity_EC_uS_cm" IS NULL 
  AND "Turbidity_NTU" IS NULL;

ALTER TABLE "Water_Records_ChemistryUnits" 
DROP COLUMN IF EXISTS "Oxygen_Saturation_pct",
DROP COLUMN IF EXISTS "COD_mg_l",
DROP COLUMN IF EXISTS "Total_Organic_Carbon_mg_l",
DROP COLUMN IF EXISTS "Total_Suspended_Solids_mg_l",
DROP COLUMN IF EXISTS "Total_Dissolved_Solids_mg_l",
DROP COLUMN IF EXISTS "Chlorides_mg_l",
DROP COLUMN IF EXISTS "Sulfates_mg_l",
DROP COLUMN IF EXISTS "Lead_Total_mg_l",
DROP COLUMN IF EXISTS "Cadmium_Total_mg_l",
DROP COLUMN IF EXISTS "Arsenic_Total_mg_l",
DROP COLUMN IF EXISTS "Mercury_Total_mg_l",
DROP COLUMN IF EXISTS "Chromium_Total_mg_l",
DROP COLUMN IF EXISTS "Nickel_Total_mg_l",
DROP COLUMN IF EXISTS "Copper_Total_mg_l",
DROP COLUMN IF EXISTS "Zinc_Total_mg_l";

DELETE FROM "Water_Records_ChemistryUnits"
WHERE "Dissolved_Oxygen_mg_l" IS NULL 
  AND "BOD_mg_l" IS NULL 
  AND "Nitrate_Nitrogen_mg_l" IS NULL 
  AND "Ammonium_Nitrogen_mg_l" IS NULL 
  AND "Total_Phosphorus_mg_l" IS NULL
  AND "Iron_Total_mg_l" IS NULL;

SELECT 
    COUNT(*) AS "total_rows",
    (COUNT(*) - COUNT("EColi_per_ml")) AS "EColi_NULLs",
    (COUNT(*) - COUNT("Enterococci_per_ml")) AS "Enterococci_NULLs"
FROM "Water_Records_Biology";

DELETE FROM "Water_Records_Biology"
WHERE "EColi_per_ml" IS NULL
  AND "Enterococci_per_ml" IS NULL
  AND "Fecal_Coliforms_per_ml" IS NULL
  AND "Fecal_Streptococci_per_ml" IS NULL
  AND "Total_Coliforms_per_ml" IS NULL
  AND "Total_Mesophilic_per_ml" IS NULL
  AND "Phytoplankton_Biomass_mg_m3" IS NULL;

SELECT * FROM "Water_Records_PhysicsUnits" LIMIT 10;
SELECT * FROM "Water_Records_ChemistryUnits" LIMIT 10;
SELECT * FROM "Water_Records_Biology" LIMIT 10;