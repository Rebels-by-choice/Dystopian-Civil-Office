# Dystopian Civil Office Backend Documentation

This folder documents the backend project in this directory. It is written from the current source code, not from generated Swagger output.

The backend is an ASP.NET Core Web API targeting `.NET 10.0`. It exposes controller-based REST-style endpoints for civil-office data: persons, addresses, documents, birth records, death records, marriage records, and API operation statistics. The application uses Entity Framework Core with the Npgsql PostgreSQL provider. Read operations are implemented with EF Core LINQ projections, while write operations call PostgreSQL stored procedures.

## Documentation Map

- [project-overview.md](project-overview.md): purpose, stack, runtime wiring, and source layout.
- [architecture.md](architecture.md): request flow, dependency injection, layers, and major design decisions.
- [api-reference.md](api-reference.md): controllers, routes, DTOs, response codes, and endpoint behavior.
- [data-model.md](data-model.md): entity model, archive model, relationships, constraints, and DTO mapping.
- [database-and-sql.md](database-and-sql.md): PostgreSQL connection, EF Core context, migrations, stored procedures, validation functions, archive triggers, and seed scripts.
- [services-and-business-logic.md](services-and-business-logic.md): read services, write services, query service, and operation logging.
- [middleware-and-error-handling.md](middleware-and-error-handling.md): database exception handling and DML tracking.
- [development-and-operations.md](development-and-operations.md): local run, Swagger, Docker, configuration, migrations, and operational notes.
- [maintenance-notes.md](maintenance-notes.md): known implementation details, risks, and future documentation improvements.
- [references.md](references.md): external documentation references used to shape this documentation.

## Quick Start

1. Configure the PostgreSQL connection string under `ConnectionStrings:DefaultConnection`.
2. Run the API with:

```powershell
dotnet run
```

3. In Development, open Swagger UI at:

```text
http://localhost:5159/swagger
https://localhost:7083/swagger
```

4. The main API base path is:

```text
/api/{ControllerName}
```

## Current Backend Shape

The project has a pragmatic split:

- Controllers receive HTTP requests and return HTTP responses.
- DTOs define request and response contracts.
- Read services delegate to `OfficeQueryService`, which projects database entities into response DTOs.
- Write services call PostgreSQL stored procedures with explicit Npgsql parameters.
- `ApplicationDbContext` maps EF Core entities to PostgreSQL tables and relationships.
- Middleware converts PostgreSQL errors into HTTP problem responses and logs write operations.

Swagger/OpenAPI is enabled in Development through `AddSwaggerGen`, `UseSwagger`, and `UseSwaggerUI`.
