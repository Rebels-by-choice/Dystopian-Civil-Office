using Dystopian_Civil_Office.Dtos.Responses;

namespace Dystopian_Civil_Office.Services.Read.Interfaces;

public interface IDeathRecordReadService
{
    Task<IEnumerable<DeathRecordResponseDto>> GetDeathRecordsAsync();
}
