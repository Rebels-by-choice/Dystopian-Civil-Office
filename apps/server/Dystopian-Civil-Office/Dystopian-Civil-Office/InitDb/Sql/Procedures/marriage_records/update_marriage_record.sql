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