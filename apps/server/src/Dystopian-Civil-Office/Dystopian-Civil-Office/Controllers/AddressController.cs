using Dystopian_Civil_Office.Dtos.Requests.Create;
using Dystopian_Civil_Office.Dtos.Requests.Update;
using Dystopian_Civil_Office.Dtos.Responses;
using Dystopian_Civil_Office.Services.Read.Interfaces;
using Dystopian_Civil_Office.Services.Write.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dystopian_Civil_Office.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AddressController : ControllerBase
{
    private readonly IAddressReadService _addressReadService;
    private readonly IAddressWriteService _addressWriteService;

    public AddressController(
        IAddressReadService addressReadService,
        IAddressWriteService addressWriteService)
    {
        _addressReadService = addressReadService;
        _addressWriteService = addressWriteService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AddressResponseDto>>> GetResponseAsync()
    {
        var personAddresses = await _addressReadService.GetAddressesAsync();
        return Ok(personAddresses);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreateAddressRequestDto request,
        CancellationToken cancellationToken)
    {
        await _addressWriteService.CreateAddressAsync(request, cancellationToken);
        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpPut("{addressId:int}")]
    public async Task<IActionResult> UpdateAsync(
        int addressId,
        [FromBody] UpdateAddressRequestDto request,
        CancellationToken cancellationToken)
    {
        await _addressWriteService.UpdateAddressAsync(addressId, request, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{addressId:int}")]
    public async Task<IActionResult> DeleteAsync(
        int addressId,
        CancellationToken cancellationToken)
    {
        await _addressWriteService.DeleteAddressAsync(addressId, cancellationToken);
        return NoContent();
    }
}
