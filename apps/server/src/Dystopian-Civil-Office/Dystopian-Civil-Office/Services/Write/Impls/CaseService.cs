using Dystopian_Civil_Office.DataSource;
using Dystopian_Civil_Office.Dtos.Requests.Create;
using Dystopian_Civil_Office.Dtos.Requests.Update;
using Dystopian_Civil_Office.Dtos.Responses;
using Dystopian_Civil_Office.Services.Write.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Dystopian_Civil_Office.Services.Write.Impls;

public class CaseService(ApplicationDbContext dbContext) : ICaseService
{
    public async Task<IEnumerable<CaseResponseDto>> GetAllAsync()
    {
        var casesVar = await dbContext.Cases
            .AsNoTracking()
            .Select(c => new CaseResponseDto
            {
                CaseId = c.CaseId,
                Status = c.Status,
                CreatedAt = c.CreatedAt,
                InitiatorId = c.InitiatorId,
                ResponderId = c.ResponderId,
                ClosedAt = c.ClosedAt
            }).ToListAsync();
        
        return casesVar;
    }

    public async Task<CaseResponseDto> GetAsync(int caseId)
    {
        var caseVar = await dbContext.Cases
            .AsNoTracking()
            .Select(c => new CaseResponseDto
            {
                CaseId = c.CaseId,
                Status = c.Status,
                CreatedAt = c.CreatedAt,
                InitiatorId = c.InitiatorId,
                ResponderId = c.ResponderId,
                ClosedAt = c.ClosedAt
            })
            .Where(c => c.CaseId == caseId)
            .FirstOrDefaultAsync();
        
        if (caseVar == null) throw new KeyNotFoundException("Case not found");
        return caseVar;
    }

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
            new NpgsqlParameter("p_new_status", (int)request.NewStatus),
            new NpgsqlParameter("p_party_id", request.PartyId),
        };
        
        return dbContext.Database.ExecuteSqlRawAsync(
            "CALL public.update_case(@p_case_id, @p_new_status, @p_party_id)",
            parameters,
            CancellationToken.None);
    }
}