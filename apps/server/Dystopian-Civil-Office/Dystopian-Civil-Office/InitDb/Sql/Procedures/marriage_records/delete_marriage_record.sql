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