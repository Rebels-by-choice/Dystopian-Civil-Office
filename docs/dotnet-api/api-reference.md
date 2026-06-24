# API Reference

## General API Conventions

All controllers use the base pattern:

```text
/api/[controller]
```

The controller class name without `Controller` becomes the route segment. For example, `PersonController` maps to `/api/Person`.

The API uses JSON request/response bodies. Most write endpoints return only a status code:

- `POST`: `201 Created`
- `PUT`: `204 No Content`
- `DELETE`: `204 No Content`
- `GET`: `200 OK` with a response body

Database errors are converted by middleware to `application/problem+json`.

## ApiStats

Base route:

```text
/api/ApiStats
```

| Method | Route           | Description                                    | Response                          |
| ------ | --------------- | ---------------------------------------------- | --------------------------------- |
| `GET`  | `/api/ApiStats` | Returns in-memory logs for non-GET operations. | `200 OK` with `ApiOperationLog[]` |

`ApiOperationLog` contains:

| Field        | Type       | Meaning                     |
| ------------ | ---------- | --------------------------- |
| `Id`         | `int`      | Incrementing in-memory ID.  |
| `Method`     | `string`   | HTTP method.                |
| `Path`       | `string`   | Request path.               |
| `StatusCode` | `int`      | Final response status code. |
| `ExecutedAt` | `DateTime` | UTC timestamp.              |

## Person

Base route:

```text
/api/Person
```

| Method   | Route                        | Request DTO              | Response                            |
| -------- | ---------------------------- | ------------------------ | ----------------------------------- |
| `GET`    | `/api/Person`                | none                     | `200 OK` with `PersonResponseDto[]` |
| `POST`   | `/api/Person`                | `CreatePersonRequestDto` | `201 Created`                       |
| `PUT`    | `/api/Person/{personId:int}` | `UpdatePersonRequestDto` | `204 No Content`                    |
| `DELETE` | `/api/Person/{personId:int}` | none                     | `204 No Content`                    |

Create fields:

| Field                   | Type       | Required by DTO shape                       |
| ----------------------- | ---------- | ------------------------------------------- |
| `Pesel`                 | `string`   | yes                                         |
| `FirstName`             | `string`   | yes                                         |
| `MiddleName`            | `string?`  | no                                          |
| `LastName`              | `string`   | yes                                         |
| `Gender`                | `string`   | yes                                         |
| `BirthDate`             | `DateOnly` | yes                                         |
| `BirthPlace`            | `string`   | yes                                         |
| `AddressRegistryNumber` | `string?`  | no (registry number of an existing address) |
| `DocumentName`          | `string?`  | no                                          |

Response fields:

`PersonPesel`, `FirstName`, `MiddleName`, `LastName`, `Gender`, `BirthDate`, `BirthPlace`, `DocumentName`.

## Address

Base route:

```text
/api/Address
```

| Method   | Route                          | Request DTO               | Response                             |
| -------- | ------------------------------ | ------------------------- | ------------------------------------ |
| `GET`    | `/api/Address`                 | none                      | `200 OK` with `AddressResponseDto[]` |
| `POST`   | `/api/Address`                 | `CreateAddressRequestDto` | `201 Created`                        |
| `PUT`    | `/api/Address/{addressId:int}` | `UpdateAddressRequestDto` | `204 No Content`                     |
| `DELETE` | `/api/Address/{addressId:int}` | none                      | `204 No Content`                     |

Create fields:

`RegistryNumber`, `City`, `Street`, `HouseNumber`, `ApartmentNumber`, `PostalCode`, `Country`, `DocumentName`.

Response fields:

`AddressId`, `RegistryNumber`, `City`, `Street`, `HouseNumber`, `ApartmentNumber`, `PostalCode`, `Country`, `DocumentName`.

## Document

Base route:

```text
/api/Document
```

| Method   | Route                            | Request DTO                | Response                              |
| -------- | -------------------------------- | -------------------------- | ------------------------------------- |
| `GET`    | `/api/Document`                  | none                       | `200 OK` with `DocumentResponseDto[]` |
| `POST`   | `/api/Document`                  | `CreateDocumentRequestDto` | `201 Created`                         |
| `PUT`    | `/api/Document/{documentId:int}` | `UpdateDocumentRequestDto` | `204 No Content`                      |
| `DELETE` | `/api/Document/{documentId:int}` | none                       | `204 No Content`                      |

Create fields:

`Name`, `Category`, `ImportDate`.

Response fields:

`Name`, `Category`, `ImportDate`.

## BirthRecord

Base route:

```text
/api/BirthRecord
```

| Method   | Route                                  | Request DTO                   | Response                                 |
| -------- | -------------------------------------- | ----------------------------- | ---------------------------------------- |
| `GET`    | `/api/BirthRecord`                     | none                          | `200 OK` with `BirthRecordResponseDto[]` |
| `POST`   | `/api/BirthRecord`                     | `CreateBirthRecordRequestDto` | `201 Created`                            |
| `PUT`    | `/api/BirthRecord/{birthRecordId:int}` | `UpdateBirthRecordRequestDto` | `204 No Content`                         |
| `DELETE` | `/api/BirthRecord/{birthRecordId:int}` | none                          | `204 No Content`                         |

Create fields:

`RegistryNumber`, `PersonId`, `MotherId`, `FatherId`, `RegistryDate`, `DocumentId`.

Response fields:

`RegistryNumber`, `RegistryDate`, `BornPersonPesel`, `MotherPesel`, `FatherPesel`, `BirthDate`, `BirthPlace`, `DocumentName`.

## DeathRecord

Base route:

```text
/api/DeathRecord
```

| Method   | Route                                  | Request DTO                   | Response                                 |
| -------- | -------------------------------------- | ----------------------------- | ---------------------------------------- |
| `GET`    | `/api/DeathRecord`                     | none                          | `200 OK` with `DeathRecordResponseDto[]` |
| `POST`   | `/api/DeathRecord`                     | `CreateDeathRecordRequestDto` | `201 Created`                            |
| `PUT`    | `/api/DeathRecord/{deathRecordId:int}` | `UpdateDeathRecordRequestDto` | `204 No Content`                         |
| `DELETE` | `/api/DeathRecord/{deathRecordId:int}` | none                          | `204 No Content`                         |

Create fields:

`RegistryNumber`, `PersonId`, `DeathDate`, `DeathPlace`, `RegistryDate`, `CauseOfDeath`, `DocumentId`.

Response fields:

`RegistryNumber`, `RegistryDate`, `PersonPesel`, `DeathDate`, `DeathPlace`, `CauseOfDeath`, `DocumentName`.

## Marriage

Base route:

```text
/api/Marriage
```

| Method   | Route                            | Request DTO                | Response                              |
| -------- | -------------------------------- | -------------------------- | ------------------------------------- |
| `GET`    | `/api/Marriage`                  | none                       | `200 OK` with `MarriageResponseDto[]` |
| `POST`   | `/api/Marriage`                  | `CreateMarriageRequestDto` | `201 Created`                         |
| `PUT`    | `/api/Marriage/{registryNumber}` | `UpdateMarriageRequestDto` | `204 No Content`                      |
| `DELETE` | `/api/Marriage/{registryNumber}` | none                       | `204 No Content`                      |

Create fields:

`RegistryNumber`, `RegistryDate`, `Spouse1Pesel`, `Spouse2Pesel`, `MarriageDate`, `MarriagePlace`, `DocumentName`.

Response fields:

`RegistryNumber`, `RegistryDate`, `Spouse1Pesel`, `Spouse2Pesel`, `MarriageDate`, `MarriagePlace`, `DocumentName`.

Unlike other update/delete endpoints, marriage records are updated and deleted by `registryNumber`, not by integer primary key.

## Error Responses

`DatabaseExceptionHandlingMiddleware` catches `PostgresException` and returns `ProblemDetails`:

| Condition                        | HTTP Status       | Title                      |
| -------------------------------- | ----------------- | -------------------------- |
| Message contains `not found`     | `404 Not Found`   | `Resource not found`       |
| PostgreSQL unique violation      | `409 Conflict`    | `Database conflict`        |
| PostgreSQL foreign key violation | `409 Conflict`    | `Database conflict`        |
| Other PostgreSQL exception       | `400 Bad Request` | `Invalid database request` |

The `Detail` field is the PostgreSQL `MessageText`.
