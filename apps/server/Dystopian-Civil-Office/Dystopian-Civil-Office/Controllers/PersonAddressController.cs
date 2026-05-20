using Dystopian_Civil_Office.Dtos.Responses;
using Dystopian_Civil_Office.Services.Read.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dystopian_Civil_Office.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonAddressController : ControllerBase
{
    private readonly IPersonAddressService _personAddressService;

    public PersonAddressController(IPersonAddressService personAddressService)
    {
        _personAddressService = personAddressService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PersonAddressResponseDto>>> GetResponseAsync()
    {
        var personAddresses = await _personAddressService.GetPersonAddressesAsync();
        return Ok(personAddresses);
    }
}
