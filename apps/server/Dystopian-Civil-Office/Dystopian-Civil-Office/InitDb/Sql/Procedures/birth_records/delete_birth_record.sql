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