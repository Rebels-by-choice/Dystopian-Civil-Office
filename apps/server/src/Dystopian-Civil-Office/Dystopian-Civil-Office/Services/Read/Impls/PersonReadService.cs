using Dystopian_Civil_Office.Dtos.Responses;
using Dystopian_Civil_Office.Services.Read.Interfaces;

namespace Dystopian_Civil_Office.Services.Read.Impls;

public class PersonReadService : IPersonReadService
{
    private readonly OfficeQueryService _officeQueryService;

    public PersonReadService(OfficeQueryService officeQueryService)
    {
        _officeQueryService = officeQueryService;
    }

    public async Task<IEnumerable<PersonResponseDto>> GetPersonsAsync()
    {
        return await _officeQueryService.GetPersonsAsync();
    }
}
