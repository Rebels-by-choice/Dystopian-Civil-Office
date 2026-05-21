DROP PROCEDURE IF EXISTS public.update_address(
    integer, varchar, varchar, varchar, varchar, varchar, varchar, integer
);

CREATE OR REPLACE PROCEDURE public.update_address(
    IN p_address_id integer,
    IN p_city varchar(100) DEFAULT NULL,
    IN p_street varchar(100) DEFAULT NULL,
    IN p_house_number varchar(20) DEFAULT NULL,
    IN p_apartment_number varchar(20) DEFAULT NULL,
    IN p_postal_code varchar(15) DEFAULT NULL,
    IN p_country varchar(60) DEFAULT NULL,
    IN p_document_id integer DEFAULT NULL
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_current_address public.addresses%ROWTYPE;

    v_final_city varchar(100);
    v_final_street varchar(100);
    v_final_house_number varchar(20);
    v_final_apartment_number varchar(20);
    v_final_postal_code varchar(15);
    v_final_country varchar(60);
    v_final_document_id integer;

    v_validated_city varchar(100);
    v_validated_street varchar(100);
    v_validated_house_number varchar(20);
    v_validated_apartment_number varchar(20);
    v_validated_postal_code varchar(15);
    v_validated_country varchar(60);
    v_validated_document_id integer;
BEGIN
    SELECT *
    INTO v_current_address
    FROM public.addresses a
    WHERE a.address_id = p_address_id;

    IF NOT FOUND THEN
        RAISE EXCEPTION 'The address to update was not found.';
    END IF;

    v_final_city := COALESCE(p_city, v_current_address.city);
    v_final_street := COALESCE(p_street, v_current_address.street);
    v_final_house_number := COALESCE(p_house_number, v_current_address.house_number);
    v_final_apartment_number := COALESCE(p_apartment_number, v_current_address.apartment_number);
    v_final_postal_code := COALESCE(p_postal_code, v_current_address.postal_code);
    v_final_country := COALESCE(p_country, v_current_address.country);
    v_final_document_id := COALESCE(p_document_id, v_current_address.document_id);

    SELECT
        v.city,
        v.street,
        v.house_number,
        v.apartment_number,
        v.postal_code,
        v.country,
        v.document_id
    INTO
        v_validated_city,
        v_validated_street,
        v_validated_house_number,
        v_validated_apartment_number,
        v_validated_postal_code,
        v_validated_country,
        v_validated_document_id
    FROM public.validate_address_data(
        v_final_city,
        v_final_street,
        v_final_house_number,
        v_final_apartment_number,
        v_final_postal_code,
        v_final_country,
        v_final_document_id
    ) v;

    UPDATE public.addresses
    SET
        city = v_validated_city,
        street = v_validated_street,
        house_number = v_validated_house_number,
        apartment_number = v_validated_apartment_number,
        postal_code = v_validated_postal_code,
        country = v_validated_country,
        document_id = v_validated_document_id
    WHERE address_id = p_address_id;

    IF NOT FOUND THEN
        RAISE EXCEPTION 'The address to update was not found.';
    END IF;

EXCEPTION
    WHEN unique_violation THEN
        RAISE EXCEPTION 'An address with this document already exists.';
    WHEN foreign_key_violation THEN
        RAISE EXCEPTION 'The address contains invalid related data.';
END;
$$;