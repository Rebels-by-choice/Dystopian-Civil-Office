DROP PROCEDURE IF EXISTS public.delete_death_record(integer);

CREATE OR REPLACE PROCEDURE public.delete_death_record(
    IN p_death_record_id integer
)
LANGUAGE plpgsql
AS $$
BEGIN
    DELETE FROM public.death_records
    WHERE death_record_id = p_death_record_id;

    IF NOT FOUND THEN
        RAISE EXCEPTION 'The death record to delete was not found.';
    END IF;
END;
$$;