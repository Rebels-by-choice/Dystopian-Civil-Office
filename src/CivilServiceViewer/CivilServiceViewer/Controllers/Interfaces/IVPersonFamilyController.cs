using Microsoft.AspNetCore.Mvc;

namespace CivilServiceViewer.Controllers
{
    public interface IVPersonFamilyController
    {
        Task<IActionResult> GetAsync();
    }
}
