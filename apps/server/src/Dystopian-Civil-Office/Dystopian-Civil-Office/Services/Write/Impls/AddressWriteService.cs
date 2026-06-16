using Dystopian_Civil_Office.DataSource;
using Dystopian_Civil_Office.Dtos.Requests.Create;
using Dystopian_Civil_Office.Dtos.Requests.Update;
using Dystopian_Civil_Office.Services.Write.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Dystopian_Civil_Office.Services.Write.Impls;

public class AddressWriteService : IAddressWriteService
{
    private readonly ApplicationDbContext _dbContext;

    public AddressWriteService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateAddressAsync(CreateAddressRequestDto request, CancellationToken cancellationToken = default)
    {
        var parameters = new[]
        {
            new NpgsqlParameter("p_registry_number", (object?)request.RegistryNumber ?? DBNull.Value),
            new NpgsqlParameter("p_city", request.City),
            new NpgsqlParameter("p_street", request.Street),
            new NpgsqlParameter("p_house_number", request.HouseNumber),
            new NpgsqlParameter("p_apartment_number", (object?)request.ApartmentNumber ?? DBNull.Value),
            new NpgsqlParameter("p_postal_code", request.PostalCode),
            new NpgsqlParameter("p_country", request.Country),
            new NpgsqlParameter("p_document_name", (object?)request.DocumentName ?? DBNull.Value)
        };

        await _dbContext.Database.ExecuteSqlRawAsync(
            "CALL public.create_address(@p_registry_number, @p_city, @p_street, @p_house_number, @p_apartment_number, @p_postal_code, @p_country, @p_document_name)",
            parameters,
            cancellationToken);
    }

    public async Task UpdateAddressAsync(int addressId, UpdateAddressRequestDto request, CancellationToken cancellationToken = default)
    {
        var parameters = new[]
        {
            new NpgsqlParameter("p_address_id", addressId),
            new NpgsqlParameter("p_registry_number", (object?)request.RegistryNumber ?? DBNull.Value),
            new NpgsqlParameter("p_city", (object?)request.City ?? DBNull.Value),
            new NpgsqlParameter("p_street", (object?)request.Street ?? DBNull.Value),
            new NpgsqlParameter("p_house_number", (object?)request.HouseNumber ?? DBNull.Value),
            new NpgsqlParameter("p_apartment_number", (object?)request.ApartmentNumber ?? DBNull.Value),
            new NpgsqlParameter("p_postal_code", (object?)request.PostalCode ?? DBNull.Value),
            new NpgsqlParameter("p_country", (object?)request.Country ?? DBNull.Value),
            new NpgsqlParameter("p_document_name", (object?)request.DocumentName ?? DBNull.Value)
        };

        await _dbContext.Database.ExecuteSqlRawAsync(
            "CALL public.update_address(@p_address_id, @p_registry_number, @p_city, @p_street, @p_house_number, @p_apartment_number, @p_postal_code, @p_country, @p_document_name)",
            parameters,
            cancellationToken);
    }

    public async Task DeleteAddressAsync(int addressId, CancellationToken cancellationToken = default)
    {
        var parameters = new[]
        {
            new NpgsqlParameter("p_address_id", addressId)
        };

        await _dbContext.Database.ExecuteSqlRawAsync(
            "CALL public.delete_address(@p_address_id)",
            parameters,
            cancellationToken);
    }
}
