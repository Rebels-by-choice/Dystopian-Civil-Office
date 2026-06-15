using Dystopian_Civil_Office.Dtos.Requests.Create;

namespace Dystopian_Civil_Office.Services.Write.Interfaces;

public interface ICaseService
{
    Task CreateAsync(CreateCaseRequestDto request);
    Task UpdateStatusAsync(UpdateCaseRequestDto request);
}