DROP PROCEDURE IF EXISTS public.delete_person(integer);

CREATE OR REPLACE PROCEDURE public.delete_person(
    IN p_person_id integer
)
LANGUAGE plpgsql
AS $$
BEGIN
    DELETE FROM public.persons
    WHERE person_id = p_person_id;

    IF NOT FOUND THEN
        RAISE EXCEPTION 'The person to delete was not found.';
    END IF;

EXCEPTION
    WHEN foreign_key_violation THEN
        RAISE EXCEPTION 'The person cannot be deleted because it is referenced by other data.';
END;
$$;