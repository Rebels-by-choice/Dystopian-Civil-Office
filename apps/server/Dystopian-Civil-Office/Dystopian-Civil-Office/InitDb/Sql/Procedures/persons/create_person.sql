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