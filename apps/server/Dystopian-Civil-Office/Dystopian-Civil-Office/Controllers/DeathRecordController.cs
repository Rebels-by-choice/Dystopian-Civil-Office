using Dystopian_Civil_Office.Dtos.Requests.Create;
using Dystopian_Civil_Office.Dtos.Requests.Update;
using Dystopian_Civil_Office.Dtos.Responses;
using Dystopian_Civil_Office.Services.Read.Interfaces;
using Dystopian_Civil_Office.Services.Write.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dystopian_Civil_Office.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DeathRecordController : ControllerBase
{
    private readonly IDeathRecordReadService _deathRecordReadService;
    private readonly IDeathRecordWriteService _deathRecordWriteService;

    public DeathRecordController(
        IDeathRecordReadService deathRecordReadService,
        IDeathRecordWriteService deathRecordWriteService)
    {
        _deathRecordReadService = deathRecordReadService;
        _deathRecordWriteService = deathRecordWriteService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DeathRecordResponseDto>>> GetResponseAsync()
    {
        var deathRecords = await _deathRecordReadService.GetDeathRecordsAsync();
        return Ok(deathRecords);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreateDeathRecordRequestDto request,
        CancellationToken cancellationToken)
    {
        await _deathRecordWriteService.CreateDeathRecordAsync(request, cancellationToken);
        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpPut("{deathRecordId:int}")]
    public async Task<IActionResult> UpdateAsync(
        int deathRecordId,
        [FromBody] UpdateDeathRecordRequestDto request,
        CancellationToken cancellationToken)
    {
        await _deathRecordWriteService.UpdateDeathRecordAsync(deathRecordId, request, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{deathRecordId:int}")]
    public async Task<IActionResult> DeleteAsync(
        int deathRecordId,
        CancellationToken cancellationToken)
    {
        await _deathRecordWriteService.DeleteDeathRecordAsync(deathRecordId, cancellationToken);
        return NoContent();
    }
}
