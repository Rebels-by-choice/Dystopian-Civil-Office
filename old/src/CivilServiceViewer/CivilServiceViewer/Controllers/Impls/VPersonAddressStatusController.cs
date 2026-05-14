using CivilServiceViewer.Services;
using Microsoft.AspNetCore.Mvc;

namespace CivilServiceViewer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VPersonAddressStatusController : ControllerBase, IVPersonAddressStatusController
    {
        private readonly IVPersonAddressStatusService _service;

        public VPersonAddressStatusController(IVPersonAddressStatusService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var items = await _service.GetVPersonAddressStatusesAsync();
            return Ok(items);
        }
    }
}
