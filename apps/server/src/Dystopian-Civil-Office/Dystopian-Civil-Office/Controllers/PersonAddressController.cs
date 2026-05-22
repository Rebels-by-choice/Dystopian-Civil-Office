using Dystopian_Civil_Office.Dtos.Requests.Create;
using Dystopian_Civil_Office.Dtos.Requests.Update;
using Dystopian_Civil_Office.Dtos.Responses;
using Dystopian_Civil_Office.Services.Read.Interfaces;
using Dystopian_Civil_Office.Services.Write.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dystopian_Civil_Office.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonAddressController : ControllerBase
{
    private readonly IPersonAddressReadService _personAddressReadService;
    private readonly IPersonAddressWriteService _personAddressWriteService;

    public PersonAddressController(
        IPersonAddressReadService personAddressReadService,
        IPersonAddressWriteService personAddressWriteService)
    {
        _personAddressReadService = personAddressReadService;
        _personAddressWriteService = personAddressWriteService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PersonAddressResponseDto>>> GetResponseAsync()
    {
        var personAddresses = await _personAddressReadService.GetPersonAddressesAsync();
        return Ok(personAddresses);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreatePersonAddressRequestDto request,
        CancellationToken cancellationToken)
    {
        await _personAddressWriteService.CreatePersonAddressAsync(request, cancellationToken);
        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpPut("{addressId:int}")]
    public async Task<IActionResult> UpdateAsync(
        int addressId,
        [FromBody] UpdatePersonAddressRequestDto request,
        CancellationToken cancellationToken)
    {
        await _personAddressWriteService.UpdatePersonAddressAsync(addressId, request, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{addressId:int}")]
    public async Task<IActionResult> DeleteAsync(
        int addressId,
        CancellationToken cancellationToken)
    {
        await _personAddressWriteService.DeletePersonAddressAsync(addressId, cancellationToken);
        return NoContent();
    }
}
