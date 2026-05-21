DROP FUNCTION IF EXISTS public.validate_person_data(
    varchar, varchar, varchar, varchar, varchar, date, varchar, integer, integer
);

CREATE OR REPLACE FUNCTION public.validate_person_data(
    p_pesel varchar(11),
    p_first_name varchar(100),
    p_middle_name varchar(100),
    p_last_name varchar(100),
    p_gender varchar(20),
    p_birth_date date,
    p_birth_place varchar(100),
    p_address_id integer,
    p_document_id integer
)
RETURNS TABLE (
    pesel varchar(11),
    first_name varchar(100),
    middle_name varchar(100),
    last_name varchar(100),
    gender varchar(20),
    birth_date date,
    birth_place varchar(100),
    address_id integer,
    document_id integer
)
LANGUAGE plpgsql
AS $$
BEGIN
    IF NULLIF(btrim(p_pesel), '') IS NULL THEN
        RAISE EXCEPTION 'The PESEL is required.';
    END IF;

    IF btrim(p_pesel) !~ '^[0-9]{11}$' THEN
        RAISE EXCEPTION 'The PESEL must contain exactly 11 digits.';
    END IF;

    IF NULLIF(btrim(p_first_name), '') IS NULL THEN
        RAISE EXCEPTION 'The first name is required.';
    END IF;

    IF length(btrim(p_first_name)) > 100 THEN
        RAISE EXCEPTION 'The first name cannot exceed 100 characters.';
    END IF;

    IF p_middle_name IS NOT NULL AND length(btrim(p_middle_name)) > 100 THEN
        RAISE EXCEPTION 'The middle name cannot exceed 100 characters.';
    END IF;

    IF NULLIF(btrim(p_last_name), '') IS NULL THEN
        RAISE EXCEPTION 'The last name is required.';
    END IF;

    IF length(btrim(p_last_name)) > 100 THEN
        RAISE EXCEPTION 'The last name cannot exceed 100 characters.';
    END IF;

    IF NULLIF(btrim(p_gender), '') IS NULL THEN
        RAISE EXCEPTION 'The gender is required.';
    END IF;

    IF btrim(p_gender) NOT IN ('Male', 'Female', 'Other') THEN
        RAISE EXCEPTION 'The gender must be one of the following values: Male, Female, Other.';
    END IF;

    IF p_birth_date IS NULL THEN
        RAISE EXCEPTION 'The birth date is required.';
    END IF;

    IF NULLIF(btrim(p_birth_place), '') IS NULL THEN
        RAISE EXCEPTION 'The birth place is required.';
    END IF;

    IF length(btrim(p_birth_place)) > 100 THEN
        RAISE EXCEPTION 'The birth place cannot exceed 100 characters.';
    END IF;

    IF p_address_id IS NULL THEN
        RAISE EXCEPTION 'The address is required.';
    END IF;

    IF NOT EXISTS (
        SELECT 1
        FROM public.addresses a
        WHERE a.address_id = p_address_id
    ) THEN
        RAISE EXCEPTION 'The selected address was not found.';
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

    pesel := btrim(p_pesel);
    first_name := btrim(p_first_name);

    IF NULLIF(btrim(p_middle_name), '') IS NOT NULL THEN
        middle_name := btrim(p_middle_name);
    ELSE
        middle_name := NULL;
    END IF;

    last_name := btrim(p_last_name);
    gender := btrim(p_gender);
    birth_date := p_birth_date;
    birth_place := btrim(p_birth_place);
    address_id := p_address_id;
    document_id := p_document_id;

    RETURN NEXT;
END;
$$;