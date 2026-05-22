using Dystopian_Civil_Office.Dtos.Requests.Create;
using Dystopian_Civil_Office.Dtos.Requests.Update;
using Dystopian_Civil_Office.Dtos.Responses;
using Dystopian_Civil_Office.Services.Read.Interfaces;
using Dystopian_Civil_Office.Services.Write.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dystopian_Civil_Office.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BirthRecordController : ControllerBase
{
    private readonly IBirthRecordService _birthRecordReadService;
    private readonly IBirthRecordWriteService _birthRecordWriteService;

    public BirthRecordController(
        IBirthRecordService birthRecordReadService,
        IBirthRecordWriteService birthRecordWriteService)
    {
        _birthRecordReadService = birthRecordReadService;
        _birthRecordWriteService = birthRecordWriteService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BirthRecordResponseDto>>> GetResponseAsync()
    {
        var birthRecords = await _birthRecordReadService.GetBirthRecordsAsync();
        return Ok(birthRecords);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreateBirthRecordRequestDto request,
        CancellationToken cancellationToken)
    {
        await _birthRecordWriteService.CreateBirthRecordAsync(request, cancellationToken);
        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpPut("{birthRecordId:int}")]
    public async Task<IActionResult> UpdateAsync(
        int birthRecordId,
        [FromBody] UpdateBirthRecordRequestDto request,
        CancellationToken cancellationToken)
    {
        await _birthRecordWriteService.UpdateBirthRecordAsync(birthRecordId, request, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{birthRecordId:int}")]
    public async Task<IActionResult> DeleteAsync(
        int birthRecordId,
        CancellationToken cancellationToken)
    {
        await _birthRecordWriteService.DeleteBirthRecordAsync(birthRecordId, cancellationToken);
        return NoContent();
    }
}
