/* Main */
SELECT * FROM documents ORDER BY document_id DESC;

SELECT * FROM addresses ORDER BY address_id DESC;

SELECT * FROM persons ORDER BY person_id DESC;

SELECT * FROM birth_records ORDER BY birth_record_id DESC;

SELECT * FROM marriage_records ORDER BY marriage_record_id DESC;

SELECT * FROM death_records ORDER BY death_record_id DESC;

/* Archives */
SELECT * FROM document_archives ORDER BY document_archive_id DESC;

SELECT * FROM address_archives ORDER BY address_archive_id DESC;

SELECT * FROM person_archives ORDER BY person_archive_id DESC;

SELECT * FROM birth_record_archives ORDER BY birth_record_archive_id DESC;

SELECT * FROM marriage_record_archives ORDER BY marriage_record_archive_id DESC;

SELECT * FROM death_record_archives ORDER BY death_record_archive_id DESC;