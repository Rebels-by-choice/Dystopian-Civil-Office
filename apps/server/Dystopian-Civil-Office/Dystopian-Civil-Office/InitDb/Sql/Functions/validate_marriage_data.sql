DROP FUNCTION IF EXISTS public.validate_record_marriage_data(
    varchar, varchar, varchar, varchar
);

CREATE OR REPLACE FUNCTION public.validate_marriage_record_data(
    p_registry_number varchar(50),
    p_spouse1_pesel varchar(11),
    p_spouse2_pesel varchar(11),
    p_document_name varchar(100)
)
RETURNS TABLE (
    spouse1_id integer,
    spouse2_id integer,
    document_id integer
)
LANGUAGE plpgsql
AS $$
BEGIN
    SELECT p.person_id
    INTO spouse1_id
    FROM public.persons p
    WHERE p.pesel = p_spouse1_pesel;

    IF spouse1_id IS NULL THEN
        RAISE EXCEPTION 'The first spouse was not found.';
    END IF;

    SELECT p.person_id
    INTO spouse2_id
    FROM public.persons p
    WHERE p.pesel = p_spouse2_pesel;

    IF spouse2_id IS NULL THEN
        RAISE EXCEPTION 'The second spouse was not found.';
    END IF;

    IF spouse1_id = spouse2_id THEN
        RAISE EXCEPTION 'Spouses cannot be the same person.';
    END IF;

    IF NULLIF(btrim(p_document_name), '') IS NOT NULL THEN
        SELECT d.document_id
        INTO document_id
        FROM public.documents d
        WHERE d.name = p_document_name;

        IF document_id IS NULL THEN
            RAISE EXCEPTION 'The selected document was not found.';
        END IF;
    ELSE
        document_id := NULL;
    END IF;

    RETURN QUERY
    SELECT spouse1_id, spouse2_id, document_id;
END;
$$;