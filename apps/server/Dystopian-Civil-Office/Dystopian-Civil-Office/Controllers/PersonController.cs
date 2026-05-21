using Dystopian_Civil_Office.Dtos.Requests.Create;
using Dystopian_Civil_Office.Dtos.Requests.Update;
using Dystopian_Civil_Office.Dtos.Responses;
using Dystopian_Civil_Office.Services.Read.Interfaces;
using Dystopian_Civil_Office.Services.Write.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dystopian_Civil_Office.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonController : ControllerBase
{
    private readonly IPersonReadService _personReadService;
    private readonly IPersonWriteService _personWriteService;

    public PersonController(
        IPersonReadService personReadService,
        IPersonWriteService personWriteService)
    {
        _personReadService = personReadService;
        _personWriteService = personWriteService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PersonResponseDto>>> GetResponseAsync()
    {
        var persons = await _personReadService.GetPersonsAsync();
        return Ok(persons);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreatePersonRequestDto request,
        CancellationToken cancellationToken)
    {
        await _personWriteService.CreatePersonAsync(request, cancellationToken);
        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpPut("{personId:int}")]
    public async Task<IActionResult> UpdateAsync(
        int personId,
        [FromBody] UpdatePersonRequestDto request,
        CancellationToken cancellationToken)
    {
        await _personWriteService.UpdatePersonAsync(personId, request, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{personId:int}")]
    public async Task<IActionResult> DeleteAsync(
        int personId,
        CancellationToken cancellationToken)
    {
        await _personWriteService.DeletePersonAsync(personId, cancellationToken);
        return NoContent();
    }
}
