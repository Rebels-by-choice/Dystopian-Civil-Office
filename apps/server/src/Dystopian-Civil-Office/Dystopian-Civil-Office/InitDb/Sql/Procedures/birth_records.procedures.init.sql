DROP PROCEDURE IF EXISTS public.create_birth_record(
    varchar, varchar, varchar, varchar, date, varchar
);

CREATE OR REPLACE PROCEDURE public.create_birth_record(
    IN p_registry_number varchar(50),
    IN p_person_pesel varchar(11),
    IN p_mother_pesel varchar(11),
    IN p_father_pesel varchar(11),
    IN p_registry_date date,
    IN p_document_name varchar(100)
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
    SELECT p.person_id INTO v_person_id
    FROM public.persons p
    WHERE p.pesel = btrim(p_person_pesel);

    IF v_person_id IS NULL THEN
        RAISE EXCEPTION 'The selected person was not found.';
    END IF;

    IF NULLIF(btrim(p_mother_pesel), '') IS NOT NULL THEN
        SELECT p.person_id INTO v_mother_id
        FROM public.persons p
        WHERE p.pesel = btrim(p_mother_pesel);

        IF v_mother_id IS NULL THEN
            RAISE EXCEPTION 'The selected mother was not found.';
        END IF;
    ELSE
        v_mother_id := NULL;
    END IF;

    IF NULLIF(btrim(p_father_pesel), '') IS NOT NULL THEN
        SELECT p.person_id INTO v_father_id
        FROM public.persons p
        WHERE p.pesel = btrim(p_father_pesel);

        IF v_father_id IS NULL THEN
            RAISE EXCEPTION 'The selected father was not found.';
        END IF;
    ELSE
        v_father_id := NULL;
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

    SELECT v.registry_number, v.person_id, v.mother_id, v.father_id, v.registry_date, v.document_id
    INTO v_registry_number, v_person_id, v_mother_id, v_father_id, v_registry_date, v_document_id
    FROM public.validate_birth_record_data(
        p_registry_number,
        v_person_id,
        v_mother_id,
        v_father_id,
        p_registry_date,
        v_document_id
    ) v;

    INSERT INTO public.birth_records (
        registry_number, person_id, mother_id, father_id, registry_date, document_id
    )
    VALUES (
        v_registry_number, v_person_id, v_mother_id, v_father_id, v_registry_date, v_document_id
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
    integer, varchar, varchar, varchar, varchar, date, varchar
);

CREATE OR REPLACE PROCEDURE public.update_birth_record(
    IN p_birth_record_id integer,
    IN p_registry_number varchar(50) DEFAULT NULL,
    IN p_person_pesel varchar(11) DEFAULT NULL,
    IN p_mother_pesel varchar(11) DEFAULT NULL,
    IN p_father_pesel varchar(11) DEFAULT NULL,
    IN p_registry_date date DEFAULT NULL,
    IN p_document_name varchar(100) DEFAULT NULL
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

    v_final_registry_number := COALESCE(NULLIF(btrim(p_registry_number), ''), v_current_birth_record.registry_number);
    v_final_registry_date := COALESCE(p_registry_date, v_current_birth_record.registry_date);

    IF p_person_pesel IS NULL THEN
        v_final_person_id := v_current_birth_record.person_id;
    ELSE
        SELECT p.person_id INTO v_final_person_id
        FROM public.persons p
        WHERE p.pesel = btrim(p_person_pesel);

        IF v_final_person_id IS NULL THEN
            RAISE EXCEPTION 'The selected person was not found.';
        END IF;
    END IF;

    IF p_mother_pesel IS NULL THEN
        v_final_mother_id := v_current_birth_record.mother_id;
    ELSIF NULLIF(btrim(p_mother_pesel), '') IS NULL THEN
        v_final_mother_id := NULL;
    ELSE
        SELECT p.person_id INTO v_final_mother_id
        FROM public.persons p
        WHERE p.pesel = btrim(p_mother_pesel);

        IF v_final_mother_id IS NULL THEN
            RAISE EXCEPTION 'The selected mother was not found.';
        END IF;
    END IF;

    IF p_father_pesel IS NULL THEN
        v_final_father_id := v_current_birth_record.father_id;
    ELSIF NULLIF(btrim(p_father_pesel), '') IS NULL THEN
        v_final_father_id := NULL;
    ELSE
        SELECT p.person_id INTO v_final_father_id
        FROM public.persons p
        WHERE p.pesel = btrim(p_father_pesel);

        IF v_final_father_id IS NULL THEN
            RAISE EXCEPTION 'The selected father was not found.';
        END IF;
    END IF;

    IF p_document_name IS NULL THEN
        v_final_document_id := v_current_birth_record.document_id;
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

    SELECT v.registry_number, v.person_id, v.mother_id, v.father_id, v.registry_date, v.document_id
    INTO v_validated_registry_number, v_validated_person_id, v_validated_mother_id, v_validated_father_id, v_validated_registry_date, v_validated_document_id
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