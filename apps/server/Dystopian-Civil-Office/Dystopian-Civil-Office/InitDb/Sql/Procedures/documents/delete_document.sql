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