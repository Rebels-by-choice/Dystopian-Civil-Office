using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;

namespace Dystopian_Civil_Office.Exceptions;

public class ViewDataExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        httpContext.Response.ContentType = "application/json";

        object response;

        switch (exception)
        {
            case DatasetNotFoundException datasetNotFoundException:
                httpContext.Response.StatusCode = datasetNotFoundException.StatusCode;
                response = new
                {
                    title = datasetNotFoundException.Title,
                    status = datasetNotFoundException.StatusCode,
                    description = datasetNotFoundException.Description
                };
                break;

            default:
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                response = new
                {
                    title = "Internal server error",
                    status = StatusCodes.Status500InternalServerError,
                    description = "Unexpected server error occurred."
                };
                break;
        }

        await httpContext.Response.WriteAsync(
            JsonSerializer.Serialize(response),
            cancellationToken);

        return true;
    }
}