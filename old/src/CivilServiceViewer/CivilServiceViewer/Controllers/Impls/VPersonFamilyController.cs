using CivilServiceViewer.Services;
using Microsoft.AspNetCore.Mvc;

namespace CivilServiceViewer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VPersonFamilyController : ControllerBase, IVPersonFamilyController
    {
        private readonly IVPersonFamilyService _service;

        public VPersonFamilyController(IVPersonFamilyService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var items = await _service.GetVPersonFamiliesAsync();
            return Ok(items);
        }
    }
}
