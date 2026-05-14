using CivilServiceViewer.Models;

namespace CivilServiceViewer.Services
{
    public class VPersonFamilyService : IVPersonFamilyService
    {
        private readonly IDataOperationsService _dataFetch;

        public VPersonFamilyService(IDataOperationsService dataFetch)
        {
            _dataFetch = dataFetch;
        }

        public async Task<IEnumerable<VPersonFamily>> GetVPersonFamiliesAsync() =>
            await _dataFetch.FetchDataAsync<VPersonFamily>();
    }
}
