using Dystopian_Civil_Office.Dtos.Requests.Create;
using Dystopian_Civil_Office.Dtos.Requests.Update;

namespace Dystopian_Civil_Office.Services.Write.Interfaces;

public interface IDeathRecordWriteService
{
    Task CreateDeathRecordAsync(CreateDeathRecordRequestDto request, CancellationToken cancellationToken = default);
    Task UpdateDeathRecordAsync(int deathRecordId, UpdateDeathRecordRequestDto request, CancellationToken cancellationToken = default);
    Task DeleteDeathRecordAsync(int deathRecordId, CancellationToken cancellationToken = default);
}
