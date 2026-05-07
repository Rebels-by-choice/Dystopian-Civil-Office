using CivilServiceViewer.Models;

namespace CivilServiceViewer.Services
{
    public class VMarriageService : IVMarriageService
    {
        private readonly IDataOperationsService _dataFetch;

        public VMarriageService(IDataOperationsService dataFetch)
        {
            _dataFetch = dataFetch;
        }

        public async Task<IEnumerable<VMarriage>> GetVMarriagesAsync() =>
            await _dataFetch.FetchDataAsync<VMarriage>();
    }
}
