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
