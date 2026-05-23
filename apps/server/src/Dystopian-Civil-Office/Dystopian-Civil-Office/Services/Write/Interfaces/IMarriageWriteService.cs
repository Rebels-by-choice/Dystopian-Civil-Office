using Dystopian_Civil_Office.Dtos.Requests.Create;
using Dystopian_Civil_Office.Dtos.Requests.Update;

namespace Dystopian_Civil_Office.Services.Write.Interfaces;

public interface IMarriageWriteService
{
    Task CreateMarriageAsync(CreateMarriageRequestDto request, CancellationToken cancellationToken = default);
    Task UpdateMarriageAsync(string registryNumber, UpdateMarriageRequestDto request, CancellationToken cancellationToken = default);
    Task DeleteMarriageAsync(string registryNumber, CancellationToken cancellationToken = default);
}
