# Development And Operations

## Local Requirements

Expected local tooling:

- .NET SDK compatible with `net10.0`.
- PostgreSQL database.
- EF Core tooling if migrations are being created or applied.

## Running Locally

From the project directory:

```powershell
dotnet run
```

Configured launch URLs:

```text
http://localhost:5159
https://localhost:7083
```

## Swagger

Swagger is enabled only in Development:

```csharp
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
```

Swagger UI should be available at:

```text
http://localhost:5159/swagger
https://localhost:7083/swagger
```

Swagger is generated from the controller routes and DTO types through Swashbuckle.

## Configuration

The application reads the PostgreSQL connection string from:

```text
ConnectionStrings:DefaultConnection
```

For local development, prefer user secrets or a local-only settings file rather than committing credentials.

Example shape:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=dystopian_civil_office;Username=postgres;Password=your-password"
  }
}
```

## Database Setup

The project contains both EF Core migrations and raw SQL scripts. A complete database setup should include:

1. Create/configure the PostgreSQL database.
2. Apply EF Core migrations to create tables and constraints.
3. Apply SQL functions from `InitDb/Sql/Functions`.
4. Apply SQL procedures from `InitDb/Sql/Procedures`.
5. Apply archive triggers from `InitDb/Sql/Triggers`.
6. Optionally apply seed data from `InitDb/Sql/Seed`.

The exact automation for applying embedded SQL resources is not present in the inspected source. If database initialization is currently manual, document the manual order in the project README or add a startup/admin initializer.

## Migrations

Common EF Core commands:

```powershell
dotnet ef migrations add MigrationName
dotnet ef database update
```

Because write behavior depends on PostgreSQL procedures and functions, migrations alone do not fully describe the running database behavior.

## Docker

The project includes a `Dockerfile` and a Docker launch profile.

The Docker profile sets:

```text
ASPNETCORE_HTTP_PORTS=8080
ASPNETCORE_HTTPS_PORTS=8081
```

When running in Docker, the connection string must point to a PostgreSQL host reachable from the container. `localhost` inside the API container refers to the container itself, not the host machine.

## Build And Verification

Useful commands:

```powershell
dotnet restore
dotnet build
dotnet run
```

Manual API checks can use:

- Swagger UI.
- `Dystopian-Civil-Office.http`.
- curl/Postman/Insomnia.

## Operational Notes

- There is no authentication configured in the current backend source.
- `UseAuthorization` is present, but without authentication and policies it does not protect endpoints by itself.
- API stats are in-memory and reset on restart.
- Swagger is Development-only.
- PostgreSQL stored procedures are required for write endpoints to work.
- Archive triggers are required if deleted data must be preserved.
