namespace CivilServiceViewer.Services
{
    public interface IDataOperationsService
    {
        Task<IEnumerable<T>> FetchDataAsync<T>() where T : class;
    }
}
