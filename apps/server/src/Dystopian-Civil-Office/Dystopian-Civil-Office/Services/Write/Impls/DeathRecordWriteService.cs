using Dystopian_Civil_Office.DataSource;
using Dystopian_Civil_Office.Dtos.Requests.Create;
using Dystopian_Civil_Office.Dtos.Requests.Update;
using Dystopian_Civil_Office.Services.Write.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Dystopian_Civil_Office.Services.Write.Impls;

public class DeathRecordWriteService : IDeathRecordWriteService
{
    private readonly ApplicationDbContext _dbContext;

    public DeathRecordWriteService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateDeathRecordAsync(CreateDeathRecordRequestDto request, CancellationToken cancellationToken = default)
    {
        var parameters = new[]
        {
            new NpgsqlParameter("p_registry_number", request.RegistryNumber),
            new NpgsqlParameter("p_person_pesel", request.PersonPesel),
            new NpgsqlParameter("p_death_date", request.DeathDate),
            new NpgsqlParameter("p_death_place", request.DeathPlace),
            new NpgsqlParameter("p_registry_date", request.RegistryDate),
            new NpgsqlParameter("p_cause_of_death", request.CauseOfDeath),
            new NpgsqlParameter("p_document_name", (object?)request.DocumentName ?? DBNull.Value)
        };

        await _dbContext.Database.ExecuteSqlRawAsync(
            "CALL public.create_death_record(@p_registry_number, @p_person_pesel, @p_death_date, @p_death_place, @p_registry_date, @p_cause_of_death, @p_document_name)",
            parameters,
            cancellationToken);
    }

    public async Task UpdateDeathRecordAsync(int deathRecordId, UpdateDeathRecordRequestDto request, CancellationToken cancellationToken = default)
    {
        var parameters = new[]
        {
            new NpgsqlParameter("p_death_record_id", deathRecordId),
            new NpgsqlParameter("p_registry_number", (object?)request.RegistryNumber ?? DBNull.Value),
            new NpgsqlParameter("p_person_pesel", (object?)request.PersonPesel ?? DBNull.Value),
            new NpgsqlParameter("p_death_date", (object?)request.DeathDate ?? DBNull.Value),
            new NpgsqlParameter("p_death_place", (object?)request.DeathPlace ?? DBNull.Value),
            new NpgsqlParameter("p_registry_date", (object?)request.RegistryDate ?? DBNull.Value),
            new NpgsqlParameter("p_cause_of_death", (object?)request.CauseOfDeath ?? DBNull.Value),
            new NpgsqlParameter("p_document_name", (object?)request.DocumentName ?? DBNull.Value)
        };

        await _dbContext.Database.ExecuteSqlRawAsync(
            "CALL public.update_death_record(@p_death_record_id, @p_registry_number, @p_person_pesel, @p_death_date, @p_death_place, @p_registry_date, @p_cause_of_death, @p_document_name)",
            parameters,
            cancellationToken);
    }

    public async Task DeleteDeathRecordAsync(int deathRecordId, CancellationToken cancellationToken = default)
    {
        var parameters = new[]
        {
            new NpgsqlParameter("p_death_record_id", deathRecordId)
        };

        await _dbContext.Database.ExecuteSqlRawAsync(
            "CALL public.delete_death_record(@p_death_record_id)",
            parameters,
            cancellationToken);
    }
}
