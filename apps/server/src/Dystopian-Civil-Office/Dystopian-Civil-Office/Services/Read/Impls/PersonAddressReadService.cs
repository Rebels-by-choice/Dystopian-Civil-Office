using Dystopian_Civil_Office.Dtos.Responses;
using Dystopian_Civil_Office.Services.Read.Interfaces;

namespace Dystopian_Civil_Office.Services.Read.Impls;

public class PersonAddressReadService : IPersonAddressReadService
{
    private readonly OfficeQueryService _officeQueryService;

    public PersonAddressReadService(OfficeQueryService officeQueryService)
    {
        _officeQueryService = officeQueryService;
    }

    public async Task<IEnumerable<PersonAddressResponseDto>> GetPersonAddressesAsync()
    {
        return await _officeQueryService.GetPersonAddressesAsync();
    }
}
