DROP TRIGGER IF EXISTS trg_addresses_after_delete_archive ON public.addresses;
CREATE TRIGGER trg_addresses_after_delete_archive
AFTER DELETE ON public.addresses
FOR EACH ROW
EXECUTE FUNCTION public.archive_deleted_address();

DROP TRIGGER IF EXISTS trg_persons_after_delete_archive ON public.persons;
CREATE TRIGGER trg_persons_after_delete_archive
AFTER DELETE ON public.persons
FOR EACH ROW
EXECUTE FUNCTION public.archive_deleted_person();

DROP TRIGGER IF EXISTS trg_documents_after_delete_archive ON public.documents;
CREATE TRIGGER trg_documents_after_delete_archive
AFTER DELETE ON public.documents
FOR EACH ROW
EXECUTE FUNCTION public.archive_deleted_document();

DROP TRIGGER IF EXISTS trg_birth_records_after_delete_archive ON public.birth_records;
CREATE TRIGGER trg_birth_records_after_delete_archive
AFTER DELETE ON public.birth_records
FOR EACH ROW
EXECUTE FUNCTION public.archive_deleted_birth_record();

DROP TRIGGER IF EXISTS trg_death_records_after_delete_archive ON public.death_records;
CREATE TRIGGER trg_death_records_after_delete_archive
AFTER DELETE ON public.death_records
FOR EACH ROW
EXECUTE FUNCTION public.archive_deleted_death_record();

DROP TRIGGER IF EXISTS trg_marriage_records_after_delete_archive ON public.marriage_records;
CREATE TRIGGER trg_marriage_records_after_delete_archive
AFTER DELETE ON public.marriage_records
FOR EACH ROW
EXECUTE FUNCTION public.archive_deleted_marriage_record();
