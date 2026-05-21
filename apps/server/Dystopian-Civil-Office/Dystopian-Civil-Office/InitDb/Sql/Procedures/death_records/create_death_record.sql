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