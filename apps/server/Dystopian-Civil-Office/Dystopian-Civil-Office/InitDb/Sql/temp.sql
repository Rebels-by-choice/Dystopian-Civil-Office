/*
    ============================================
    ADDRESS ARCHIVE
    ============================================
*/
DROP TRIGGER IF EXISTS trg_addresses_after_delete_archive ON public.addresses;
DROP FUNCTION IF EXISTS public.archive_deleted_address();

CREATE OR REPLACE FUNCTION public.archive_deleted_address()
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

    INSERT INTO public.address_archives
    (
        "City",
        "Street",
        "HouseNumber",
        "ApartmentNumber",
        "PostalCode",
        "Country",
        "DocumentName",
        "DeletedAt"
    )
    VALUES
    (
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

CREATE TRIGGER trg_addresses_after_delete_archive
AFTER DELETE ON public.addresses
FOR EACH ROW
EXECUTE FUNCTION public.archive_deleted_address();


/*
    ============================================
    PERSON ARCHIVE
    ============================================
*/
DROP TRIGGER IF EXISTS trg_persons_after_delete_archive ON public.persons;
DROP FUNCTION IF EXISTS public.archive_deleted_person();

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
        "PersonPesel",
        "FirstName",
        "MiddleName",
        "LastName",
        "Gender",
        "BirthDate",
        "BirthPlace",
        "DocumentName",
        "DeletedAt"
    )
    VALUES
    (
        OLD.pesel,
        OLD.first_name,
        OLD.middle_name,
        OLD.last_name,
        OLD.gender,
        OLD.birth_date,
        OLD.birth_place,
        v_document_name,
        NOW()
    );

    RETURN OLD;
END;
$$;

CREATE TRIGGER trg_persons_after_delete_archive
AFTER DELETE ON public.persons
FOR EACH ROW
EXECUTE FUNCTION public.archive_deleted_person();


/*
    ============================================
    DOCUMENT ARCHIVE
    ============================================
*/
DROP TRIGGER IF EXISTS trg_documents_after_delete_archive ON public.documents;
DROP FUNCTION IF EXISTS public.archive_deleted_document();

CREATE OR REPLACE FUNCTION public.archive_deleted_document()
RETURNS trigger
LANGUAGE plpgsql
AS $$
BEGIN
    INSERT INTO public.document_archives
    (
        "Name",
        "Category",
        "ImportDate",
        "DeletedAt"
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

CREATE TRIGGER trg_documents_after_delete_archive
AFTER DELETE ON public.documents
FOR EACH ROW
EXECUTE FUNCTION public.archive_deleted_document();


/*
    ============================================
    BIRTH RECORD ARCHIVE
    ============================================
*/
DROP TRIGGER IF EXISTS trg_birth_records_after_delete_archive ON public.birth_records;
DROP FUNCTION IF EXISTS public.archive_deleted_birth_record();

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
        "RegistryNumber",
        "RegistryDate",
        "BornPersonPesel",
        "MotherPesel",
        "FatherPesel",
        "BirthDate",
        "BirthPlace",
        "DocumentName",
        "DeletedAt"
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

CREATE TRIGGER trg_birth_records_after_delete_archive
AFTER DELETE ON public.birth_records
FOR EACH ROW
EXECUTE FUNCTION public.archive_deleted_birth_record();


/*
    ============================================
    DEATH RECORD ARCHIVE
    ============================================
*/
DROP TRIGGER IF EXISTS trg_death_records_after_delete_archive ON public.death_records;
DROP FUNCTION IF EXISTS public.archive_deleted_death_record();

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
        "RegistryNumber",
        "RegistryDate",
        "PersonPesel",
        "DeathDate",
        "DeathPlace",
        "CauseOfDeath",
        "DocumentName",
        "DeletedAt"
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

CREATE TRIGGER trg_death_records_after_delete_archive
AFTER DELETE ON public.death_records
FOR EACH ROW
EXECUTE FUNCTION public.archive_deleted_death_record();


/*
    ============================================
    MARRIAGE RECORD ARCHIVE
    ============================================
*/
DROP TRIGGER IF EXISTS trg_marriage_records_after_delete_archive ON public.marriage_records;
DROP FUNCTION IF EXISTS public.archive_deleted_marriage_record();

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
        "RegistryNumber",
        "RegistryDate",
        "Spouse1Pesel",
        "Spouse2Pesel",
        "MarriageDate",
        "MarriagePlace",
        "DocumentName",
        "DeletedAt"
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

CREATE TRIGGER trg_marriage_records_after_delete_archive
AFTER DELETE ON public.marriage_records
FOR EACH ROW
EXECUTE FUNCTION public.archive_deleted_marriage_record();