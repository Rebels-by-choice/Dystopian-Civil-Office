using Dystopian_Civil_Office.Services.Read.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dystopian_Civil_Office.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApiStatsController : ControllerBase
{
    private readonly IApiStatsService _apiStatsService;

    public ApiStatsController(IApiStatsService apiStatsService)
    {
        _apiStatsService = apiStatsService;
    }

    [HttpGet]
    public IActionResult GetApiStats()
    {
        var logs = _apiStatsService.GetAllLogs().ToList();
        
        return Ok(logs);
    }
}