using Dystopian_Civil_Office.Dtos.Responses;
using Dystopian_Civil_Office.Services.Read.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dystopian_Civil_Office.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DeathRecordController : ControllerBase
{
    private readonly IDeathRecordReadService _deathRecordService;

    public DeathRecordController(IDeathRecordReadService deathRecordService)
    {
        _deathRecordService = deathRecordService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DeathRecordResponseDto>>> GetResponseAsync()
    {
        var deathRecords = await _deathRecordService.GetDeathRecordsAsync();
        return Ok(deathRecords);
    }
}
