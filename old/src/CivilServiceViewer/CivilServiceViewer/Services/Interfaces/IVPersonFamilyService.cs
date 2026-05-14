using CivilServiceViewer.Models;

namespace CivilServiceViewer.Services
{
    public interface IVPersonFamilyService
    {
        Task<IEnumerable<VPersonFamily>> GetVPersonFamiliesAsync();
    }
}
