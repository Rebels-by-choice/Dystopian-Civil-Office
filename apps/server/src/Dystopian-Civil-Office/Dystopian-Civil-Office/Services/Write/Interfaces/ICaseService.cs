using Dystopian_Civil_Office.Dtos.Requests.Create;
using Dystopian_Civil_Office.Dtos.Requests.Update;
using Dystopian_Civil_Office.Dtos.Responses;

namespace Dystopian_Civil_Office.Services.Write.Interfaces;

public interface ICaseService
{
    Task<IEnumerable<CaseResponseDto>> GetAllAsync();
    Task<CaseResponseDto> GetAsync(int caseId);
    Task CreateAsync(CreateCaseRequestDto request);
    Task UpdateStatusAsync(UpdateCaseRequestDto request);
}