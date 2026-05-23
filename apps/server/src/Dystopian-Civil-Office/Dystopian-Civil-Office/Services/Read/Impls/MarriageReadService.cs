using Dystopian_Civil_Office.Dtos.Responses;
using Dystopian_Civil_Office.Services.Read.Interfaces;

namespace Dystopian_Civil_Office.Services.Read.Impls;

public class MarriageReadService : IMarriageReadService
{
    private readonly OfficeQueryService _officeQueryService;

    public MarriageReadService(OfficeQueryService officeQueryService)
    {
        _officeQueryService = officeQueryService;
    }

    public async Task<IEnumerable<MarriageResponseDto>> GetMarriagesAsync()
    {
        return await _officeQueryService.GetMarriageRecordsAsync();
    }
}