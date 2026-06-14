/*
    Section for addresses
*/
DROP PROCEDURE IF EXISTS public.create_address(
    varchar, varchar, varchar, varchar, varchar, varchar, varchar, varchar
);

CREATE OR REPLACE PROCEDURE public.create_address(
    IN p_registry_number varchar(50),
    IN p_city varchar(100),
    IN p_street varchar(100),
    IN p_house_number varchar(20),
    IN p_apartment_number varchar(20),
    IN p_postal_code varchar(15),
    IN p_country varchar(60),
    IN p_document_name varchar(255)
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_registry_number varchar(50);
    v_city varchar(100);
    v_street varchar(100);
    v_house_number varchar(20);
    v_apartment_number varchar(20);
    v_postal_code varchar(15);
    v_country varchar(60);
    v_document_id integer;
BEGIN
    IF NULLIF(btrim(p_document_name), '') IS NOT NULL THEN
        SELECT d.document_id
        INTO v_document_id
        FROM public.documents d
        WHERE d.name = btrim(p_document_name);

        IF v_document_id IS NULL THEN
            RAISE EXCEPTION 'The selected document was not found.';
        END IF;
    ELSE
        v_document_id := NULL;
    END IF;

    SELECT
        v.registry_number,
        v.city,
        v.street,
        v.house_number,
        v.apartment_number,
        v.postal_code,
        v.country,
        v.document_id
    INTO
        v_registry_number,
        v_city,
        v_street,
        v_house_number,
        v_apartment_number,
        v_postal_code,
        v_country,
        v_document_id
    FROM public.validate_address_data(
        p_registry_number,
        p_city,
        p_street,
        p_house_number,
        p_apartment_number,
        p_postal_code,
        p_country,
        v_document_id
    ) v;

    INSERT INTO public.addresses (
        registry_number,
        city,
        street,
        house_number,
        apartment_number,
        postal_code,
        country,
        document_id
    )
    VALUES (
        v_registry_number,
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
        RAISE EXCEPTION 'Data with this document already exists.';
    WHEN foreign_key_violation THEN
        RAISE EXCEPTION 'The address contains invalid related data.';
END;
$$;

DROP PROCEDURE IF EXISTS public.delete_address(integer);

CREATE OR REPLACE PROCEDURE public.delete_address(
    IN p_address_id integer
)
LANGUAGE plpgsql
AS $$
BEGIN
    DELETE FROM public.addresses
    WHERE address_id = p_address_id;

    IF NOT FOUND THEN
        RAISE EXCEPTION 'The address to delete was not found.';
    END IF;
END;
$$;

DROP PROCEDURE IF EXISTS public.update_address(
    integer, varchar, varchar, varchar, varchar, varchar, varchar, varchar, varchar
);

CREATE OR REPLACE PROCEDURE public.update_address(
    IN p_address_id integer,
    IN p_registry_number varchar(50) DEFAULT NULL,
    IN p_city varchar(100) DEFAULT NULL,
    IN p_street varchar(100) DEFAULT NULL,
    IN p_house_number varchar(20) DEFAULT NULL,
    IN p_apartment_number varchar(20) DEFAULT NULL,
    IN p_postal_code varchar(15) DEFAULT NULL,
    IN p_country varchar(60) DEFAULT NULL,
    IN p_document_name varchar(255) DEFAULT NULL
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_current_address public.addresses%ROWTYPE;

    v_final_registry_number varchar(50);
    v_final_city varchar(100);
    v_final_street varchar(100);
    v_final_house_number varchar(20);
    v_final_apartment_number varchar(20);
    v_final_postal_code varchar(15);
    v_final_country varchar(60);
    v_final_document_id integer;

    v_validated_registry_number varchar(50);
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

    v_final_registry_number := COALESCE(NULLIF(btrim(p_registry_number), ''), v_current_address.registry_number);
    v_final_city := COALESCE(NULLIF(btrim(p_city), ''), v_current_address.city);
    v_final_street := COALESCE(NULLIF(btrim(p_street), ''), v_current_address.street);
    v_final_house_number := COALESCE(NULLIF(btrim(p_house_number), ''), v_current_address.house_number);
    v_final_apartment_number := COALESCE(NULLIF(btrim(p_apartment_number), ''), v_current_address.apartment_number);
    v_final_postal_code := COALESCE(NULLIF(btrim(p_postal_code), ''), v_current_address.postal_code);
    v_final_country := COALESCE(NULLIF(btrim(p_country), ''), v_current_address.country);

    IF p_document_name IS NULL THEN
        v_final_document_id := v_current_address.document_id;
    ELSIF NULLIF(btrim(p_document_name), '') IS NULL THEN
        v_final_document_id := NULL;
    ELSE
        SELECT d.document_id
        INTO v_final_document_id
        FROM public.documents d
        WHERE d.name = btrim(p_document_name);

        IF v_final_document_id IS NULL THEN
            RAISE EXCEPTION 'The selected document was not found.';
        END IF;
    END IF;

    SELECT
        v.registry_number,
        v.city,
        v.street,
        v.house_number,
        v.apartment_number,
        v.postal_code,
        v.country,
        v.document_id
    INTO
        v_validated_registry_number,
        v_validated_city,
        v_validated_street,
        v_validated_house_number,
        v_validated_apartment_number,
        v_validated_postal_code,
        v_validated_country,
        v_validated_document_id
    FROM public.validate_address_data(
        v_final_registry_number,
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
        registry_number = v_validated_registry_number,
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
        RAISE EXCEPTION 'Data with this document already exists.';
    WHEN foreign_key_violation THEN
        RAISE EXCEPTION 'The address contains invalid related data.';
END;
$$;