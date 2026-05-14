using CivilServiceViewer.Models;

namespace CivilServiceViewer.Services
{
    public class VPersonDeathService : IVPersonDeathService
    {
        private readonly IDataOperationsService _dataFetch;

        public VPersonDeathService(IDataOperationsService dataFetch)
        {
            _dataFetch = dataFetch;
        }

        public async Task<IEnumerable<VPersonDeath>> GetVPersonDeathsAsync() =>
            await _dataFetch.FetchDataAsync<VPersonDeath>();
    }
}
