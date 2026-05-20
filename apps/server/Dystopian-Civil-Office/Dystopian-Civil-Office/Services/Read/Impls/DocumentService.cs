using Dystopian_Civil_Office.Dtos.Responses;
using Dystopian_Civil_Office.Services.Read.Interfaces;

namespace Dystopian_Civil_Office.Services.Read.Impls;

public class DocumentService : IDocumentService
{
    private readonly OfficeQueryService _officeQueryService;

    public DocumentService(OfficeQueryService officeQueryService)
    {
        _officeQueryService = officeQueryService;
    }

    public async Task<IEnumerable<DocumentResponseDto>> GetDocumentsAsync()
    {
        return await _officeQueryService.GetDocumentsAsync();
    }
}
