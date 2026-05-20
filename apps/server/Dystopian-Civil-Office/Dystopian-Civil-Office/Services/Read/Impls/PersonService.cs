using Dystopian_Civil_Office.Dtos.Responses;
using Dystopian_Civil_Office.Services.Read.Interfaces;

namespace Dystopian_Civil_Office.Services.Read.Impls;

public class PersonService : IPersonService
{
    private readonly OfficeQueryService _officeQueryService;

    public PersonService(OfficeQueryService officeQueryService)
    {
        _officeQueryService = officeQueryService;
    }

    public async Task<IEnumerable<PersonResponseDto>> GetPersonsAsync()
    {
        return await _officeQueryService.GetPersonsAsync();
    }
}
