# Services And Business Logic

## Service Registration

`Program.cs` registers services through ASP.NET Core dependency injection.

Read services:

- `IMarriageReadService` -> `MarriageReadService`
- `IPersonReadService` -> `PersonReadService`
- `IDocumentReadService` -> `DocumentReadService`
- `IDeathRecordReadService` -> `DeathRecordReadService`
- `IBirthRecordService` -> `BirthRecordReadService`
- `IPersonAddressReadService` -> `PersonAddressReadService`
- `IApiStatsService` -> `ApiStatsService`

Write services:

- `IBirthRecordWriteService` -> `BirthRecordWriteService`
- `IDeathRecordWriteService` -> `DeathRecordWriteService`
- `IDocumentWriteService` -> `DocumentWriteService`
- `IMarriageWriteService` -> `MarriageWriteService`
- `IPersonAddressWriteService` -> `PersonAddressWriteService`
- `IPersonWriteService` -> `PersonWriteService`

## Read Path

Read controllers call read service interfaces. Those read services delegate to `OfficeQueryService`.

Example path:

```text
GET /api/Person
  -> PersonController.GetResponseAsync
  -> IPersonReadService.GetPersonsAsync
  -> PersonReadService.GetPersonsAsync
  -> OfficeQueryService.GetPersonsAsync
  -> EF Core query over Persons
  -> PersonResponseDto[]
```

`OfficeQueryService` uses:

```csharp
AsNoTracking()
```

This is appropriate for read-only API queries because EF Core does not need to track returned entities for updates.

## OfficeQueryService

`OfficeQueryService` is the main read projection service.

| Method | Query source | Response DTO |
| --- | --- | --- |
| `GetPersonsAsync` | `Persons` | `PersonResponseDto` |
| `GetDocumentsAsync` | `Documents` | `DocumentResponseDto` |
| `GetDeathRecordsAsync` | `DeathRecords` | `DeathRecordResponseDto` |
| `GetBirthRecordsAsync` | `BirthRecords` | `BirthRecordResponseDto` |
| `GetMarriageRecordsAsync` | `MarriageRecords` | `MarriageResponseDto` |
| `GetPersonAddressesAsync` | `Persons` with `Address` | `PersonAddressResponseDto` |

The service projects directly to DTOs using LINQ `Select`. This keeps response shape centralized and avoids returning EF navigation graphs to clients.

## Write Path

Write controllers call write services. Write services call PostgreSQL stored procedures.

Example path:

```text
POST /api/Person
  -> PersonController.CreateAsync
  -> IPersonWriteService.CreatePersonAsync
  -> PersonWriteService.CreatePersonAsync
  -> CALL public.create_person(...)
  -> PostgreSQL validation/procedure logic
  -> 201 Created or problem response
```

## Write Service Behavior

All write services follow the same pattern:

1. Build an array of `NpgsqlParameter`.
2. Convert nullable missing values to `DBNull.Value`.
3. Execute a `CALL public.<procedure_name>(...)` statement.
4. Let PostgreSQL raise exceptions for database validation failures.
5. Let middleware translate PostgreSQL exceptions to HTTP responses.

This creates a clear contract between C# and SQL. If a procedure signature changes, the matching write service must be updated.

## Domain-Specific Write Notes

### Person

`PersonWriteService` calls:

- `create_person`
- `update_person`
- `delete_person`

The API identifies persons by `personId` for update/delete. The read response exposes PESEL instead of `personId`.

### Address

`PersonAddressWriteService` calls:

- `create_address`
- `update_address`
- `delete_address`

The controller name is `PersonAddress`, but the underlying database table is `addresses`.

### Document

`DocumentWriteService` calls:

- `create_document`
- `update_document`
- `delete_document`

Documents can be optionally linked one-to-one to several entity types.

### Birth Record

`BirthRecordWriteService` calls:

- `create_birth_record`
- `update_birth_record`
- `delete_birth_record`

The API identifies birth records by integer `birthRecordId` for update/delete.

### Death Record

`DeathRecordWriteService` calls:

- `create_death_record`
- `update_death_record`
- `delete_death_record`

The API identifies death records by integer `deathRecordId` for update/delete.

### Marriage Record

`MarriageWriteService` calls:

- `create_marriage_record`
- `update_marriage_record`
- `delete_marriage_record`

The API identifies marriage records by `registryNumber` for update/delete. Create/update DTOs use spouse PESEL values, not spouse IDs.

## API Stats Service

`ApiStatsService` stores operation logs in memory:

- Storage: `ConcurrentBag<ApiOperationLog>`.
- ID generation: `Interlocked.Increment`.
- Timestamp: `DateTime.UtcNow`.
- Query order: newest ID first.

Because this is in-memory:

- Logs reset when the application restarts.
- Logs are not shared between multiple application instances.
- Logs are not persisted to PostgreSQL.
