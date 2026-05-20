using Dystopian_Civil_Office.Dtos.Responses;
using Dystopian_Civil_Office.Services.Read.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dystopian_Civil_Office.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MarriageController : ControllerBase
{
    private readonly IMarriageService _marriageService;

    public MarriageController(IMarriageService marriageService)
    {
        _marriageService = marriageService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MarriageResponseDto>>> GetResponseAsync()
    {
        var marriages = await _marriageService.GetMarriagesAsync();
        return Ok(marriages);
    }
}