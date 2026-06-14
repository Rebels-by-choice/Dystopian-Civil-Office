using Dystopian_Civil_Office.Dtos.Requests.Create;
using Dystopian_Civil_Office.Dtos.Requests.Update;

namespace Dystopian_Civil_Office.Services.Write.Interfaces;

public interface IMarriageWriteService
{
    Task CreateMarriageAsync(CreateMarriageRequestDto request, CancellationToken cancellationToken = default);
    Task UpdateMarriageAsync(int marriageRecordId, UpdateMarriageRequestDto request, CancellationToken cancellationToken = default);
    Task DeleteMarriageAsync(int marriageRecordId, CancellationToken cancellationToken = default);
}
