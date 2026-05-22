DROP FUNCTION IF EXISTS public.validate_address_data(
    varchar, varchar, varchar, varchar, varchar, varchar, integer
);

CREATE OR REPLACE FUNCTION public.validate_address_data(
    p_city varchar(100),
    p_street varchar(100),
    p_house_number varchar(20),
    p_apartment_number varchar(20),
    p_postal_code varchar(15),
    p_country varchar(60),
    p_document_id integer
)
RETURNS TABLE (
    city varchar(100),
    street varchar(100),
    house_number varchar(20),
    apartment_number varchar(20),
    postal_code varchar(15),
    country varchar(60),
    document_id integer
)
LANGUAGE plpgsql
AS $$
BEGIN
    IF NULLIF(btrim(p_city), '') IS NULL THEN
        RAISE EXCEPTION 'The city is required.';
    END IF;

    IF length(btrim(p_city)) > 100 THEN
        RAISE EXCEPTION 'The city cannot exceed 100 characters.';
    END IF;

    IF NULLIF(btrim(p_street), '') IS NULL THEN
        RAISE EXCEPTION 'The street is required.';
    END IF;

    IF length(btrim(p_street)) > 100 THEN
        RAISE EXCEPTION 'The street cannot exceed 100 characters.';
    END IF;

    IF NULLIF(btrim(p_house_number), '') IS NULL THEN
        RAISE EXCEPTION 'The house number is required.';
    END IF;

    IF length(btrim(p_house_number)) > 20 THEN
        RAISE EXCEPTION 'The house number cannot exceed 20 characters.';
    END IF;

    IF p_apartment_number IS NOT NULL AND length(btrim(p_apartment_number)) > 20 THEN
        RAISE EXCEPTION 'The apartment number cannot exceed 20 characters.';
    END IF;

    IF NULLIF(btrim(p_postal_code), '') IS NULL THEN
        RAISE EXCEPTION 'The postal code is required.';
    END IF;

    IF length(btrim(p_postal_code)) > 15 THEN
        RAISE EXCEPTION 'The postal code cannot exceed 15 characters.';
    END IF;

    IF NULLIF(btrim(p_country), '') IS NULL THEN
        RAISE EXCEPTION 'The country is required.';
    END IF;

    IF length(btrim(p_country)) > 60 THEN
        RAISE EXCEPTION 'The country cannot exceed 60 characters.';
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

    city := btrim(p_city);
    street := btrim(p_street);
    house_number := btrim(p_house_number);

    IF NULLIF(btrim(p_apartment_number), '') IS NOT NULL THEN
        apartment_number := btrim(p_apartment_number);
    ELSE
        apartment_number := NULL;
    END IF;

    postal_code := btrim(p_postal_code);
    country := btrim(p_country);
    document_id := p_document_id;

    RETURN NEXT;
END;
$$;

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

DROP FUNCTION IF EXISTS public.validate_document_data(
    varchar, varchar, timestamptz
);

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

    IF p_import_date IS NULL THEN
        RAISE EXCEPTION 'The import date is required.';
    END IF;

    name := btrim(p_name);
    category := btrim(p_category);
    import_date := p_import_date;

    RETURN NEXT;
END;
$$;

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