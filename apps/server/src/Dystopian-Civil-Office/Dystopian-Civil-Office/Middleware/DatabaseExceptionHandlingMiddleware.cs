using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace Dystopian_Civil_Office.Middleware;

public class DatabaseExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public DatabaseExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (PostgresException ex)
        {
            var statusCode = GetStatusCode(ex);

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/problem+json";

            await context.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = statusCode,
                Title = GetTitle(statusCode),
                Detail = ex.MessageText
            });
        }
    }

    private static int GetStatusCode(PostgresException exception)
    {
        if (exception.MessageText.Contains("not found", StringComparison.OrdinalIgnoreCase))
        {
            return StatusCodes.Status404NotFound;
        }

        return exception.SqlState switch
        {
            PostgresErrorCodes.UniqueViolation => StatusCodes.Status409Conflict,
            PostgresErrorCodes.ForeignKeyViolation => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status400BadRequest
        };
    }

    private static string GetTitle(int statusCode)
    {
        return statusCode switch
        {
            StatusCodes.Status404NotFound => "Resource not found",
            StatusCodes.Status409Conflict => "Database conflict",
            _ => "Invalid database request"
        };
    }
}
