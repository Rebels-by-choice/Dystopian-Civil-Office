DO $$
DECLARE
    r RECORD;
BEGIN
    FOR r IN 
        SELECT oid::regprocedure AS proc_identity
        FROM pg_proc
        WHERE prokind = 'p' 
          AND pronamespace = 'public'::regnamespace
    LOOP
        EXECUTE 'DROP PROCEDURE ' || r.proc_identity || ' CASCADE';
    END LOOP;
END $$;

DO $$
DECLARE
    r RECORD;
BEGIN
    FOR r IN 
        SELECT oid::regprocedure AS func_identity
        FROM pg_proc
        WHERE prokind IN ('f', 'a', 'w') 
          AND pronamespace = 'public'::regnamespace
    LOOP
        EXECUTE 'DROP FUNCTION ' || r.func_identity || ' CASCADE';
    END LOOP;
END $$;