using Dystopian_Civil_Office.Services.Read.Interfaces;

namespace Dystopian_Civil_Office.Middleware;

public class DmlTrackerMiddleware
{
    private readonly RequestDelegate _next;

    public DmlTrackerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IApiStatsService apiStatsService)
    {
        await _next(context);

        var method = context.Request.Method.ToUpper();
        var path = context.Request.Path.Value ?? "";
        var statusCode = context.Response.StatusCode;
        
        if (method != "GET" && !path.Contains("/api/stats"))
        {
            apiStatsService.LogOperation(method, path, statusCode);
        }
    }
}