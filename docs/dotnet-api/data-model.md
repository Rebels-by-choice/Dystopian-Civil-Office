# Data Model

## Live Entities

The live entity classes are in `Models/Entities`.

### Document

Table: `documents`

| Property | Column | Type/Notes |
| --- | --- | --- |
| `DocumentId` | `document_id` | Primary key, identity. |
| `Name` | `name` | Required, max length 100. |
| `Category` | `category` | Required, max length 100. |
| `ImportDate` | `import_date` | Required. |

Navigation properties:

- Optional one-to-one with `Person`.
- Optional one-to-one with `Address`.
- Optional one-to-one with `BirthRecord`.
- Optional one-to-one with `DeathRecord`.
- Optional one-to-one with `MarriageRecord`.

### Address

Table: `addresses`

| Property | Column | Notes |
| --- | --- | --- |
| `AddressId` | `address_id` | Primary key, identity. |
| `City` | `city` | Required, max length 100. |
| `Street` | `street` | Required, max length 100. |
| `HouseNumber` | `house_number` | Required, max length 20. |
| `ApartmentNumber` | `apartment_number` | Optional, max length 20. |
| `PostalCode` | `postal_code` | Required, max length 15. |
| `Country` | `country` | Required, max length 60. |
| `DocumentId` | `document_id` | Optional, unique index. |

Relationships:

- One address can have many persons.
- Optional one-to-one with document.
- Deleting the linked document sets `DocumentId` to null.

### Person

Table: `persons`

| Property | Column | Notes |
| --- | --- | --- |
| `PersonId` | `person_id` | Primary key, identity. |
| `Pesel` | `pesel` | Required, max length 11, unique. |
| `FirstName` | `first_name` | Required, max length 100. |
| `MiddleName` | `middle_name` | Optional, max length 100. |
| `LastName` | `last_name` | Required, max length 100. |
| `Gender` | `gender` | Required, max length 20. |
| `BirthDate` | `birth_date` | Required, PostgreSQL `date`. |
| `BirthPlace` | `birth_place` | Required, max length 100. |
| `AddressId` | `address_id` | Required foreign key. |
| `DocumentId` | `document_id` | Optional, unique index. |

Relationships:

- Required many-to-one to `Address`.
- Optional one-to-one to `Document`.
- Optional one-to-one from `BirthRecord`.
- Optional one-to-one from `DeathRecord`.
- One-to-many as mother/father for birth records.
- One-to-many as spouse 1/spouse 2 for marriage records.

Constraints:

- `pesel` is unique.
- `gender` must be one of `Male`, `Female`, or `Other`.

### BirthRecord

Table: `birth_records`

| Property | Column | Notes |
| --- | --- | --- |
| `BirthRecordId` | `birth_record_id` | Primary key, identity. |
| `RegistryNumber` | `registry_number` | Required, max length 50, unique. |
| `PersonId` | `person_id` | Required, unique. |
| `MotherId` | `mother_id` | Optional person foreign key. |
| `FatherId` | `father_id` | Optional person foreign key. |
| `RegistryDate` | `registry_date` | Required, PostgreSQL `date`. |
| `DocumentId` | `document_id` | Optional, unique index. |

Relationships:

- One-to-one with born person.
- Optional many-to-one to mother.
- Optional many-to-one to father.
- Optional one-to-one to document.

Delete behavior is `Restrict` for person relationships and `SetNull` for document relationship.

### DeathRecord

Table: `death_records`

| Property | Column | Notes |
| --- | --- | --- |
| `DeathRecordId` | `death_record_id` | Primary key, identity. |
| `RegistryNumber` | `registry_number` | Required, max length 50, unique. |
| `PersonId` | `person_id` | Required, unique. |
| `DeathDate` | `death_date` | Required, PostgreSQL `date`. |
| `DeathPlace` | `death_place` | Required, max length 100. |
| `RegistryDate` | `registry_date` | Required, PostgreSQL `date`. |
| `CauseOfDeath` | `cause_of_death` | Required, max length 200. |
| `DocumentId` | `document_id` | Optional, unique index. |

Relationships:

- One-to-one with person.
- Optional one-to-one with document.

### MarriageRecord

Table: `marriage_records`

| Property | Column | Notes |
| --- | --- | --- |
| `MarriageRecordId` | `marriage_record_id` | Primary key, identity. |
| `RegistryNumber` | `registry_number` | Required, max length 50, unique. |
| `Spouse1Id` | `spouse1_id` | Required person foreign key. |
| `Spouse2Id` | `spouse2_id` | Required person foreign key. |
| `MarriageDate` | `marriage_date` | Required, PostgreSQL `date`. |
| `MarriagePlace` | `marriage_place` | Required, max length 100. |
| `RegistryDate` | `registry_date` | Required, PostgreSQL `date`. |
| `DocumentId` | `document_id` | Optional, unique index. |

Relationships:

- Many-to-one to spouse 1 person.
- Many-to-one to spouse 2 person.
- Optional one-to-one to document.

Constraint:

- `spouse1_id` and `spouse2_id` must be different.

## Archive Entities

Archive models live in `Models/Archives`. They are mapped to archive tables and store denormalized historical data after deletes.

| Archive class | Table | Main purpose |
| --- | --- | --- |
| `AddressArchive` | `address_archives` | Stores deleted address data and optional document name. |
| `PersonArchive` | `person_archives` | Stores deleted person data and optional document name. |
| `DocumentArchive` | `document_archives` | Stores deleted document data. |
| `BirthRecordArchive` | `birth_record_archives` | Stores deleted birth record data plus person and parent PESEL values. |
| `DeathRecordArchive` | `death_record_archives` | Stores deleted death record data plus person PESEL. |
| `MarriageRecordArchive` | `marriage_record_archives` | Stores deleted marriage data plus spouse PESEL values. |

All archive models include a `DeletedAt` timestamp.

## Relationship Summary

```text
Address 1 -> many Persons
Document 0..1 -> 0..1 Address
Document 0..1 -> 0..1 Person
Document 0..1 -> 0..1 BirthRecord
Document 0..1 -> 0..1 DeathRecord
Document 0..1 -> 0..1 MarriageRecord
Person 1 -> 0..1 BirthRecord as born person
Person 1 -> 0..1 DeathRecord
Person 1 -> many BirthRecords as mother
Person 1 -> many BirthRecords as father
Person 1 -> many MarriageRecords as spouse1
Person 1 -> many MarriageRecords as spouse2
```

## DTO Mapping

The response DTOs intentionally flatten relationships:

- Person responses expose `DocumentName`, not `DocumentId`.
- Birth records expose PESEL values for born person, mother, and father.
- Death records expose the deceased person's PESEL.
- Marriage records expose spouse PESEL values.
- Person-address responses expose a person's PESEL and the address fields together.

This design makes the API easier for clients to consume because they do not need to walk nested entity graphs.
