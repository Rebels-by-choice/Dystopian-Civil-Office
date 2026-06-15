using Dystopian_Civil_Office.DataSource;
using Dystopian_Civil_Office.Dtos.Requests.Create;
using Dystopian_Civil_Office.Dtos.Requests.Update;
using Dystopian_Civil_Office.Services.Write.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Dystopian_Civil_Office.Services.Write.Impls;

public class CaseService(ApplicationDbContext dbContext) : ICaseService
{
    public async Task CreateAsync(CreateCaseRequestDto request)
    {
        var parameters = new[]
        {
            new NpgsqlParameter("p_initiator_id", request.InitiatorId),
        };

        await dbContext.Database.ExecuteSqlRawAsync(
            "CALL public.create_case(@p_initiator_id)",
            parameters,
            CancellationToken.None);
    }

    public Task UpdateStatusAsync(UpdateCaseRequestDto request)
    {
        var parameters = new[]
        {
            new NpgsqlParameter("p_case_id", request.CaseId),
            new NpgsqlParameter("p_new_status", request.NewStatus),
            new NpgsqlParameter("p_responder_id", request.ResponderId),
        };
        
        return dbContext.Database.ExecuteSqlRawAsync(
            "CALL public.update_case(@p_case_id, @p_new_status, @p_responder_id)",
            parameters,
            CancellationToken.None);
    }
}