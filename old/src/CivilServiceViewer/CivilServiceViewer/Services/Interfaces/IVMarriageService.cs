using CivilServiceViewer.Models;

namespace CivilServiceViewer.Services
{
    public interface IVMarriageService
    {
        Task<IEnumerable<VMarriage>> GetVMarriagesAsync();
    }
}
