using Dystopian_Civil_Office.Dtos.Responses;

namespace Dystopian_Civil_Office.Services.Read.Interfaces;

public interface IMarriageReadService
{
    Task<IEnumerable<MarriageResponseDto>> GetMarriagesAsync();
}