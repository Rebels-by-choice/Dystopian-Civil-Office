CREATE OR REPLACE FUNCTION public.validate_document_data(
    p_name varchar(100),
    p_category varchar(100),
    p_import_date timestamptz
)
RETURNS TABLE (
    name varchar(100),
    category varchar(100),
    import_date timestamptz
)
LANGUAGE plpgsql
AS $$
BEGIN
    IF NULLIF(btrim(p_name), '') IS NULL THEN
        RAISE EXCEPTION 'The document name is required.';
    END IF;

    IF length(btrim(p_name)) > 100 THEN
        RAISE EXCEPTION 'The document name cannot exceed 100 characters.';
    END IF;

    IF NULLIF(btrim(p_category), '') IS NULL THEN
        RAISE EXCEPTION 'The document category is required.';
    END IF;

    IF length(btrim(p_category)) > 100 THEN
        RAISE EXCEPTION 'The document category cannot exceed 100 characters.';
    END IF;

    IF p_import_date IS NULL OR p_import_date = '-infinity'::timestamp THEN
        RAISE EXCEPTION 'The import date is required.';
    END IF;

    name := btrim(p_name);
    category := btrim(p_category);
    import_date := p_import_date;

    RETURN NEXT;
END;
$$;