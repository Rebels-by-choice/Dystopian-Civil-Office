using Microsoft.AspNetCore.Mvc;

namespace CivilServiceViewer.Controllers
{
    public interface IVPersonBirthController
    {
        Task<IActionResult> GetAsync();
    }
}
