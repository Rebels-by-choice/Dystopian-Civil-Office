using Dystopian_Civil_Office.Dtos.Responses;

namespace Dystopian_Civil_Office.Services.Read.Interfaces;

public interface IDocumentReadService
{
    Task<IEnumerable<DocumentResponseDto>> GetDocumentsAsync();
    Task<IEnumerable<DocumentResponseDto>> GetDocumentsByCategoryAsync(string category);
}