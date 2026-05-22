/*
    Section for documents
*/
DROP PROCEDURE IF EXISTS public.create_document(
    varchar, varchar, timestamptz
);

CREATE OR REPLACE PROCEDURE public.create_document(
    IN p_name varchar(100),
    IN p_category varchar(100),
    IN p_import_date timestamptz
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_name varchar(100);
    v_category varchar(100);
    v_import_date timestamptz;
BEGIN
    SELECT v.name, v.category, v.import_date
    INTO v_name, v_category, v_import_date
    FROM public.validate_document_data(
        p_name,
        p_category,
        p_import_date
    ) v;

    INSERT INTO public.documents (
        name,
        category,
        import_date
    )
    VALUES (
        v_name,
        v_category,
        v_import_date
    );

EXCEPTION
    WHEN unique_violation THEN
        RAISE EXCEPTION 'A document with this data already exists.';
END;
$$;

DROP PROCEDURE IF EXISTS public.delete_document(integer);

CREATE OR REPLACE PROCEDURE public.delete_document(
    IN p_document_id integer
)
LANGUAGE plpgsql
AS $$
BEGIN
    DELETE FROM public.documents
    WHERE document_id = p_document_id;

    IF NOT FOUND THEN
        RAISE EXCEPTION 'The document to delete was not found.';
    END IF;

EXCEPTION
    WHEN foreign_key_violation THEN
        RAISE EXCEPTION 'The document cannot be deleted because it is referenced by other data.';
END;
$$;

DROP PROCEDURE IF EXISTS public.update_document(
    integer, varchar, varchar, timestamptz
);

CREATE OR REPLACE PROCEDURE public.update_document(
    IN p_document_id integer,
    IN p_name varchar(100) DEFAULT NULL,
    IN p_category varchar(100) DEFAULT NULL,
    IN p_import_date timestamptz DEFAULT NULL
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_current_document public.documents%ROWTYPE;

    v_final_name varchar(100);
    v_final_category varchar(100);
    v_final_import_date timestamptz;

    v_validated_name varchar(100);
    v_validated_category varchar(100);
    v_validated_import_date timestamptz;
BEGIN
    SELECT *
    INTO v_current_document
    FROM public.documents d
    WHERE d.document_id = p_document_id;

    IF NOT FOUND THEN
        RAISE EXCEPTION 'The document to update was not found.';
    END IF;

    v_final_name := COALESCE(p_name, v_current_document.name);
    v_final_category := COALESCE(p_category, v_current_document.category);
    v_final_import_date := COALESCE(p_import_date, v_current_document.import_date);

    SELECT v.name, v.category, v.import_date
    INTO v_validated_name, v_validated_category, v_validated_import_date
    FROM public.validate_document_data(
        v_final_name,
        v_final_category,
        v_final_import_date
    ) v;

    UPDATE public.documents
    SET
        name = v_validated_name,
        category = v_validated_category,
        import_date = v_validated_import_date
    WHERE document_id = p_document_id;

    IF NOT FOUND THEN
        RAISE EXCEPTION 'The document to update was not found.';
    END IF;

EXCEPTION
    WHEN unique_violation THEN
        RAISE EXCEPTION 'A document with this data already exists.';
END;
$$;