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