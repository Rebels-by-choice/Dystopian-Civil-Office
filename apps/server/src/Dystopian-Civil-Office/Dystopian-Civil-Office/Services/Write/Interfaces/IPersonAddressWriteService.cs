using Dystopian_Civil_Office.Dtos.Requests.Create;
using Dystopian_Civil_Office.Dtos.Requests.Update;

namespace Dystopian_Civil_Office.Services.Write.Interfaces;

public interface IPersonAddressWriteService
{
    Task CreatePersonAddressAsync(CreatePersonAddressRequestDto request, CancellationToken cancellationToken = default);
    Task UpdatePersonAddressAsync(int addressId, UpdatePersonAddressRequestDto request, CancellationToken cancellationToken = default);
    Task DeletePersonAddressAsync(int addressId, CancellationToken cancellationToken = default);
}
