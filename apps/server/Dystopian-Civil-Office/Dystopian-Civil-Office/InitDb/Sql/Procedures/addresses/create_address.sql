DROP PROCEDURE IF EXISTS public.create_address(
    varchar, varchar, varchar, varchar, varchar, varchar, integer
);

CREATE OR REPLACE PROCEDURE public.create_address(
    IN p_city varchar(100),
    IN p_street varchar(100),
    IN p_house_number varchar(20),
    IN p_apartment_number varchar(20),
    IN p_postal_code varchar(15),
    IN p_country varchar(60),
    IN p_document_id integer
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_city varchar(100);
    v_street varchar(100);
    v_house_number varchar(20);
    v_apartment_number varchar(20);
    v_postal_code varchar(15);
    v_country varchar(60);
    v_document_id integer;
BEGIN
    SELECT
        v.city,
        v.street,
        v.house_number,
        v.apartment_number,
        v.postal_code,
        v.country,
        v.document_id
    INTO
        v_city,
        v_street,
        v_house_number,
        v_apartment_number,
        v_postal_code,
        v_country,
        v_document_id
    FROM public.validate_address_data(
        p_city,
        p_street,
        p_house_number,
        p_apartment_number,
        p_postal_code,
        p_country,
        p_document_id
    ) v;

    INSERT INTO public.addresses (
        city,
        street,
        house_number,
        apartment_number,
        postal_code,
        country,
        document_id
    )
    VALUES (
        v_city,
        v_street,
        v_house_number,
        v_apartment_number,
        v_postal_code,
        v_country,
        v_document_id
    );

EXCEPTION
    WHEN unique_violation THEN
        RAISE EXCEPTION 'An address with this document already exists.';
    WHEN foreign_key_violation THEN
        RAISE EXCEPTION 'The address contains invalid related data.';
END;
$$;