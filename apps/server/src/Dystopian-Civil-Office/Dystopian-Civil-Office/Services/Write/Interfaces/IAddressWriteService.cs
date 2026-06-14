using Dystopian_Civil_Office.Dtos.Requests.Create;
using Dystopian_Civil_Office.Dtos.Requests.Update;

namespace Dystopian_Civil_Office.Services.Write.Interfaces;

public interface IAddressWriteService
{
    Task CreateAddressAsync(CreateAddressRequestDto request, CancellationToken cancellationToken = default);
    Task UpdateAddressAsync(int addressId, UpdateAddressRequestDto request, CancellationToken cancellationToken = default);
    Task DeleteAddressAsync(int addressId, CancellationToken cancellationToken = default);
}
