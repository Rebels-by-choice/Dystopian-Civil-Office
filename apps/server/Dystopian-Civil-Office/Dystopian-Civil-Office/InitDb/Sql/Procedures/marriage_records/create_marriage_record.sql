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
    v_spouse1_id integer;
    v_spouse2_id integer;
    v_document_id integer;
BEGIN
    SELECT v.spouse1_id, v.spouse2_id, v.document_id
    INTO v_spouse1_id, v_spouse2_id, v_document_id
    FROM public.validate_marriage_record_data(
        p_registry_number,
        p_spouse1_pesel,
        p_spouse2_pesel,
        p_document_name
    ) v;

    INSERT INTO public.marriage_records (
        registry_number,
        spouse1_id,
        spouse2_id,
        marriage_date,
        marriage_place,
        registry_date,
        document_id
    )
    VALUES (
        p_registry_number,
        v_spouse1_id,
        v_spouse2_id,
        p_marriage_date,
        p_marriage_place,
        p_registry_date,
        v_document_id
    );

EXCEPTION
    WHEN unique_violation THEN
        RAISE EXCEPTION 'A marriage record with this data already exists.';
    WHEN foreign_key_violation THEN
        RAISE EXCEPTION 'The marriage record contains invalid related data.';
END;
$$;