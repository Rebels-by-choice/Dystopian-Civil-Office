# Database And SQL

## Database Connection

The backend uses PostgreSQL through EF Core and Npgsql:

```csharp
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
```

The connection string key is:

```text
ConnectionStrings:DefaultConnection
```

`appsettings.json` currently contains an empty placeholder:

```json
{
  "ConnectionStrings": { "DefaultConnection": "" }
}
```

In real use, provide the connection string through user secrets, environment variables, local app settings, or deployment configuration.

## EF Core Context

`ApplicationDbContext` exposes DbSets for live and archive data:

Live DbSets:

- `Addresses`
- `Persons`
- `BirthRecords`
- `MarriageRecords`
- `DeathRecords`
- `Documents`

Archive DbSets:

- `DocumentArchives`
- `PersonArchives`
- `BirthRecordArchives`
- `DeathRecordArchives`
- `MarriageRecordArchives`
- `AddressArchives`

`OnModelCreating` configures the relational schema explicitly:

- Table names use snake case.
- Column names use snake case.
- Primary keys use identity columns.
- Foreign keys and delete behaviors are explicit.
- Unique indexes protect PESEL, registry numbers, and one-to-one document links.
- Check constraints validate gender and spouse identity.

## Migrations

The `Migrations` folder contains:

- `20260523153424_InitialPipeline.cs`
- `20260523153424_InitialPipeline.Designer.cs`
- `ApplicationDbContextModelSnapshot.cs`

These files represent the EF Core schema pipeline at the time the migration was created. Because the project also contains raw SQL procedures/functions/triggers, database setup must account for both EF migrations and SQL scripts.

## SQL Script Areas

`InitDb/Sql` is divided by responsibility:

| Folder | Purpose |
| --- | --- |
| `Clean` | Drop tables, procedures, and functions. |
| `Functions` | Validation functions and archive functions. |
| `Procedures` | Create, update, and delete procedures used by write services. |
| `Seed` | Mock seed data. |
| `Triggers` | Triggers that archive deleted rows. |

The SQL files are included as embedded resources in the project file.

## Stored Procedures

Write services call stored procedures directly. The C# service parameter names are aligned with SQL parameter names.

| Domain | Procedures |
| --- | --- |
| Address | `create_address`, `update_address`, `delete_address` |
| Person | `create_person`, `update_person`, `delete_person` |
| Document | `create_document`, `update_document`, `delete_document` |
| Birth record | `create_birth_record`, `update_birth_record`, `delete_birth_record` |
| Death record | `create_death_record`, `update_death_record`, `delete_death_record` |
| Marriage record | `create_marriage_record`, `update_marriage_record`, `delete_marriage_record` |

The write services execute these with:

```csharp
await _dbContext.Database.ExecuteSqlRawAsync("CALL public.some_procedure(...)", parameters, cancellationToken);
```

Nullable DTO values are converted to `DBNull.Value` before being sent to PostgreSQL.

## Validation Functions

`InitDb/Sql/Functions/validate_data.defs.sql` defines validation functions for:

- Address data.
- Birth record data.
- Death record data.
- Document data.
- Marriage record data.
- Person data.

These functions are part of the write-side business rules. They are expected to raise PostgreSQL exceptions when input is invalid. Those exceptions are caught by `DatabaseExceptionHandlingMiddleware` and converted to HTTP problem responses.

## Archive Functions And Triggers

`InitDb/Sql/Functions/archive_data.defs.sql` defines archive functions for deleted rows:

- `archive_deleted_address`
- `archive_deleted_person`
- `archive_deleted_document`
- `archive_deleted_birth_record`
- `archive_deleted_death_record`
- `archive_deleted_marriage_record`

`InitDb/Sql/Triggers/archive_data.triggers.sql` attaches those functions as `AFTER DELETE` triggers:

| Live table | Trigger |
| --- | --- |
| `addresses` | `trg_addresses_after_delete_archive` |
| `persons` | `trg_persons_after_delete_archive` |
| `documents` | `trg_documents_after_delete_archive` |
| `birth_records` | `trg_birth_records_after_delete_archive` |
| `death_records` | `trg_death_records_after_delete_archive` |
| `marriage_records` | `trg_marriage_records_after_delete_archive` |

The archive design stores denormalized readable values such as PESEL and document name. This helps preserve history even if related live rows change or are deleted later.

## Seed Data

`InitDb/Sql/Seed/seed_mocks.init.sql` inserts mock data into:

- `documents`
- `addresses`
- `persons`
- `birth_records`
- `marriage_records`
- `death_records`

This is useful for local development and API testing.

## Cleanup Scripts

Cleanup scripts exist for resetting database objects:

- `drop_cascade_tables.sql`
- `drop_subobjects.sql`

These should be used carefully because they drop database objects. They are development/admin utilities, not runtime API behavior.
