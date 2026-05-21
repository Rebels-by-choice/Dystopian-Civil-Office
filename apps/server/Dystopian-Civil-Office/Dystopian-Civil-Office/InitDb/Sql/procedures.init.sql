/*
    Section for addresses
*/
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

/*
    Section for birth records
*/
DROP PROCEDURE IF EXISTS public.create_birth_record(
    varchar, integer, integer, integer, date, integer
);

CREATE OR REPLACE PROCEDURE public.create_birth_record(
    IN p_registry_number varchar(50),
    IN p_person_id integer,
    IN p_mother_id integer,
    IN p_father_id integer,
    IN p_registry_date date,
    IN p_document_id integer
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_registry_number varchar(50);
    v_person_id integer;
    v_mother_id integer;
    v_father_id integer;
    v_registry_date date;
    v_document_id integer;
BEGIN
    SELECT
        v.registry_number,
        v.person_id,
        v.mother_id,
        v.father_id,
        v.registry_date,
        v.document_id
    INTO
        v_registry_number,
        v_person_id,
        v_mother_id,
        v_father_id,
        v_registry_date,
        v_document_id
    FROM public.validate_birth_record_data(
        p_registry_number,
        p_person_id,
        p_mother_id,
        p_father_id,
        p_registry_date,
        p_document_id
    ) v;

    INSERT INTO public.birth_records (
        registry_number,
        person_id,
        mother_id,
        father_id,
        registry_date,
        document_id
    )
    VALUES (
        v_registry_number,
        v_person_id,
        v_mother_id,
        v_father_id,
        v_registry_date,
        v_document_id
    );

EXCEPTION
    WHEN unique_violation THEN
        RAISE EXCEPTION 'A birth record with this registry number, person, or document already exists.';
    WHEN foreign_key_violation THEN
        RAISE EXCEPTION 'The birth record contains invalid related data.';
END;
$$;

DROP PROCEDURE IF EXISTS public.delete_birth_record(integer);

CREATE OR REPLACE PROCEDURE public.delete_birth_record(
    IN p_birth_record_id integer
)
LANGUAGE plpgsql
AS $$
BEGIN
    DELETE FROM public.birth_records
    WHERE birth_record_id = p_birth_record_id;

    IF NOT FOUND THEN
        RAISE EXCEPTION 'The birth record to delete was not found.';
    END IF;
END;
$$;

DROP PROCEDURE IF EXISTS public.update_birth_record(
    integer, varchar, integer, integer, integer, date, integer
);

CREATE OR REPLACE PROCEDURE public.update_birth_record(
    IN p_birth_record_id integer,
    IN p_registry_number varchar(50) DEFAULT NULL,
    IN p_person_id integer DEFAULT NULL,
    IN p_mother_id integer DEFAULT NULL,
    IN p_father_id integer DEFAULT NULL,
    IN p_registry_date date DEFAULT NULL,
    IN p_document_id integer DEFAULT NULL
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_current_birth_record public.birth_records%ROWTYPE;

    v_final_registry_number varchar(50);
    v_final_person_id integer;
    v_final_mother_id integer;
    v_final_father_id integer;
    v_final_registry_date date;
    v_final_document_id integer;

    v_validated_registry_number varchar(50);
    v_validated_person_id integer;
    v_validated_mother_id integer;
    v_validated_father_id integer;
    v_validated_registry_date date;
    v_validated_document_id integer;
BEGIN
    SELECT *
    INTO v_current_birth_record
    FROM public.birth_records br
    WHERE br.birth_record_id = p_birth_record_id;

    IF NOT FOUND THEN
        RAISE EXCEPTION 'The birth record to update was not found.';
    END IF;

    v_final_registry_number := COALESCE(p_registry_number, v_current_birth_record.registry_number);
    v_final_person_id := COALESCE(p_person_id, v_current_birth_record.person_id);
    v_final_mother_id := COALESCE(p_mother_id, v_current_birth_record.mother_id);
    v_final_father_id := COALESCE(p_father_id, v_current_birth_record.father_id);
    v_final_registry_date := COALESCE(p_registry_date, v_current_birth_record.registry_date);
    v_final_document_id := COALESCE(p_document_id, v_current_birth_record.document_id);

    SELECT
        v.registry_number,
        v.person_id,
        v.mother_id,
        v.father_id,
        v.registry_date,
        v.document_id
    INTO
        v_validated_registry_number,
        v_validated_person_id,
        v_validated_mother_id,
        v_validated_father_id,
        v_validated_registry_date,
        v_validated_document_id
    FROM public.validate_birth_record_data(
        v_final_registry_number,
        v_final_person_id,
        v_final_mother_id,
        v_final_father_id,
        v_final_registry_date,
        v_final_document_id
    ) v;

    UPDATE public.birth_records
    SET
        registry_number = v_validated_registry_number,
        person_id = v_validated_person_id,
        mother_id = v_validated_mother_id,
        father_id = v_validated_father_id,
        registry_date = v_validated_registry_date,
        document_id = v_validated_document_id
    WHERE birth_record_id = p_birth_record_id;

    IF NOT FOUND THEN
        RAISE EXCEPTION 'The birth record to update was not found.';
    END IF;

EXCEPTION
    WHEN unique_violation THEN
        RAISE EXCEPTION 'A birth record with this registry number, person, or document already exists.';
    WHEN foreign_key_violation THEN
        RAISE EXCEPTION 'The birth record contains invalid related data.';
END;
$$;

/*
    Section for death records
*/
DROP PROCEDURE IF EXISTS public.create_death_record(
    varchar, integer, date, varchar, date, varchar, integer
);

CREATE OR REPLACE PROCEDURE public.create_death_record(
    IN p_registry_number varchar(50),
    IN p_person_id integer,
    IN p_death_date date,
    IN p_death_place varchar(100),
    IN p_registry_date date,
    IN p_cause_of_death varchar(200),
    IN p_document_id integer
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_registry_number varchar(50);
    v_person_id integer;
    v_death_date date;
    v_death_place varchar(100);
    v_registry_date date;
    v_cause_of_death varchar(200);
    v_document_id integer;
BEGIN
    SELECT
        v.registry_number,
        v.person_id,
        v.death_date,
        v.death_place,
        v.registry_date,
        v.cause_of_death,
        v.document_id
    INTO
        v_registry_number,
        v_person_id,
        v_death_date,
        v_death_place,
        v_registry_date,
        v_cause_of_death,
        v_document_id
    FROM public.validate_death_record_data(
        p_registry_number,
        p_person_id,
        p_death_date,
        p_death_place,
        p_registry_date,
        p_cause_of_death,
        p_document_id
    ) v;

    INSERT INTO public.death_records (
        registry_number,
        person_id,
        death_date,
        death_place,
        registry_date,
        cause_of_death,
        document_id
    )
    VALUES (
        v_registry_number,
        v_person_id,
        v_death_date,
        v_death_place,
        v_registry_date,
        v_cause_of_death,
        v_document_id
    );

EXCEPTION
    WHEN unique_violation THEN
        RAISE EXCEPTION 'A death record with this registry number, person, or document already exists.';
    WHEN foreign_key_violation THEN
        RAISE EXCEPTION 'The death record contains invalid related data.';
END;
$$;

DROP PROCEDURE IF EXISTS public.delete_death_record(integer);

CREATE OR REPLACE PROCEDURE public.delete_death_record(
    IN p_death_record_id integer
)
LANGUAGE plpgsql
AS $$
BEGIN
    DELETE FROM public.death_records
    WHERE death_record_id = p_death_record_id;

    IF NOT FOUND THEN
        RAISE EXCEPTION 'The death record to delete was not found.';
    END IF;
END;
$$;

DROP PROCEDURE IF EXISTS public.update_death_record(
    integer, varchar, integer, date, varchar, date, varchar, integer
);

CREATE OR REPLACE PROCEDURE public.update_death_record(
    IN p_death_record_id integer,
    IN p_registry_number varchar(50) DEFAULT NULL,
    IN p_person_id integer DEFAULT NULL,
    IN p_death_date date DEFAULT NULL,
    IN p_death_place varchar(100) DEFAULT NULL,
    IN p_registry_date date DEFAULT NULL,
    IN p_cause_of_death varchar(200) DEFAULT NULL,
    IN p_document_id integer DEFAULT NULL
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_current_death_record public.death_records%ROWTYPE;

    v_final_registry_number varchar(50);
    v_final_person_id integer;
    v_final_death_date date;
    v_final_death_place varchar(100);
    v_final_registry_date date;
    v_final_cause_of_death varchar(200);
    v_final_document_id integer;

    v_validated_registry_number varchar(50);
    v_validated_person_id integer;
    v_validated_death_date date;
    v_validated_death_place varchar(100);
    v_validated_registry_date date;
    v_validated_cause_of_death varchar(200);
    v_validated_document_id integer;
BEGIN
    SELECT *
    INTO v_current_death_record
    FROM public.death_records dr
    WHERE dr.death_record_id = p_death_record_id;

    IF NOT FOUND THEN
        RAISE EXCEPTION 'The death record to update was not found.';
    END IF;

    v_final_registry_number := COALESCE(p_registry_number, v_current_death_record.registry_number);
    v_final_person_id := COALESCE(p_person_id, v_current_death_record.person_id);
    v_final_death_date := COALESCE(p_death_date, v_current_death_record.death_date);
    v_final_death_place := COALESCE(p_death_place, v_current_death_record.death_place);
    v_final_registry_date := COALESCE(p_registry_date, v_current_death_record.registry_date);
    v_final_cause_of_death := COALESCE(p_cause_of_death, v_current_death_record.cause_of_death);
    v_final_document_id := COALESCE(p_document_id, v_current_death_record.document_id);

    SELECT
        v.registry_number,
        v.person_id,
        v.death_date,
        v.death_place,
        v.registry_date,
        v.cause_of_death,
        v.document_id
    INTO
        v_validated_registry_number,
        v_validated_person_id,
        v_validated_death_date,
        v_validated_death_place,
        v_validated_registry_date,
        v_validated_cause_of_death,
        v_validated_document_id
    FROM public.validate_death_record_data(
        v_final_registry_number,
        v_final_person_id,
        v_final_death_date,
        v_final_death_place,
        v_final_registry_date,
        v_final_cause_of_death,
        v_final_document_id
    ) v;

    UPDATE public.death_records
    SET
        registry_number = v_validated_registry_number,
        person_id = v_validated_person_id,
        death_date = v_validated_death_date,
        death_place = v_validated_death_place,
        registry_date = v_validated_registry_date,
        cause_of_death = v_validated_cause_of_death,
        document_id = v_validated_document_id
    WHERE death_record_id = p_death_record_id;

    IF NOT FOUND THEN
        RAISE EXCEPTION 'The death record to update was not found.';
    END IF;

EXCEPTION
    WHEN unique_violation THEN
        RAISE EXCEPTION 'A death record with this registry number, person, or document already exists.';
    WHEN foreign_key_violation THEN
        RAISE EXCEPTION 'The death record contains invalid related data.';
END;
$$;

/*
    Section for documents
*/
DROP PROCEDURE IF EXISTS public.create_document(
    varchar, varchar, timestamptz
);

CREATE OR REPLACE PROCEDURE public.create_document(
    IN p_name varchar(100),
    IN p_category varchar(100),
    IN p_import_date timestamptz
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_name varchar(100);
    v_category varchar(100);
    v_import_date timestamptz;
BEGIN
    SELECT v.name, v.category, v.import_date
    INTO v_name, v_category, v_import_date
    FROM public.validate_document_data(
        p_name,
        p_category,
        p_import_date
    ) v;

    INSERT INTO public.documents (
        name,
        category,
        import_date
    )
    VALUES (
        v_name,
        v_category,
        v_import_date
    );

EXCEPTION
    WHEN unique_violation THEN
        RAISE EXCEPTION 'A document with this data already exists.';
END;
$$;

DROP PROCEDURE IF EXISTS public.delete_document(integer);

CREATE OR REPLACE PROCEDURE public.delete_document(
    IN p_document_id integer
)
LANGUAGE plpgsql
AS $$
BEGIN
    DELETE FROM public.documents
    WHERE document_id = p_document_id;

    IF NOT FOUND THEN
        RAISE EXCEPTION 'The document to delete was not found.';
    END IF;

EXCEPTION
    WHEN foreign_key_violation THEN
        RAISE EXCEPTION 'The document cannot be deleted because it is referenced by other data.';
END;
$$;

DROP PROCEDURE IF EXISTS public.update_document(
    integer, varchar, varchar, timestamptz
);

CREATE OR REPLACE PROCEDURE public.update_document(
    IN p_document_id integer,
    IN p_name varchar(100) DEFAULT NULL,
    IN p_category varchar(100) DEFAULT NULL,
    IN p_import_date timestamptz DEFAULT NULL
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_current_document public.documents%ROWTYPE;

    v_final_name varchar(100);
    v_final_category varchar(100);
    v_final_import_date timestamptz;

    v_validated_name varchar(100);
    v_validated_category varchar(100);
    v_validated_import_date timestamptz;
BEGIN
    SELECT *
    INTO v_current_document
    FROM public.documents d
    WHERE d.document_id = p_document_id;

    IF NOT FOUND THEN
        RAISE EXCEPTION 'The document to update was not found.';
    END IF;

    v_final_name := COALESCE(p_name, v_current_document.name);
    v_final_category := COALESCE(p_category, v_current_document.category);
    v_final_import_date := COALESCE(p_import_date, v_current_document.import_date);

    SELECT v.name, v.category, v.import_date
    INTO v_validated_name, v_validated_category, v_validated_import_date
    FROM public.validate_document_data(
        v_final_name,
        v_final_category,
        v_final_import_date
    ) v;

    UPDATE public.documents
    SET
        name = v_validated_name,
        category = v_validated_category,
        import_date = v_validated_import_date
    WHERE document_id = p_document_id;

    IF NOT FOUND THEN
        RAISE EXCEPTION 'The document to update was not found.';
    END IF;

EXCEPTION
    WHEN unique_violation THEN
        RAISE EXCEPTION 'A document with this data already exists.';
END;
$$;

/*
    Section for marriage records
*/
DROP PROCEDURE IF EXISTS public.create_marriage_record(
    varchar, date, varchar, varchar, date, varchar, varchar
);

CREATE OR REPLACE PROCEDURE public.create_marriage_record(
    IN p_registry_number varchar(50),
    IN p_registry_date date,
    IN p_spouse1_pesel varchar(11),
    IN p_spouse2_pesel varchar(11),
    IN p_marriage_date date,
    IN p_marriage_place varchar(100),
    IN p_document_name varchar(100)
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_spouse1_id integer;
    v_spouse2_id integer;
    v_document_id integer;
BEGIN
    SELECT v.spouse1_id, v.spouse2_id, v.document_id
    INTO v_spouse1_id, v_spouse2_id, v_document_id
    FROM public.validate_marriage_record_data(
        p_registry_number,
        p_spouse1_pesel,
        p_spouse2_pesel,
        p_document_name
    ) v;

    INSERT INTO public.marriage_records (
        registry_number,
        spouse1_id,
        spouse2_id,
        marriage_date,
        marriage_place,
        registry_date,
        document_id
    )
    VALUES (
        p_registry_number,
        v_spouse1_id,
        v_spouse2_id,
        p_marriage_date,
        p_marriage_place,
        p_registry_date,
        v_document_id
    );

EXCEPTION
    WHEN unique_violation THEN
        RAISE EXCEPTION 'A marriage record with this data already exists.';
    WHEN foreign_key_violation THEN
        RAISE EXCEPTION 'The marriage record contains invalid related data.';
END;
$$;

DROP PROCEDURE IF EXISTS public.delete_marriage_record(varchar);

CREATE OR REPLACE PROCEDURE public.delete_marriage_record(
    IN p_registry_number varchar(50)
)
LANGUAGE plpgsql
AS $$
BEGIN
    DELETE FROM public.marriage_records
    WHERE registry_number = p_registry_number;

    IF NOT FOUND THEN
        RAISE EXCEPTION 'The marriage record to delete was not found.';
    END IF;

EXCEPTION
    WHEN foreign_key_violation THEN
        RAISE EXCEPTION 'The marriage record cannot be deleted because it is referenced by other data.';
END;
$$;

DROP PROCEDURE IF EXISTS public.update_marriage_record(
    varchar, varchar, date, varchar, varchar, date, varchar, varchar
);

CREATE OR REPLACE PROCEDURE public.update_marriage_record(
    IN p_current_registry_number varchar(50),
    IN p_new_registry_number varchar(50) DEFAULT NULL,
    IN p_registry_date date DEFAULT NULL,
    IN p_spouse1_pesel varchar(11) DEFAULT NULL,
    IN p_spouse2_pesel varchar(11) DEFAULT NULL,
    IN p_marriage_date date DEFAULT NULL,
    IN p_marriage_place varchar(100) DEFAULT NULL,
    IN p_document_name varchar(100) DEFAULT NULL
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_current_record public.marriage_records%ROWTYPE;

    v_final_registry_number varchar(50);
    v_final_registry_date date;
    v_final_marriage_date date;
    v_final_marriage_place varchar(100);

    v_effective_spouse1_pesel varchar(11);
    v_effective_spouse2_pesel varchar(11);
    v_effective_document_name varchar(100);

    v_spouse1_id integer;
    v_spouse2_id integer;
    v_document_id integer;
BEGIN
    SELECT *
    INTO v_current_record
    FROM public.marriage_records mr
    WHERE mr.registry_number = p_current_registry_number;

    IF NOT FOUND THEN
        RAISE EXCEPTION 'The marriage record to update was not found.';
    END IF;

    v_final_registry_number := COALESCE(p_new_registry_number, v_current_record.registry_number);
    v_final_registry_date := COALESCE(p_registry_date, v_current_record.registry_date);
    v_final_marriage_date := COALESCE(p_marriage_date, v_current_record.marriage_date);
    v_final_marriage_place := COALESCE(p_marriage_place, v_current_record.marriage_place);

    IF p_spouse1_pesel IS NOT NULL THEN
        v_effective_spouse1_pesel := p_spouse1_pesel;
    ELSE
        SELECT p.pesel
        INTO v_effective_spouse1_pesel
        FROM public.persons p
        WHERE p.person_id = v_current_record.spouse1_id;
    END IF;

    IF p_spouse2_pesel IS NOT NULL THEN
        v_effective_spouse2_pesel := p_spouse2_pesel;
    ELSE
        SELECT p.pesel
        INTO v_effective_spouse2_pesel
        FROM public.persons p
        WHERE p.person_id = v_current_record.spouse2_id;
    END IF;

    IF p_document_name IS NOT NULL THEN
        v_effective_document_name := p_document_name;
    ELSIF v_current_record.document_id IS NOT NULL THEN
        SELECT d.name
        INTO v_effective_document_name
        FROM public.documents d
        WHERE d.document_id = v_current_record.document_id;
    ELSE
        v_effective_document_name := NULL;
    END IF;

    SELECT v.spouse1_id, v.spouse2_id, v.document_id
    INTO v_spouse1_id, v_spouse2_id, v_document_id
    FROM public.validate_marriage_record_data(
        v_final_registry_number,
        v_effective_spouse1_pesel,
        v_effective_spouse2_pesel,
        v_effective_document_name
    ) v;

    UPDATE public.marriage_records
    SET
        registry_number = v_final_registry_number,
        registry_date = v_final_registry_date,
        spouse1_id = v_spouse1_id,
        spouse2_id = v_spouse2_id,
        marriage_date = v_final_marriage_date,
        marriage_place = v_final_marriage_place,
        document_id = v_document_id
    WHERE registry_number = p_current_registry_number;

    IF NOT FOUND THEN
        RAISE EXCEPTION 'The marriage record to update was not found.';
    END IF;

EXCEPTION
    WHEN unique_violation THEN
        RAISE EXCEPTION 'A marriage record with this data already exists.';
    WHEN foreign_key_violation THEN
        RAISE EXCEPTION 'The marriage record contains invalid related data.';
END;
$$;

/*
    Section for persons
*/
DROP PROCEDURE IF EXISTS public.create_person(
    varchar, varchar, varchar, varchar, varchar, date, varchar, integer, integer
);

CREATE OR REPLACE PROCEDURE public.create_person(
    IN p_pesel varchar(11),
    IN p_first_name varchar(100),
    IN p_middle_name varchar(100),
    IN p_last_name varchar(100),
    IN p_gender varchar(20),
    IN p_birth_date date,
    IN p_birth_place varchar(100),
    IN p_address_id integer,
    IN p_document_id integer
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_pesel varchar(11);
    v_first_name varchar(100);
    v_middle_name varchar(100);
    v_last_name varchar(100);
    v_gender varchar(20);
    v_birth_date date;
    v_birth_place varchar(100);
    v_address_id integer;
    v_document_id integer;
BEGIN
    SELECT
        v.pesel,
        v.first_name,
        v.middle_name,
        v.last_name,
        v.gender,
        v.birth_date,
        v.birth_place,
        v.address_id,
        v.document_id
    INTO
        v_pesel,
        v_first_name,
        v_middle_name,
        v_last_name,
        v_gender,
        v_birth_date,
        v_birth_place,
        v_address_id,
        v_document_id
    FROM public.validate_person_data(
        p_pesel,
        p_first_name,
        p_middle_name,
        p_last_name,
        p_gender,
        p_birth_date,
        p_birth_place,
        p_address_id,
        p_document_id
    ) v;

    INSERT INTO public.persons (
        pesel,
        first_name,
        middle_name,
        last_name,
        gender,
        birth_date,
        birth_place,
        address_id,
        document_id
    )
    VALUES (
        v_pesel,
        v_first_name,
        v_middle_name,
        v_last_name,
        v_gender,
        v_birth_date,
        v_birth_place,
        v_address_id,
        v_document_id
    );

EXCEPTION
    WHEN unique_violation THEN
        RAISE EXCEPTION 'A person with this PESEL or document already exists.';
    WHEN foreign_key_violation THEN
        RAISE EXCEPTION 'The person contains invalid related data.';
END;
$$;

DROP PROCEDURE IF EXISTS public.delete_person(integer);

CREATE OR REPLACE PROCEDURE public.delete_person(
    IN p_person_id integer
)
LANGUAGE plpgsql
AS $$
BEGIN
    DELETE FROM public.persons
    WHERE person_id = p_person_id;

    IF NOT FOUND THEN
        RAISE EXCEPTION 'The person to delete was not found.';
    END IF;

EXCEPTION
    WHEN foreign_key_violation THEN
        RAISE EXCEPTION 'The person cannot be deleted because it is referenced by other data.';
END;
$$;

DROP PROCEDURE IF EXISTS public.update_person(
    integer, varchar, varchar, varchar, varchar, varchar, date, varchar, integer, integer
);

CREATE OR REPLACE PROCEDURE public.update_person(
    IN p_person_id integer,
    IN p_pesel varchar(11) DEFAULT NULL,
    IN p_first_name varchar(100) DEFAULT NULL,
    IN p_middle_name varchar(100) DEFAULT NULL,
    IN p_last_name varchar(100) DEFAULT NULL,
    IN p_gender varchar(20) DEFAULT NULL,
    IN p_birth_date date DEFAULT NULL,
    IN p_birth_place varchar(100) DEFAULT NULL,
    IN p_address_id integer DEFAULT NULL,
    IN p_document_id integer DEFAULT NULL
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_current_person public.persons%ROWTYPE;

    v_final_pesel varchar(11);
    v_final_first_name varchar(100);
    v_final_middle_name varchar(100);
    v_final_last_name varchar(100);
    v_final_gender varchar(20);
    v_final_birth_date date;
    v_final_birth_place varchar(100);
    v_final_address_id integer;
    v_final_document_id integer;

    v_validated_pesel varchar(11);
    v_validated_first_name varchar(100);
    v_validated_middle_name varchar(100);
    v_validated_last_name varchar(100);
    v_validated_gender varchar(20);
    v_validated_birth_date date;
    v_validated_birth_place varchar(100);
    v_validated_address_id integer;
    v_validated_document_id integer;
BEGIN
    SELECT *
    INTO v_current_person
    FROM public.persons p
    WHERE p.person_id = p_person_id;

    IF NOT FOUND THEN
        RAISE EXCEPTION 'The person to update was not found.';
    END IF;

    v_final_pesel := COALESCE(p_pesel, v_current_person.pesel);
    v_final_first_name := COALESCE(p_first_name, v_current_person.first_name);
    v_final_middle_name := COALESCE(p_middle_name, v_current_person.middle_name);
    v_final_last_name := COALESCE(p_last_name, v_current_person.last_name);
    v_final_gender := COALESCE(p_gender, v_current_person.gender);
    v_final_birth_date := COALESCE(p_birth_date, v_current_person.birth_date);
    v_final_birth_place := COALESCE(p_birth_place, v_current_person.birth_place);
    v_final_address_id := COALESCE(p_address_id, v_current_person.address_id);
    v_final_document_id := COALESCE(p_document_id, v_current_person.document_id);

    SELECT
        v.pesel,
        v.first_name,
        v.middle_name,
        v.last_name,
        v.gender,
        v.birth_date,
        v.birth_place,
        v.address_id,
        v.document_id
    INTO
        v_validated_pesel,
        v_validated_first_name,
        v_validated_middle_name,
        v_validated_last_name,
        v_validated_gender,
        v_validated_birth_date,
        v_validated_birth_place,
        v_validated_address_id,
        v_validated_document_id
    FROM public.validate_person_data(
        v_final_pesel,
        v_final_first_name,
        v_final_middle_name,
        v_final_last_name,
        v_final_gender,
        v_final_birth_date,
        v_final_birth_place,
        v_final_address_id,
        v_final_document_id
    ) v;

    UPDATE public.persons
    SET
        pesel = v_validated_pesel,
        first_name = v_validated_first_name,
        middle_name = v_validated_middle_name,
        last_name = v_validated_last_name,
        gender = v_validated_gender,
        birth_date = v_validated_birth_date,
        birth_place = v_validated_birth_place,
        address_id = v_validated_address_id,
        document_id = v_validated_document_id
    WHERE person_id = p_person_id;

    IF NOT FOUND THEN
        RAISE EXCEPTION 'The person to update was not found.';
    END IF;

EXCEPTION
    WHEN unique_violation THEN
        RAISE EXCEPTION 'A person with this PESEL or document already exists.';
    WHEN foreign_key_violation THEN
        RAISE EXCEPTION 'The person contains invalid related data.';
END;
$$;
