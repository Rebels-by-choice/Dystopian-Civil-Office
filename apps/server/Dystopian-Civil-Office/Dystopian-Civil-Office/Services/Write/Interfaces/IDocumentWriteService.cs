using Dystopian_Civil_Office.Dtos.Requests.Create;
using Dystopian_Civil_Office.Dtos.Requests.Update;

namespace Dystopian_Civil_Office.Services.Write.Interfaces;

public interface IDocumentWriteService
{
    Task CreateDocumentAsync(CreateDocumentRequestDto request, CancellationToken cancellationToken = default);
    Task UpdateDocumentAsync(int documentId, UpdateDocumentRequestDto request, CancellationToken cancellationToken = default);
    Task DeleteDocumentAsync(int documentId, CancellationToken cancellationToken = default);
}