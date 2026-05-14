using CivilServiceViewer.Models;

namespace CivilServiceViewer.Services
{
    public interface IVPersonBirthService
    {
        Task<IEnumerable<VPersonBirth>> GetVPersonBirthsAsync();
    }
}
