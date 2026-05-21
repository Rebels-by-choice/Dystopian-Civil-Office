DROP FUNCTION IF EXISTS public.validate_birth_record_data(
    varchar, integer, integer, integer, date, integer
);

CREATE OR REPLACE FUNCTION public.validate_birth_record_data(
    p_registry_number varchar(50),
    p_person_id integer,
    p_mother_id integer,
    p_father_id integer,
    p_registry_date date,
    p_document_id integer
)
RETURNS TABLE (
    registry_number varchar(50),
    person_id integer,
    mother_id integer,
    father_id integer,
    registry_date date,
    document_id integer
)
LANGUAGE plpgsql
AS $$
BEGIN
    IF NULLIF(btrim(p_registry_number), '') IS NULL THEN
        RAISE EXCEPTION 'The registry number is required.';
    END IF;

    IF length(btrim(p_registry_number)) > 50 THEN
        RAISE EXCEPTION 'The registry number cannot exceed 50 characters.';
    END IF;

    IF p_person_id IS NULL THEN
        RAISE EXCEPTION 'The person is required.';
    END IF;

    IF NOT EXISTS (
        SELECT 1
        FROM public.persons p
        WHERE p.person_id = p_person_id
    ) THEN
        RAISE EXCEPTION 'The selected person was not found.';
    END IF;

    IF p_mother_id IS NOT NULL THEN
        IF NOT EXISTS (
            SELECT 1
            FROM public.persons p
            WHERE p.person_id = p_mother_id
        ) THEN
            RAISE EXCEPTION 'The selected mother was not found.';
        END IF;
    END IF;

    IF p_father_id IS NOT NULL THEN
        IF NOT EXISTS (
            SELECT 1
            FROM public.persons p
            WHERE p.person_id = p_father_id
        ) THEN
            RAISE EXCEPTION 'The selected father was not found.';
        END IF;
    END IF;

    IF p_registry_date IS NULL THEN
        RAISE EXCEPTION 'The registry date is required.';
    END IF;

    IF p_document_id IS NOT NULL THEN
        IF NOT EXISTS (
            SELECT 1
            FROM public.documents d
            WHERE d.document_id = p_document_id
        ) THEN
            RAISE EXCEPTION 'The selected document was not found.';
        END IF;
    END IF;

    IF p_mother_id IS NOT NULL AND p_mother_id = p_person_id THEN
        RAISE EXCEPTION 'The person cannot be their own mother.';
    END IF;

    IF p_father_id IS NOT NULL AND p_father_id = p_person_id THEN
        RAISE EXCEPTION 'The person cannot be their own father.';
    END IF;

    IF p_mother_id IS NOT NULL
       AND p_father_id IS NOT NULL
       AND p_mother_id = p_father_id THEN
        RAISE EXCEPTION 'The mother and father cannot be the same person.';
    END IF;

    registry_number := btrim(p_registry_number);
    person_id := p_person_id;
    mother_id := p_mother_id;
    father_id := p_father_id;
    registry_date := p_registry_date;
    document_id := p_document_id;

    RETURN NEXT;
END;
$$;