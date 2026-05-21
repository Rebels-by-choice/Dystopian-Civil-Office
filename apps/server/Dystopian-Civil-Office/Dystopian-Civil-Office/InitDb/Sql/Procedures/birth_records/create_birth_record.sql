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