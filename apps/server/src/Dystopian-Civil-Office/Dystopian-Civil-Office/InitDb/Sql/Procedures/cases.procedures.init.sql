DROP PROCEDURE IF EXISTS public.create_case(
    integer, timestamptz, integer
    );

CREATE OR REPLACE PROCEDURE public.create_case(
	IN p_status integer,
    IN p_created_at timestamptz,
	IN p_initiator_id integer
)
LANGUAGE plpgsql
AS $$
BEGIN
INSERT INTO public.cases (
    status,
    created_at,
    initiator_id
)
VALUES (
           p_status,
           p_created_at,
           p_initiator_id
       );

EXCEPTION
    WHEN unique_violation THEN
        RAISE EXCEPTION 'A case with this data already exists.';
END;
$$;

DROP PROCEDURE IF EXISTS public.update_case(
    integer, integer, integer
    );
CREATE OR REPLACE PROCEDURE public.update_case(
    IN p_case_id integer,
    IN p_new_status integer,
    IN p_responder_id integer
)
LANGUAGE plpgsql
AS $$
DECLARE
v_existing_responder_id integer;
    v_existing_status integer;
	v_success boolean;
BEGIN
SELECT v.responder_id, v.status
INTO v_existing_responder_id, v_existing_status
FROM public.cases v
WHERE v.case_id = p_case_id;

IF v_existing_status = 2 THEN
        RAISE EXCEPTION 'You''re not authorized. Case is closed.';
END IF;
	
    IF v_existing_responder_id IS NULL THEN
UPDATE public.cases
SET responder_id = p_responder_id,
    status = p_new_status
WHERE case_id = p_case_id;

v_success = true;

    ELSIF v_existing_responder_id = p_responder_id THEN
UPDATE public.cases
SET status = p_new_status
WHERE case_id = p_case_id;

v_success = true;
ELSE
        RAISE EXCEPTION 'You''re not authorized. Someone else took over that case.';
END IF;

	IF v_success AND p_new_status = 2 THEN
UPDATE public.cases
SET closed_at = clock_timestamp()
WHERE case_id = p_case_id;
END IF;

EXCEPTION
    WHEN unique_violation THEN
        RAISE EXCEPTION 'A case with this data already exists.';
END;
$$;