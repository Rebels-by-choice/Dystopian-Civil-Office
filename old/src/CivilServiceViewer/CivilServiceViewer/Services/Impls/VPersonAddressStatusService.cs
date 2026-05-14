using CivilServiceViewer.Models;

namespace CivilServiceViewer.Services
{
    public class VPersonAddressStatusService : IVPersonAddressStatusService
    {
        private readonly IDataOperationsService _dataFetch;

        public VPersonAddressStatusService(IDataOperationsService dataFetch)
        {
            _dataFetch = dataFetch;
        }

        public async Task<IEnumerable<VPersonAddressStatus>> GetVPersonAddressStatusesAsync() =>
            await _dataFetch.FetchDataAsync<VPersonAddressStatus>();
    }
}
