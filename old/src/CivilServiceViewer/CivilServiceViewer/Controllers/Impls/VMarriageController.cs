using CivilServiceViewer.Services;
using Microsoft.AspNetCore.Mvc;

namespace CivilServiceViewer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VMarriageController : ControllerBase, IVMarriageController
    {
        private readonly IVMarriageService _marriageService;

        public VMarriageController(IVMarriageService marriageService)
        {
            _marriageService = marriageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var marriages = await _marriageService.GetVMarriagesAsync();
            return Ok(marriages);
        }
    }
}
