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