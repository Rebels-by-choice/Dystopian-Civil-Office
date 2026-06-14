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
            new NpgsqlParameter("p_person_pesel", request.PersonPesel),
            new NpgsqlParameter("p_mother_pesel", (object?)request.MotherPesel?? DBNull.Value),
            new NpgsqlParameter("p_father_pesel", (object?)request.FatherPesel ?? DBNull.Value),
            new NpgsqlParameter("p_registry_date", request.RegistryDate),
            new NpgsqlParameter("p_document_name", (object?)request.DocumentName ?? DBNull.Value)
        };

        await _dbContext.Database.ExecuteSqlRawAsync(
            "CALL public.create_birth_record(@p_registry_number, @p_person_pesel, @p_mother_pesel, @p_father_pesel, @p_registry_date, @p_document_name)",
            parameters,
            cancellationToken);
    }

    public async Task UpdateBirthRecordAsync(int birthRecordId, UpdateBirthRecordRequestDto request, CancellationToken cancellationToken = default)
    {
        var parameters = new[]
        {
            new NpgsqlParameter("p_birth_record_id", birthRecordId),
            new NpgsqlParameter("p_registry_number", (object?)request.RegistryNumber ?? DBNull.Value),
            new NpgsqlParameter("p_person_pesel", (object?)request.PersonPesel ?? DBNull.Value),
            new NpgsqlParameter("p_mother_pesel", (object?)request.MotherPesel ?? DBNull.Value),
            new NpgsqlParameter("p_father_pesel", (object?)request.FatherPesel ?? DBNull.Value),
            new NpgsqlParameter("p_registry_date", (object?)request.RegistryDate ?? DBNull.Value),
            new NpgsqlParameter("p_document_name", (object?)request.DocumentName ?? DBNull.Value)
        };

        await _dbContext.Database.ExecuteSqlRawAsync(
            "CALL public.update_birth_record(@p_birth_record_id, @p_registry_number, @p_person_pesel, @p_mother_pesel, @p_father_pesel, @p_registry_date, @p_document_name)",
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
