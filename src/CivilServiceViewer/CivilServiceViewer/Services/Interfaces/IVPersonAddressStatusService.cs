using CivilServiceViewer.Models;

namespace CivilServiceViewer.Services
{
    public interface IVPersonAddressStatusService
    {
        Task<IEnumerable<VPersonAddressStatus>> GetVPersonAddressStatusesAsync();
    }
}
