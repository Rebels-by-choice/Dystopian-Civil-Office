DROP FUNCTION IF EXISTS public.validate_death_record_data(
    varchar, integer, date, varchar, date, varchar, integer
);

CREATE OR REPLACE FUNCTION public.validate_death_record_data(
    p_registry_number varchar(50),
    p_person_id integer,
    p_death_date date,
    p_death_place varchar(100),
    p_registry_date date,
    p_cause_of_death varchar(200),
    p_document_id integer
)
RETURNS TABLE (
    registry_number varchar(50),
    person_id integer,
    death_date date,
    death_place varchar(100),
    registry_date date,
    cause_of_death varchar(200),
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

    IF p_death_date IS NULL THEN
        RAISE EXCEPTION 'The death date is required.';
    END IF;

    IF NULLIF(btrim(p_death_place), '') IS NULL THEN
        RAISE EXCEPTION 'The death place is required.';
    END IF;

    IF length(btrim(p_death_place)) > 100 THEN
        RAISE EXCEPTION 'The death place cannot exceed 100 characters.';
    END IF;

    IF p_registry_date IS NULL THEN
        RAISE EXCEPTION 'The registry date is required.';
    END IF;

    IF p_registry_date < p_death_date THEN
        RAISE EXCEPTION 'The registry date cannot be earlier than the death date.';
    END IF;

    IF NULLIF(btrim(p_cause_of_death), '') IS NULL THEN
        RAISE EXCEPTION 'The cause of death is required.';
    END IF;

    IF length(btrim(p_cause_of_death)) > 200 THEN
        RAISE EXCEPTION 'The cause of death cannot exceed 200 characters.';
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

    registry_number := btrim(p_registry_number);
    person_id := p_person_id;
    death_date := p_death_date;
    death_place := btrim(p_death_place);
    registry_date := p_registry_date;
    cause_of_death := btrim(p_cause_of_death);
    document_id := p_document_id;

    RETURN NEXT;
END;
$$;