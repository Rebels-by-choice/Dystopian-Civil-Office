DROP PROCEDURE IF EXISTS public.create_case(
    integer, timestamptz, integer
    );

CREATE OR REPLACE PROCEDURE public.create_case(
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
            0,
           clock_timestamp(),
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
v_existing_initiator_id integer;
v_existing_responder_id integer;
    v_existing_status integer;
	v_success boolean;
	v_is_responder_functionary boolean;
BEGIN
SELECT v.initiator_id, v.responder_id, v.status, v.is_functionary
INTO v_existing_initiator_id, v_existing_responder_id, v_existing_status, v_is_responder_functionary
FROM public.cases v
WHERE v.case_id = p_case_id;

IF p_responder_id = v_existing_initiator_id THEN
        RAISE EXCEPTION 'You can''t respond to your own case.';
END IF;	

-- 0 Open
-- 1 PendingDocuments
-- 2 Closed
-- 3 Cancelled

IF v_existing_status >= 2 THEN
        RAISE EXCEPTION 'You''re not authorized. Case is closed.';
END IF;

IF (p_new_status = 1 OR p_new_status = 2) AND NOT v_is_responder_functionary OR
   (p_new_status = 0 OR p_new_status = 3) AND v_is_responder_functionary THEN
        RAISE EXCEPTION 'You''re not authorized to perform this kind of action.';
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

	IF v_success AND p_new_status >= 2 THEN
UPDATE public.cases
SET closed_at = clock_timestamp()
WHERE case_id = p_case_id;
END IF;

EXCEPTION
    WHEN unique_violation THEN
        RAISE EXCEPTION 'A case with this data already exists.';
END;
$$;