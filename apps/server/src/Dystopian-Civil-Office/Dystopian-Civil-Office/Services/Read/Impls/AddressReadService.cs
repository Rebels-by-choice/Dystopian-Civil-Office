using Dystopian_Civil_Office.Dtos.Responses;
using Dystopian_Civil_Office.Services.Read.Interfaces;

namespace Dystopian_Civil_Office.Services.Read.Impls;

public class AddressReadService : IAddressReadService
{
    private readonly OfficeQueryService _officeQueryService;

    public AddressReadService(OfficeQueryService officeQueryService)
    {
        _officeQueryService = officeQueryService;
    }

    public async Task<IEnumerable<AddressResponseDto>> GetAddressesAsync()
    {
        return await _officeQueryService.GetAddressesAsync();
    }
}
