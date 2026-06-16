CREATE OR REPLACE FUNCTION public.archive_deleted_address()
RETURNS trigger
LANGUAGE plpgsql
AS $$
DECLARE
    v_document_name varchar(255);
BEGIN
    IF OLD.document_id IS NOT NULL THEN
        SELECT d.document_name
        INTO v_document_name
        FROM public.documents d
        WHERE d.document_id = OLD.document_id;
    ELSE
        v_document_name := NULL;
    END IF;

    INSERT INTO public.address_archives
    (
        registry_number,
        city,
        street,
        house_number,
        apartment_number,
        postal_code,
        country,
        document_name,
        deleted_at
    )
    VALUES
    (
        OLD.registry_number,
        OLD.city,
        OLD.street,
        OLD.house_number,
        OLD.apartment_number,
        OLD.postal_code,
        OLD.country,
        v_document_name,
        NOW()
    );

    RETURN OLD;
END;
$$;

CREATE OR REPLACE FUNCTION public.archive_deleted_person()
RETURNS trigger
LANGUAGE plpgsql
AS $$
DECLARE
    v_document_name varchar(255);
BEGIN
    IF OLD.document_id IS NOT NULL THEN
        SELECT d.name
        INTO v_document_name
        FROM public.documents d
        WHERE d.document_id = OLD.document_id;
    ELSE
        v_document_name := NULL;
    END IF;

    INSERT INTO public.person_archives
    (
        person_pesel,
        first_name,
        middle_name,
        last_name,
        gender,
        birth_date,
        birth_place,
        document_name,
        deleted_at
    )
    VALUES
    (
        OLD.pesel,
        OLD.first_name,
        OLD.middle_name,
        OLD.last_name,
        OLD.wgender,
        OLD.birth_date,
        OLD.birth_place,
        v_document_name,
        NOW()
    );

    RETURN OLD;
END;
$$;

CREATE OR REPLACE FUNCTION public.archive_deleted_document()
RETURNS trigger
LANGUAGE plpgsql
AS $$
BEGIN
    INSERT INTO public.document_archives
    (
        name,
        category,
        import_date,
        deleted_at
    )
    VALUES
    (
        OLD.name,
        OLD.category,
        OLD.import_date,
        NOW()
    );

    RETURN OLD;
END;
$$;

CREATE OR REPLACE FUNCTION public.archive_deleted_birth_record()
RETURNS trigger
LANGUAGE plpgsql
AS $$
DECLARE
    v_born_person_pesel varchar(11);
    v_mother_pesel varchar(11);
    v_father_pesel varchar(11);
    v_birth_date date;
    v_birth_place varchar(200);
    v_document_name varchar(255);
BEGIN
    SELECT
        p.pesel,
        p.birth_date,
        p.birth_place
    INTO
        v_born_person_pesel,
        v_birth_date,
        v_birth_place
    FROM public.persons p
    WHERE p.person_id = OLD.person_id;

    IF OLD.mother_id IS NOT NULL THEN
        SELECT p.pesel
        INTO v_mother_pesel
        FROM public.persons p
        WHERE p.person_id = OLD.mother_id;
    ELSE
        v_mother_pesel := NULL;
    END IF;

    IF OLD.father_id IS NOT NULL THEN
        SELECT p.pesel
        INTO v_father_pesel
        FROM public.persons p
        WHERE p.person_id = OLD.father_id;
    ELSE
        v_father_pesel := NULL;
    END IF;

    IF OLD.document_id IS NOT NULL THEN
        SELECT d.name
        INTO v_document_name
        FROM public.documents d
        WHERE d.document_id = OLD.document_id;
    ELSE
        v_document_name := NULL;
    END IF;

    INSERT INTO public.birth_record_archives
    (
        registry_number,
        registry_date,
        born_person_pesel,
        mother_pesel,
        father_pesel,
        birth_date,
        birth_place,
        document_name,
        deleted_at
    )
    VALUES
    (
        OLD.registry_number,
        OLD.registry_date,
        v_born_person_pesel,
        v_mother_pesel,
        v_father_pesel,
        v_birth_date,
        v_birth_place,
        v_document_name,
        NOW()
    );

    RETURN OLD;
END;
$$;

CREATE OR REPLACE FUNCTION public.archive_deleted_death_record()
RETURNS trigger
LANGUAGE plpgsql
AS $$
DECLARE
    v_person_pesel varchar(11);
    v_document_name varchar(255);
BEGIN
    SELECT p.pesel
    INTO v_person_pesel
    FROM public.persons p
    WHERE p.person_id = OLD.person_id;

    IF OLD.document_id IS NOT NULL THEN
        SELECT d.name
        INTO v_document_name
        FROM public.documents d
        WHERE d.document_id = OLD.document_id;
    ELSE
        v_document_name := NULL;
    END IF;

    INSERT INTO public.death_record_archives
    (
        registry_number,
        registry_date,
        person_pesel,
        death_date,
        death_place,
        cause_of_death,
        document_name,
        deleted_at
    )
    VALUES
    (
        OLD.registry_number,
        OLD.registry_date,
        v_person_pesel,
        OLD.death_date,
        OLD.death_place,
        OLD.cause_of_death,
        v_document_name,
        NOW()
    );

    RETURN OLD;
END;
$$;

CREATE OR REPLACE FUNCTION public.archive_deleted_marriage_record()
RETURNS trigger
LANGUAGE plpgsql
AS $$
DECLARE
    v_spouse1_pesel varchar(11);
    v_spouse2_pesel varchar(11);
    v_document_name varchar(255);
BEGIN
    SELECT p.pesel
    INTO v_spouse1_pesel
    FROM public.persons p
    WHERE p.person_id = OLD.spouse1_id;

    SELECT p.pesel
    INTO v_spouse2_pesel
    FROM public.persons p
    WHERE p.person_id = OLD.spouse2_id;

    IF OLD.document_id IS NOT NULL THEN
        SELECT d.name
        INTO v_document_name
        FROM public.documents d
        WHERE d.document_id = OLD.document_id;
    ELSE
        v_document_name := NULL;
    END IF;

    INSERT INTO public.marriage_record_archives
    (
        registry_number,
        registry_date,
        spouse1_pesel,
        spouse2_pesel,
        marriage_date,
        marriage_place,
        document_name,
        deleted_at
    )
    VALUES
    (
        OLD.registry_number,
        OLD.registry_date,
        v_spouse1_pesel,
        v_spouse2_pesel,
        OLD.marriage_date,
        OLD.marriage_place,
        v_document_name,
        NOW()
    );

    RETURN OLD;
END;
$$;