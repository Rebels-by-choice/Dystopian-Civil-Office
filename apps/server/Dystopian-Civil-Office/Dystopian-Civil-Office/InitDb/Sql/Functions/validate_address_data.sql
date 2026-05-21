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