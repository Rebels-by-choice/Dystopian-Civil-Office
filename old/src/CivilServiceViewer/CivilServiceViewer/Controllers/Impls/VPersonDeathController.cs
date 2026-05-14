using CivilServiceViewer.Services;
using Microsoft.AspNetCore.Mvc;

namespace CivilServiceViewer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VPersonDeathController : ControllerBase, IVPersonDeathController
    {
        private readonly IVPersonDeathService _service;

        public VPersonDeathController(IVPersonDeathService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var items = await _service.GetVPersonDeathsAsync();
            return Ok(items);
        }
    }
}
