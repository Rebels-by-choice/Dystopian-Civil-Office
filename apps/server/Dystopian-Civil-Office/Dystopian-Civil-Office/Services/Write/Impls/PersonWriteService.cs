using Dystopian_Civil_Office.DataSource;
using Dystopian_Civil_Office.Dtos.Requests.Create;
using Dystopian_Civil_Office.Dtos.Requests.Update;
using Dystopian_Civil_Office.Services.Write.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Dystopian_Civil_Office.Services.Write.Impls;

public class PersonWriteService : IPersonWriteService
{
    private readonly ApplicationDbContext _dbContext;

    public PersonWriteService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreatePersonAsync(CreatePersonRequestDto request, CancellationToken cancellationToken = default)
    {
        var parameters = new[]
        {
            new NpgsqlParameter("p_pesel", request.Pesel),
            new NpgsqlParameter("p_first_name", request.FirstName),
            new NpgsqlParameter("p_middle_name", (object?)request.MiddleName ?? DBNull.Value),
            new NpgsqlParameter("p_last_name", request.LastName),
            new NpgsqlParameter("p_gender", request.Gender),
            new NpgsqlParameter("p_birth_date", request.BirthDate),
            new NpgsqlParameter("p_birth_place", request.BirthPlace),
            new NpgsqlParameter("p_address_id", request.AddressId),
            new NpgsqlParameter("p_document_id", (object?)request.DocumentId ?? DBNull.Value)
        };

        await _dbContext.Database.ExecuteSqlRawAsync(
            "CALL public.create_person(@p_pesel, @p_first_name, @p_middle_name, @p_last_name, @p_gender, @p_birth_date, @p_birth_place, @p_address_id, @p_document_id)",
            parameters,
            cancellationToken);
    }

    public async Task UpdatePersonAsync(int personId, UpdatePersonRequestDto request, CancellationToken cancellationToken = default)
    {
        var parameters = new[]
        {
            new NpgsqlParameter("p_person_id", personId),
            new NpgsqlParameter("p_pesel", (object?)request.Pesel ?? DBNull.Value),
            new NpgsqlParameter("p_first_name", (object?)request.FirstName ?? DBNull.Value),
            new NpgsqlParameter("p_middle_name", (object?)request.MiddleName ?? DBNull.Value),
            new NpgsqlParameter("p_last_name", (object?)request.LastName ?? DBNull.Value),
            new NpgsqlParameter("p_gender", (object?)request.Gender ?? DBNull.Value),
            new NpgsqlParameter("p_birth_date", (object?)request.BirthDate ?? DBNull.Value),
            new NpgsqlParameter("p_birth_place", (object?)request.BirthPlace ?? DBNull.Value),
            new NpgsqlParameter("p_address_id", (object?)request.AddressId ?? DBNull.Value),
            new NpgsqlParameter("p_document_id", (object?)request.DocumentId ?? DBNull.Value)
        };

        await _dbContext.Database.ExecuteSqlRawAsync(
            "CALL public.update_person(@p_person_id, @p_pesel, @p_first_name, @p_middle_name, @p_last_name, @p_gender, @p_birth_date, @p_birth_place, @p_address_id, @p_document_id)",
            parameters,
            cancellationToken);
    }

    public async Task DeletePersonAsync(int personId, CancellationToken cancellationToken = default)
    {
        var parameters = new[]
        {
            new NpgsqlParameter("p_person_id", personId)
        };

        await _dbContext.Database.ExecuteSqlRawAsync(
            "CALL public.delete_person(@p_person_id)",
            parameters,
            cancellationToken);
    }
}
