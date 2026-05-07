using Microsoft.AspNetCore.Mvc;

namespace CivilServiceViewer.Controllers
{
    public interface IVMarriageController
    {
        Task<IActionResult> GetAsync();
    }
}
