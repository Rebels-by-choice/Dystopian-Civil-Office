using Dystopian_Civil_Office.DataSource;
using Dystopian_Civil_Office.Dtos.Requests.Create;
using Dystopian_Civil_Office.Dtos.Requests.Update;
using Dystopian_Civil_Office.Services.Write.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Dystopian_Civil_Office.Services.Write.Impls;

public class MarriageWriteService : IMarriageWriteService
{
    private readonly ApplicationDbContext _dbContext;

    public MarriageWriteService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateMarriageAsync(CreateMarriageRequestDto request, CancellationToken cancellationToken = default)
    {
        var parameters = new[]
        {
            new NpgsqlParameter("p_registry_number", request.RegistryNumber),
            new NpgsqlParameter("p_registry_date", request.RegistryDate),
            new NpgsqlParameter("p_spouse1_pesel", request.Spouse1Pesel),
            new NpgsqlParameter("p_spouse2_pesel", request.Spouse2Pesel),
            new NpgsqlParameter("p_marriage_date", request.MarriageDate),
            new NpgsqlParameter("p_marriage_place", request.MarriagePlace),
            new NpgsqlParameter("p_document_name", (object?)request.DocumentName ?? DBNull.Value)
        };

        await _dbContext.Database.ExecuteSqlRawAsync(
            "CALL public.create_marriage_record(@p_registry_number, @p_registry_date, @p_spouse1_pesel, @p_spouse2_pesel, @p_marriage_date, @p_marriage_place, @p_document_name)",
            parameters,
            cancellationToken);
    }

    public async Task UpdateMarriageAsync(string registryNumber, UpdateMarriageRequestDto request, CancellationToken cancellationToken = default)
    {
        var parameters = new[]
        {
            new NpgsqlParameter("p_current_registry_number", registryNumber),
            new NpgsqlParameter("p_new_registry_number", (object?)request.RegistryNumber ?? DBNull.Value),
            new NpgsqlParameter("p_registry_date", (object?)request.RegistryDate ?? DBNull.Value),
            new NpgsqlParameter("p_spouse1_pesel", (object?)request.Spouse1Pesel ?? DBNull.Value),
            new NpgsqlParameter("p_spouse2_pesel", (object?)request.Spouse2Pesel ?? DBNull.Value),
            new NpgsqlParameter("p_marriage_date", (object?)request.MarriageDate ?? DBNull.Value),
            new NpgsqlParameter("p_marriage_place", (object?)request.MarriagePlace ?? DBNull.Value),
            new NpgsqlParameter("p_document_name", (object?)request.DocumentName ?? DBNull.Value)
        };

        await _dbContext.Database.ExecuteSqlRawAsync(
            "CALL public.update_marriage_record(@p_current_registry_number, @p_new_registry_number, @p_registry_date, @p_spouse1_pesel, @p_spouse2_pesel, @p_marriage_date, @p_marriage_place, @p_document_name)",
            parameters,
            cancellationToken);
    }

    public async Task DeleteMarriageAsync(string registryNumber, CancellationToken cancellationToken = default)
    {
        var parameters = new[]
        {
            new NpgsqlParameter("p_registry_number", registryNumber)
        };

        await _dbContext.Database.ExecuteSqlRawAsync(
            "CALL public.delete_marriage_record(@p_registry_number)",
            parameters,
            cancellationToken);
    }
}
