using Microsoft.AspNetCore.Mvc;

namespace CivilServiceViewer.Controllers
{
    public interface IVPersonDeathController
    {
        Task<IActionResult> GetAsync();
    }
}
