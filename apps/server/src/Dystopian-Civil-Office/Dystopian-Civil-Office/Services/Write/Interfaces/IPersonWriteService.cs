using Dystopian_Civil_Office.Dtos.Requests.Create;
using Dystopian_Civil_Office.Dtos.Requests.Update;

namespace Dystopian_Civil_Office.Services.Write.Interfaces;

public interface IPersonWriteService
{
    Task CreatePersonAsync(CreatePersonRequestDto request, CancellationToken cancellationToken = default);
    Task UpdatePersonAsync(int personId, UpdatePersonRequestDto request, CancellationToken cancellationToken = default);
    Task DeletePersonAsync(int personId, CancellationToken cancellationToken = default);
}
