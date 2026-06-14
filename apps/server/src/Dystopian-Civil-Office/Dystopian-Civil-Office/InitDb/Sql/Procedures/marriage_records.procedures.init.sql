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
    v_registry_number varchar(50);
    v_spouse1_id integer;
    v_spouse2_id integer;
    v_marriage_date date;
    v_marriage_place varchar(100);
    v_registry_date date;
    v_document_id integer;
BEGIN
    SELECT p.person_id INTO v_spouse1_id
    FROM public.persons p
    WHERE p.pesel = btrim(p_spouse1_pesel);

    IF v_spouse1_id IS NULL THEN
        RAISE EXCEPTION 'The first spouse was not found.';
    END IF;

    SELECT p.person_id INTO v_spouse2_id
    FROM public.persons p
    WHERE p.pesel = btrim(p_spouse2_pesel);

    IF v_spouse2_id IS NULL THEN
        RAISE EXCEPTION 'The second spouse was not found.';
    END IF;

    IF NULLIF(btrim(p_document_name), '') IS NOT NULL THEN
        SELECT d.document_id INTO v_document_id
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
        v.spouse1_id,
        v.spouse2_id,
        v.marriage_date,
        v.marriage_place,
        v.registry_date,
        v.document_id
    INTO
        v_registry_number,
        v_spouse1_id,
        v_spouse2_id,
        v_marriage_date,
        v_marriage_place,
        v_registry_date,
        v_document_id
    FROM public.validate_marriage_record_data(
        p_registry_number,
        v_spouse1_id,
        v_spouse2_id,
        p_marriage_date,
        p_marriage_place,
        p_registry_date,
        v_document_id
    ) v;

    INSERT INTO public.marriage_records (
        registry_number,
        registry_date,
        spouse1_id,
        spouse2_id,
        marriage_date,
        marriage_place,
        document_id
    )
    VALUES (
        v_registry_number,
        v_registry_date,
        v_spouse1_id,
        v_spouse2_id,
        v_marriage_date,
        v_marriage_place,
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
    integer, varchar, date, varchar, varchar, date, varchar, varchar
);

CREATE OR REPLACE PROCEDURE public.update_marriage_record(
    IN p_marriage_record_id integer,
    IN p_registry_number varchar(50) DEFAULT NULL,
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
    v_final_spouse1_id integer;
    v_final_spouse2_id integer;
    v_final_document_id integer;
    v_validated_registry_number varchar(50);
    v_validated_spouse1_id integer;
    v_validated_spouse2_id integer;
    v_validated_marriage_date date;
    v_validated_marriage_place varchar(100);
    v_validated_registry_date date;
    v_validated_document_id integer;
BEGIN
    SELECT *
    INTO v_current_record
    FROM public.marriage_records mr
    WHERE mr.marriage_record_id = p_marriage_record_id;

    IF NOT FOUND THEN
        RAISE EXCEPTION 'The marriage record to update was not found.';
    END IF;

    v_final_registry_number := COALESCE(NULLIF(btrim(p_registry_number), ''), v_current_record.registry_number);
    v_final_registry_date := COALESCE(p_registry_date, v_current_record.registry_date);
    v_final_marriage_date := COALESCE(p_marriage_date, v_current_record.marriage_date);
    v_final_marriage_place := COALESCE(NULLIF(btrim(p_marriage_place), ''), v_current_record.marriage_place);

    IF p_spouse1_pesel IS NULL THEN
        v_final_spouse1_id := v_current_record.spouse1_id;
    ELSE
        SELECT p.person_id INTO v_final_spouse1_id
        FROM public.persons p
        WHERE p.pesel = btrim(p_spouse1_pesel);

        IF v_final_spouse1_id IS NULL THEN
            RAISE EXCEPTION 'The first spouse was not found.';
        END IF;
    END IF;

    IF p_spouse2_pesel IS NULL THEN
        v_final_spouse2_id := v_current_record.spouse2_id;
    ELSE
        SELECT p.person_id INTO v_final_spouse2_id
        FROM public.persons p
        WHERE p.pesel = btrim(p_spouse2_pesel);

        IF v_final_spouse2_id IS NULL THEN
            RAISE EXCEPTION 'The second spouse was not found.';
        END IF;
    END IF;

    IF p_document_name IS NULL THEN
        v_final_document_id := v_current_record.document_id;
    ELSIF NULLIF(btrim(p_document_name), '') IS NULL THEN
        v_final_document_id := NULL;
    ELSE
        SELECT d.document_id INTO v_final_document_id
        FROM public.documents d
        WHERE d.name = btrim(p_document_name);

        IF v_final_document_id IS NULL THEN
            RAISE EXCEPTION 'The selected document was not found.';
        END IF;
    END IF;

    SELECT
        v.registry_number,
        v.spouse1_id,
        v.spouse2_id,
        v.marriage_date,
        v.marriage_place,
        v.registry_date,
        v.document_id
    INTO
        v_validated_registry_number,
        v_validated_spouse1_id,
        v_validated_spouse2_id,
        v_validated_marriage_date,
        v_validated_marriage_place,
        v_validated_registry_date,
        v_validated_document_id
    FROM public.validate_marriage_record_data(
        v_final_registry_number,
        v_final_spouse1_id,
        v_final_spouse2_id,
        v_final_marriage_date,
        v_final_marriage_place,
        v_final_registry_date,
        v_final_document_id
    ) v;

    UPDATE public.marriage_records
    SET
        registry_number = v_validated_registry_number,
        registry_date = v_validated_registry_date,
        spouse1_id = v_validated_spouse1_id,
        spouse2_id = v_validated_spouse2_id,
        marriage_date = v_validated_marriage_date,
        marriage_place = v_validated_marriage_place,
        document_id = v_validated_document_id
    WHERE marriage_record_id = p_marriage_record_id;

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