using Microsoft.AspNetCore.Mvc;

namespace CivilServiceViewer.Controllers
{
    public interface IVPersonAddressStatusController
    {
        Task<IActionResult> GetAsync();
    }
}
