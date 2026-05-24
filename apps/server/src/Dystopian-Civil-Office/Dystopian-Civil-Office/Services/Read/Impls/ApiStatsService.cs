using System.Collections.Concurrent;
using Dystopian_Civil_Office.Services.Read.Interfaces;
using Dystopian_Civil_Office.ViewModels;

namespace Dystopian_Civil_Office.Services.Read.Impls;

public class ApiStatsService : IApiStatsService 
{
    private readonly ConcurrentBag<ApiOperationLog> _logs = new();
    private int _currentId = 0;

    public void LogOperation(string method, string path, int statusCode)
    {
        var log = new ApiOperationLog
        {
            Id = Interlocked.Increment(ref _currentId),
            Method = method.ToUpperInvariant(),
            Path = path,
            StatusCode = statusCode,
            ExecutedAt = DateTime.UtcNow
        };

        _logs.Add(log);
    }

    public IEnumerable<ApiOperationLog> GetAllLogs()
    {
        return _logs.OrderByDescending(x => x.Id);
    }
}