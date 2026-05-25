# Middleware And Error Handling

## Middleware Pipeline

The custom middleware is registered in this order:

```csharp
app.UseMiddleware<DatabaseExceptionHandlingMiddleware>();
app.UseMiddleware<DmlTrackerMiddleware>();
```

This means the database exception middleware wraps the DML tracker and the controller execution that follows.

## DatabaseExceptionHandlingMiddleware

File:

```text
Middleware/DatabaseExceptionHandlingMiddleware.cs
```

Purpose:

- Catch `PostgresException`.
- Choose an HTTP status code.
- Return a JSON `ProblemDetails` response.

Response content type:

```text
application/problem+json
```

Status mapping:

| PostgreSQL condition | HTTP status | Title |
| --- | --- | --- |
| Exception message contains `not found` | `404` | `Resource not found` |
| `PostgresErrorCodes.UniqueViolation` | `409` | `Database conflict` |
| `PostgresErrorCodes.ForeignKeyViolation` | `409` | `Database conflict` |
| Any other PostgreSQL exception | `400` | `Invalid database request` |

The `ProblemDetails.Detail` field is populated with `ex.MessageText`.

## DmlTrackerMiddleware

File:

```text
Middleware/DmlTrackerMiddleware.cs
```

Purpose:

- Run the downstream request.
- Read the final request method, path, and response status code.
- Log non-GET operations through `IApiStatsService`.

Logic:

```text
if method is not GET and path does not contain /api/stats:
    log method, path, status code
```

This logs operations such as:

- `POST /api/Person`
- `PUT /api/Document/1`
- `DELETE /api/Marriage/MR-123`

It does not log regular GET reads.

## Error Handling Flow

For a database-backed write request:

```text
Controller
  -> Write service
  -> PostgreSQL stored procedure
  -> PostgreSQL raises PostgresException
  -> DatabaseExceptionHandlingMiddleware catches it
  -> HTTP problem response returned
```

For successful non-GET requests:

```text
Controller returns status
  -> DmlTrackerMiddleware sees method/path/status
  -> ApiStatsService stores operation
  -> Client receives response
```

## Current Limitations

The middleware only catches `PostgresException`. Other exception types will follow ASP.NET Core's default exception behavior unless another handler is added.

The DML tracker stores logs in process memory. It is useful for local/admin visibility but is not durable audit logging.

The path exclusion checks for lowercase `/api/stats`, while the current stats endpoint is `/api/ApiStats`. Because the middleware only logs non-GET methods and the stats controller only has GET, this currently has no practical effect. If write endpoints are later added under `ApiStats`, the exclusion should be reviewed.
