DROP PROCEDURE IF EXISTS public.create_document(
    varchar, varchar, timestamptz
);

CREATE OR REPLACE PROCEDURE public.create_document(
    IN p_name varchar(100),
    IN p_category varchar(100),
    IN p_import_date timestamptz
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_name varchar(100);
    v_category varchar(100);
    v_import_date timestamptz;
BEGIN
    SELECT v.name, v.category, v.import_date
    INTO v_name, v_category, v_import_date
    FROM public.validate_document_data(
        p_name,
        p_category,
        p_import_date
    ) v;

    INSERT INTO public.documents (
        name,
        category,
        import_date
    )
    VALUES (
        v_name,
        v_category,
        v_import_date
    );

EXCEPTION
    WHEN unique_violation THEN
        RAISE EXCEPTION 'A document with this data already exists.';
END;
$$;