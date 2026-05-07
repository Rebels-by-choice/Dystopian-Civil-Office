using CivilServiceViewer.Services;
using Microsoft.AspNetCore.Mvc;

namespace CivilServiceViewer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VPersonBirthController : ControllerBase, IVPersonBirthController
    {
        private readonly IVPersonBirthService _service;

        public VPersonBirthController(IVPersonBirthService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var items = await _service.GetVPersonBirthsAsync();
            return Ok(items);
        }
    }
}
