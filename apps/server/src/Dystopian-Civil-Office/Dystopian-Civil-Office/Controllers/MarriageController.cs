using Dystopian_Civil_Office.Dtos.Requests.Create;
using Dystopian_Civil_Office.Dtos.Requests.Update;
using Dystopian_Civil_Office.Dtos.Responses;
using Dystopian_Civil_Office.Services.Read.Interfaces;
using Dystopian_Civil_Office.Services.Write.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dystopian_Civil_Office.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MarriageController : ControllerBase
{
    private readonly IMarriageReadService _marriageReadService;
    private readonly IMarriageWriteService _marriageWriteService;

    public MarriageController(
        IMarriageReadService marriageReadService,
        IMarriageWriteService marriageWriteService)
    {
        _marriageReadService = marriageReadService;
        _marriageWriteService = marriageWriteService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MarriageResponseDto>>> GetResponseAsync()
    {
        var marriages = await _marriageReadService.GetMarriagesAsync();
        return Ok(marriages);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreateMarriageRequestDto request,
        CancellationToken cancellationToken)
    {
        await _marriageWriteService.CreateMarriageAsync(request, cancellationToken);
        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpPut("{registryNumber}")]
    public async Task<IActionResult> UpdateAsync(
        string registryNumber,
        [FromBody] UpdateMarriageRequestDto request,
        CancellationToken cancellationToken)
    {
        await _marriageWriteService.UpdateMarriageAsync(registryNumber, request, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{registryNumber}")]
    public async Task<IActionResult> DeleteAsync(
        string registryNumber,
        CancellationToken cancellationToken)
    {
        await _marriageWriteService.DeleteMarriageAsync(registryNumber, cancellationToken);
        return NoContent();
    }
}
