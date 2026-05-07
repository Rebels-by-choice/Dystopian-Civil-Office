using CivilServiceViewer.Models;

namespace CivilServiceViewer.Services
{
    public interface IVPersonDeathService
    {
        Task<IEnumerable<VPersonDeath>> GetVPersonDeathsAsync();
    }
}
