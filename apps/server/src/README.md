# DystopianCivilService

This project was built with [ASP.NET Core](https://learn.microsoft.com/aspnet/core) and targets a backend API for the Dystopian Civil Office system.

## Development server

To start the local development server, run:

```bash
dotnet run
```

For hot reload:

```bash
dotnet watch run
```

By default, the application will start on the URLs configured in the launch profile or application settings. Once the server is running, open Swagger or your API client and navigate to the configured local address, for example for default https://localhost:5159/.

## Restoring dependencies

To restore the project dependencies, run:

```bash
dotnet restore
```

This command downloads all NuGet packages required by the application.

## Building

To build the project, run:

```bash
dotnet build
```

This compiles the backend application and verifies that the codebase builds correctly.

## Running the application

To run the API in the current environment, use:

```bash
dotnet run
```

For development, it is recommended to use the `Development` environment so that debugging tools, Swagger, and detailed logs are available.

## Running unit tests

To execute unit tests, run:

```bash
dotnet test
```

This command builds the solution and runs all configured test projects.

## Database migrations

The project uses Entity Framework Core migrations, you can update the database with:

```bash
dotnet ef database update
```

To create a new migration, run:

```bash
dotnet ef migrations add MigrationName
```

## API documentation

Swagger is enabled in project in developer environment by default, the API documentation and testing tool will be available after startup under:

```text
/swagger
```

This provides an interactive interface for testing endpoints and reviewing request and response models.

## Publishing

To publish the project for deployment, run:

```bash
dotnet publish -c Release
```

The published build artifacts will be generated in the `bin/Release/` directory under the target framework folder.

## Additional Resources

For more information about ASP.NET Core, Entity Framework Core, and .NET CLI commands, see the following resources:

- [ASP.NET Core documentation](https://learn.microsoft.com/aspnet/core)
- [Entity Framework Core documentation](https://learn.microsoft.com/ef/core)
- [.NET CLI overview](https://learn.microsoft.com/dotnet/core/tools/)