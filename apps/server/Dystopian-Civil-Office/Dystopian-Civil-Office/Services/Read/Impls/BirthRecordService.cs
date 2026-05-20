using Dystopian_Civil_Office.Dtos.Responses;
using Dystopian_Civil_Office.Services.Read.Interfaces;

namespace Dystopian_Civil_Office.Services.Read.Impls;

public class BirthRecordService : IBirthRecordService
{
    private readonly OfficeQueryService _officeQueryService;

    public BirthRecordService(OfficeQueryService officeQueryService)
    {
        _officeQueryService = officeQueryService;
    }

    public async Task<IEnumerable<BirthRecordResponseDto>> GetBirthRecordsAsync()
    {
        return await _officeQueryService.GetBirthRecordsAsync();
    }
}
