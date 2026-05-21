DROP PROCEDURE IF EXISTS public.delete_address(integer);

CREATE OR REPLACE PROCEDURE public.delete_address(
    IN p_address_id integer
)
LANGUAGE plpgsql
AS $$
BEGIN
    DELETE FROM public.addresses
    WHERE address_id = p_address_id;

    IF NOT FOUND THEN
        RAISE EXCEPTION 'The address to delete was not found.';
    END IF;
END;
$$;