using CivilServiceViewer.Models;

namespace CivilServiceViewer.Services
{
    public class VPersonBirthService : IVPersonBirthService
    {
        private readonly IDataOperationsService _dataFetch;

        public VPersonBirthService(IDataOperationsService dataFetch)
        {
            _dataFetch = dataFetch;
        }

        public async Task<IEnumerable<VPersonBirth>> GetVPersonBirthsAsync()
        {
            return await _dataFetch.FetchDataAsync<VPersonBirth>();
        }
    }
}
