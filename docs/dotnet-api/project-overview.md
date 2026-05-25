# Project Overview

## Purpose

`Dystopian-Civil-Office` is a backend API for managing civil-office records. The domain includes:

- People and their personal data.
- Addresses assigned to people.
- Documents attached to domain records.
- Birth records.
- Death records.
- Marriage records.
- Archive records created when live data is deleted.
- In-memory API operation logs for non-GET requests.

## Technology Stack

| Area | Implementation |
| --- | --- |
| Runtime | ASP.NET Core Web API |
| Target framework | `.NET 10.0` |
| Persistence | Entity Framework Core |
| Database provider | Npgsql Entity Framework Core PostgreSQL provider |
| API documentation | Swagger via Swashbuckle |
| Container support | Dockerfile and `Microsoft.NET.Build.Containers` |
| Main namespace | `Dystopian_Civil_Office` |
| Nullable references | Enabled |
| Implicit usings | Enabled |

Main NuGet packages from `Dystopian-Civil-Office.csproj`:

- `Microsoft.AspNetCore.OpenApi`
- `Microsoft.EntityFrameworkCore.Design`
- `Microsoft.EntityFrameworkCore.Tools`
- `Microsoft.NET.Build.Containers`
- `Npgsql.EntityFrameworkCore.PostgreSQL`
- `Swashbuckle.AspNetCore`

## Runtime Configuration

`Program.cs` wires the application:

1. Registers MVC/API controllers.
2. Registers `ApplicationDbContext` with `UseNpgsql`.
3. Registers read services, write services, and `OfficeQueryService`.
4. Registers `IApiStatsService` as a singleton.
5. Enables Swagger only for Development.
6. Adds HTTPS redirection.
7. Adds database exception middleware.
8. Adds DML tracking middleware.
9. Adds authorization middleware.
10. Maps controllers.

## Configuration Files

| File | Purpose |
| --- | --- |
| `appsettings.json` | Default configuration. Contains an empty `DefaultConnection` placeholder. |
| `appsettings.Development.json` | Development-specific app settings. |
| `Properties/launchSettings.json` | Local run profiles for HTTP, HTTPS, and Docker. |
| `Dockerfile` | Container image build/run definition. |
| `Dystopian-Civil-Office.http` | Manual HTTP request scratch file. |

The configured local URLs are:

| Profile | URL |
| --- | --- |
| `http` | `http://localhost:5159` |
| `https` | `https://localhost:7083` and `http://localhost:5159` |
| Docker | container ports `8080` HTTP and `8081` HTTPS |

## Source Layout

| Folder | Responsibility |
| --- | --- |
| `Controllers` | HTTP controllers and API routes. |
| `DataSource` | EF Core `ApplicationDbContext`. |
| `Dtos` | Request and response DTOs used by controllers/services. |
| `InitDb/Sql` | SQL scripts for cleanup, validation, stored procedures, triggers, and seed data. |
| `Middleware` | Custom ASP.NET Core middleware. |
| `Migrations` | EF Core migration and model snapshot. |
| `Models/Entities` | Live database entity classes. |
| `Models/Archives` | Archive entity classes. |
| `Services` | Read services, write services, shared query service, and interfaces. |
| `ViewModels` | Non-persistent view models such as API operation logs. |

## High-Level Behavior

Reads and writes use different paths:

- Reads: controller -> read service -> `OfficeQueryService` -> EF Core query -> response DTO.
- Writes: controller -> write service -> PostgreSQL stored procedure -> database validation/mutation -> HTTP status.

This means business rules are split between C# and SQL:

- C# controls request routing, DTO contracts, query projections, middleware, and dependency injection.
- PostgreSQL controls most write validation, mutation procedure behavior, and archive-on-delete behavior.
