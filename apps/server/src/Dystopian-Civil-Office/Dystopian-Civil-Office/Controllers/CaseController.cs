using Dystopian_Civil_Office.Dtos.Requests.Create;
using Dystopian_Civil_Office.Dtos.Requests.Update;
using Dystopian_Civil_Office.Services.Write.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dystopian_Civil_Office.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CaseController(ICaseService caseService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var dtoCases = await caseService.GetAllAsync();
        return Ok(dtoCases);
    }
    
    [HttpGet("{caseId:int}")]
    public async Task<IActionResult> GetAsync(int caseId)
    {
        var caseDto = await caseService.GetAsync(caseId);
        return Ok(caseDto);
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> CreateAsync(CreateCaseRequestDto request)
    {
        await caseService.CreateAsync(request);
        return Ok();
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateAsync(UpdateCaseRequestDto request)
    {
        await caseService.UpdateStatusAsync(request);
        return Ok();
    }
}