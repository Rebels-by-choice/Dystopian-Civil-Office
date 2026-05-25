# Maintenance Notes

## What To Keep In Sync

The most important maintenance rule is to keep C# and SQL contracts aligned.

When changing a write endpoint, check:

- Controller route and action signature.
- Create/update request DTO.
- Write service method.
- `NpgsqlParameter` names and values.
- PostgreSQL procedure name and parameter order.
- Validation function behavior.
- Archive trigger behavior if delete semantics change.

When changing a read endpoint, check:

- Response DTO.
- `OfficeQueryService` projection.
- EF entity relationships required by the projection.
- Controller return type.

## Naming Observations

Most read service interfaces follow the pattern `I<Entity>ReadService`, but birth records use `IBirthRecordService`. This works, but a future cleanup could rename it to `IBirthRecordReadService` for consistency.

`PersonAddressController` writes address rows but reads person-address projections. That naming is understandable from the client point of view, but maintainers should remember the database table is `addresses`.

## Persistence Boundaries

The API has two persistence styles:

- Reads use EF Core queries.
- Writes use PostgreSQL stored procedures.

This is valid, but it means tests should cover both the C# service layer and the SQL layer. A mocked DbContext test will not prove that write behavior works, because the real write rules are inside PostgreSQL.

## Documentation Gaps To Consider Next

The current source does not include:

- A generated OpenAPI document committed to the repository.
- XML comments on controllers/DTOs for richer Swagger output.
- A database initialization runner that applies embedded SQL scripts automatically.
- Authentication/authorization documentation.
- Automated tests documenting expected behavior.

Good next improvements:

- Enable XML documentation generation and add summaries to controllers and DTOs.
- Add examples for each request body in Swagger.
- Add integration tests against a PostgreSQL test container.
- Add a database setup script that applies migrations and SQL objects in one repeatable flow.
- Persist API operation logs if they are intended as real audit data.

## API Compatibility Notes

The update DTOs use nullable properties. A null value means "do not change this field" in the current service/procedure design. If clients ever need to explicitly set a nullable database field to null, the API will need a different patch representation or explicit clearing endpoints.

Marriage update/delete uses `registryNumber` in the route. Other update/delete endpoints use integer IDs. Clients must handle that difference.

## Error Handling Notes

`DatabaseExceptionHandlingMiddleware` currently detects not-found errors by checking whether the PostgreSQL message text contains `not found`. This depends on SQL exception message wording. A more robust long-term approach would use custom SQLSTATE codes or structured exception details.

Only `PostgresException` is mapped to `ProblemDetails`. Validation errors from ASP.NET Core model binding are handled by `[ApiController]`, but non-database runtime exceptions are not mapped by this custom middleware.
