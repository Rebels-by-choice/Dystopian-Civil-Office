# Architecture

## Architectural Style

The backend is a layered ASP.NET Core API:

```text
HTTP request
  -> Controller
  -> Service interface
  -> Service implementation
  -> EF Core DbContext or PostgreSQL stored procedure
  -> Database
  -> DTO response / HTTP status
```

The code does not currently use a separate repository layer. `ApplicationDbContext` is injected directly into services that need database access.

## Layers

### API Layer

Controllers live in `Controllers`. Every controller uses:

```csharp
[ApiController]
[Route("api/[controller]")]
```

This produces routes such as:

- `/api/Person`
- `/api/Document`
- `/api/BirthRecord`
- `/api/DeathRecord`
- `/api/Marriage`
- `/api/PersonAddress`
- `/api/ApiStats`

Controllers are intentionally thin. They accept DTOs, call services, and return `Ok`, `Created`, or `NoContent` style responses.

### DTO Layer

DTOs live in `Dtos`:

- `Dtos/Requests/Create`: required data for creating records.
- `Dtos/Requests/Update`: nullable patch-style fields for updates.
- `Dtos/Responses`: flattened response shapes for clients.

The API does not expose EF entities directly. This keeps database navigation properties and internal relationships out of the HTTP contract.

### Service Layer

Services are split by read/write intent:

- `Services/Read/Interfaces`
- `Services/Read/Impls`
- `Services/Write/Interfaces`
- `Services/Write/Impls`

Read services are thin wrappers around `OfficeQueryService`. Write services execute PostgreSQL stored procedures.

### Data Layer

`ApplicationDbContext` in `DataSource` owns EF Core mapping for:

- Live tables: `addresses`, `persons`, `birth_records`, `death_records`, `marriage_records`, `documents`.
- Archive tables: `address_archives`, `person_archives`, `birth_record_archives`, `death_record_archives`, `marriage_record_archives`, `document_archives`.

The context defines table names, column names, keys, indexes, relationships, delete behaviors, and check constraints.

### SQL Layer

`InitDb/Sql` contains SQL scripts for:

- Dropping tables/subobjects.
- Validation functions.
- Write procedures.
- Archive functions.
- Archive triggers.
- Mock seed data.

Write services depend on procedures such as:

- `create_person`, `update_person`, `delete_person`
- `create_address`, `update_address`, `delete_address`
- `create_document`, `update_document`, `delete_document`
- `create_birth_record`, `update_birth_record`, `delete_birth_record`
- `create_death_record`, `update_death_record`, `delete_death_record`
- `create_marriage_record`, `update_marriage_record`, `delete_marriage_record`

## Dependency Injection

`Program.cs` registers services as follows:

| Registration | Lifetime | Meaning |
| --- | --- | --- |
| `ApplicationDbContext` | Scoped | One EF Core context per request scope. |
| `OfficeQueryService` | Scoped | Shared read query projection service. |
| Read services | Scoped | Per-request read behavior. |
| Write services | Scoped | Per-request write behavior. |
| `IApiStatsService` | Singleton | In-memory log storage shared across requests. |

`IApiStatsService` uses `ConcurrentBag` and `Interlocked.Increment`, so it is designed for concurrent request logging.

## Request Pipeline

The middleware order in `Program.cs` is:

```text
Swagger in Development
HTTPS redirection
DatabaseExceptionHandlingMiddleware
DmlTrackerMiddleware
Authorization
Controllers
```

Important consequences:

- PostgreSQL exceptions thrown later in the pipeline are converted to JSON `ProblemDetails`.
- DML tracking runs around controller execution and logs non-GET requests after the downstream handler completes.
- Authorization middleware is present, but no authentication or authorization policies are configured in this project.

## Design Implications

The system uses a database-centric write model. That is useful when the database must enforce rules consistently, but it makes the SQL scripts part of the application logic. Any backend change to write behavior must check both:

- C# write service parameter lists.
- PostgreSQL procedure signatures and validation function behavior.

Read behavior is C#-centric. Response shape changes usually happen in:

- Response DTO classes.
- `OfficeQueryService` projections.
- Controller return types, if needed.
