SELECT * FROM "WaterParameters";

UPDATE "WaterParameters"
SET "Description" = TRIM(
    regexp_replace(
        replace(
            replace(
                replace("Description", 'm', 'µm'),
                '', ' '
            ),
            '|', ' - '
        ),
        '\s+', ' ', 'g'
    )
);

UPDATE "WaterParameters"
SET "Description" = REGEXP_REPLACE("Description", '(\s[A-Za-z]{1,3})$', '...')
WHERE "Description" ~ '\s[A-Za-z]{1,3}$';

UPDATE "WaterParameters"
SET "Description" = REGEXP_REPLACE("Description", '(\s-$)$', '')
WHERE "Description" ~ '\s-$';

UPDATE "WaterParameters"
SET "Description" = TRIM(
    regexp_replace(
        replace(
            replace(
                replace(
                    replace(
                        replace("Description", 'µm', '___MICRON___'),
                        'µm', '___MICRON___'
                    ),
                    'µg', '___MICROGRAM___'
                ),
                'µ', 'm'
            ),
            '___MICRON___', 'µm'
        ),
        '\s+', ' ', 'g'
    )
);

UPDATE "WaterParameters"
SET "Description" = replace("Description", '___MICROGRAM___', 'µg')
WHERE "Description" LIKE '%___MICROGRAM___%';

UPDATE "WaterParameters"
SET "Description" = replace("Description", '', '')
WHERE "Description" LIKE '%%';

UPDATE "WaterParameters"
SET "Description" = replace("Description", '', '')
WHERE "Description" LIKE '%%';

UPDATE "WaterParameters"
SET "Description" = REGEXP_REPLACE("Description", '(,\s\.\.\.|\s-\s\.\.\.|\.\.\.\.\.\.|\s\.\.\.)$', '...')
WHERE "Description" ~ '(,\s\.\.\.|\s-\s\.\.\.|\.\.\.\.\.\.|\s\.\.\.)$';

UPDATE "WaterParameters"
SET "Description" = REGEXP_REPLACE("Description", '(\s[A-Za-z]{1,4})$', '...')
WHERE "Description" ~ '\s[A-Za-z]{1,4}$';

UPDATE "WaterParameters"
SET "Description" = regexp_replace(
    replace(
        replace(
            replace("Description", '0.45 µm', '___REAL_MICRON___'),
            'µ', 'm'
        ),
        '___REAL_MICRON___', '0.45 µm'
    ),
    '\s+', ' ', 'g'
);

UPDATE "WaterParameters"
SET "Description" = TRIM(
    replace(
        regexp_replace(
            regexp_replace("Description", '(\s[A-Za-z]{1,4})$', '...'),
            '(\s-|\s,|\s\.\.\.\.\.\.)$', '...'
        ),
        '', ''
    )
);

SELECT "Description" FROM "WaterParameters";

UPDATE "WaterParameters"
SET "Description" = REGEXP_REPLACE(
    REGEXP_REPLACE(
        REGEXP_REPLACE(
            REPLACE(
                REPLACE(
                    "Description", 
                    '0.45 µm', '___SAFE_MICRON___'
                ),
                '0.45 m', '___SAFE_MICRON___'
            ),
            '(\d+(?:\.\d+)?)\s?µ\s?C', '\1 °C', 'g'
        ),
        'µ([a-zA-Z])', '\1', 'g'
    ),
    '___SAFE_MICRON___', '0.45 µm', 'g'
)
WHERE "Description" LIKE '%µ%' 
   OR "Description" LIKE '%%';

UPDATE "WaterParameters"
SET "Description" = TRIM(
    regexp_replace(
        replace(
            replace(
                replace(
                    replace(
                        replace("Description", '0.45 µm', '0.45 µm'),
                        '0.45 mm', '0.45 µm'
                    ),
                    'mmemmbrane', 'membrane'
                ),
                'mm', 'm'
            ),
            '0.45 µ', '0.45 µm'
        ),
        '\s+', ' ', 'g'
    )
);

UPDATE "WaterParameters"
SET "Description" = TRIM(
    regexp_replace(
        regexp_replace("Description", '[\uFFFD\u007F]+', 'µ', 'g'),
        '0.45\s?µ\s?m?', '0.45 µm', 'g'
    )
)
WHERE "Description" ~ '[\uFFFD\u007F]' 
   OR "Description" LIKE '%0.45%';

UPDATE "WaterParameters"
SET "Description" = REGEXP_REPLACE(
    REGEXP_REPLACE(
        REGEXP_REPLACE(
            REGEXP_REPLACE(
                "Description",
                ' \((source|IUPAC|trade name)[^)]*\)', '', 'g' 
            ),
            '\[\d+\]', '', 'g'                                 
        ),
        '^([A-Z0-9a-z,+-]+)\s?-\s?([^-\s][^-]+)\s?-\s?(.*)$', '\2 (\1) - \3'
    ),
    '^([A-Z0-9a-z,+-]{4,})\s?,\s?(.*)$', '\2 (\1)'
)
WHERE "Description" ~ '^[A-Z0-9]' 
   OR "Description" LIKE '%source%';

GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA public TO karina_user;
   