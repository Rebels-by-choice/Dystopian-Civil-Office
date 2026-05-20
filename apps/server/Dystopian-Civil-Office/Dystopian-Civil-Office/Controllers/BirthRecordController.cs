using Dystopian_Civil_Office.Dtos.Responses;
using Dystopian_Civil_Office.Services.Read.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dystopian_Civil_Office.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BirthRecordController : ControllerBase
{
    private readonly IBirthRecordService _birthRecordService;

    public BirthRecordController(IBirthRecordService birthRecordService)
    {
        _birthRecordService = birthRecordService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BirthRecordResponseDto>>> GetResponseAsync()
    {
        var birthRecords = await _birthRecordService.GetBirthRecordsAsync();
        return Ok(birthRecords);
    }
}
