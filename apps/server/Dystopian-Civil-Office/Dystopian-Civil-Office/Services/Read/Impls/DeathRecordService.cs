using Dystopian_Civil_Office.Dtos.Responses;
using Dystopian_Civil_Office.Services.Read.Interfaces;

namespace Dystopian_Civil_Office.Services.Read.Impls;

public class DeathRecordService : IDeathRecordService
{
    private readonly OfficeQueryService _officeQueryService;

    public DeathRecordService(OfficeQueryService officeQueryService)
    {
        _officeQueryService = officeQueryService;
    }

    public async Task<IEnumerable<DeathRecordResponseDto>> GetDeathRecordsAsync()
    {
        return await _officeQueryService.GetDeathRecordsAsync();
    }
}
