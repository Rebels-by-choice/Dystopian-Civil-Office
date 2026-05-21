using Dystopian_Civil_Office.DataSource;
using Dystopian_Civil_Office.Dtos.Requests.Create;
using Dystopian_Civil_Office.Dtos.Requests.Update;
using Dystopian_Civil_Office.Services.Write.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Dystopian_Civil_Office.Services.Write.Impls;

public class BirthRecordWriteService : IBirthRecordWriteService
{
    private readonly ApplicationDbContext _dbContext;

    public BirthRecordWriteService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateBirthRecordAsync(CreateBirthRecordRequestDto request, CancellationToken cancellationToken = default)
    {
        var parameters = new[]
        {
            new NpgsqlParameter("p_registry_number", request.RegistryNumber),
            new NpgsqlParameter("p_person_id", request.PersonId),
            new NpgsqlParameter("p_mother_id", (object?)request.MotherId ?? DBNull.Value),
            new NpgsqlParameter("p_father_id", (object?)request.FatherId ?? DBNull.Value),
            new NpgsqlParameter("p_registry_date", request.RegistryDate),
            new NpgsqlParameter("p_document_id", (object?)request.DocumentId ?? DBNull.Value)
        };

        await _dbContext.Database.ExecuteSqlRawAsync(
            "CALL public.create_birth_record(@p_registry_number, @p_person_id, @p_mother_id, @p_father_id, @p_registry_date, @p_document_id)",
            parameters,
            cancellationToken);
    }

    public async Task UpdateBirthRecordAsync(int birthRecordId, UpdateBirthRecordRequestDto request, CancellationToken cancellationToken = default)
    {
        var parameters = new[]
        {
            new NpgsqlParameter("p_birth_record_id", birthRecordId),
            new NpgsqlParameter("p_registry_number", (object?)request.RegistryNumber ?? DBNull.Value),
            new NpgsqlParameter("p_person_id", (object?)request.PersonId ?? DBNull.Value),
            new NpgsqlParameter("p_mother_id", (object?)request.MotherId ?? DBNull.Value),
            new NpgsqlParameter("p_father_id", (object?)request.FatherId ?? DBNull.Value),
            new NpgsqlParameter("p_registry_date", (object?)request.RegistryDate ?? DBNull.Value),
            new NpgsqlParameter("p_document_id", (object?)request.DocumentId ?? DBNull.Value)
        };

        await _dbContext.Database.ExecuteSqlRawAsync(
            "CALL public.update_birth_record(@p_birth_record_id, @p_registry_number, @p_person_id, @p_mother_id, @p_father_id, @p_registry_date, @p_document_id)",
            parameters,
            cancellationToken);
    }

    public async Task DeleteBirthRecordAsync(int birthRecordId, CancellationToken cancellationToken = default)
    {
        var parameters = new[]
        {
            new NpgsqlParameter("p_birth_record_id", birthRecordId)
        };

        await _dbContext.Database.ExecuteSqlRawAsync(
            "CALL public.delete_birth_record(@p_birth_record_id)",
            parameters,
            cancellationToken);
    }
}
