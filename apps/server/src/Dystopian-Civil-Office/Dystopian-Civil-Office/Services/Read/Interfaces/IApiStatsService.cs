using Dystopian_Civil_Office.ViewModels;

namespace Dystopian_Civil_Office.Services.Read.Interfaces;

public interface IApiStatsService
{
    void LogOperation(string method, string path, int statusCode);
    IEnumerable<ApiOperationLog> GetAllLogs();
}