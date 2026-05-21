using Dystopian_Civil_Office.Dtos.Requests.Create;
using Dystopian_Civil_Office.Dtos.Requests.Update;

namespace Dystopian_Civil_Office.Services.Write.Interfaces;

public interface IBirthRecordWriteService
{
    Task CreateBirthRecordAsync(CreateBirthRecordRequestDto request, CancellationToken cancellationToken = default);
    Task UpdateBirthRecordAsync(int birthRecordId, UpdateBirthRecordRequestDto request, CancellationToken cancellationToken = default);
    Task DeleteBirthRecordAsync(int birthRecordId, CancellationToken cancellationToken = default);
}
