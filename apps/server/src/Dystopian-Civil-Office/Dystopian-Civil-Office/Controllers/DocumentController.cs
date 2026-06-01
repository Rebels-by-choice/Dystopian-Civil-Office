using Dystopian_Civil_Office.Dtos.Requests.Create;
using Dystopian_Civil_Office.Dtos.Requests.Update;
using Dystopian_Civil_Office.Dtos.Responses;
using Dystopian_Civil_Office.Services.Read.Interfaces;
using Dystopian_Civil_Office.Services.Validation;
using Dystopian_Civil_Office.Services.Write.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dystopian_Civil_Office.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DocumentController : ControllerBase
{
    private readonly IDocumentReadService _documentReadService;
    private readonly IDocumentWriteService _documentWriteService;
    private readonly IQueryValidationService _queryValidationService;

    public DocumentController(
        IDocumentReadService documentReadService,
        IDocumentWriteService documentWriteService,
        IQueryValidationService queryValidationService)
    {
        _documentReadService = documentReadService;
        _documentWriteService = documentWriteService;
        _queryValidationService = queryValidationService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DocumentResponseDto>>> GetAsync()
    {
        var documents = await _documentReadService.GetDocumentsAsync();
        return Ok(documents);
    }

    [HttpGet("categories")]
    public async Task<ActionResult<IEnumerable<DocumentResponseDto>>> GetByCategoryAsync(
        [FromQuery] string category)
    {
        _queryValidationService.ValidateCategory(category);

        var documents = await _documentReadService.GetDocumentsByCategoryAsync(category);
        return Ok(documents);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreateDocumentRequestDto request,
        CancellationToken cancellationToken)
    {
        await _documentWriteService.CreateDocumentAsync(request, cancellationToken);
        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpPut("{documentId:int}")]
    public async Task<IActionResult> UpdateAsync(
        int documentId,
        [FromBody] UpdateDocumentRequestDto request,
        CancellationToken cancellationToken)
    {
        await _documentWriteService.UpdateDocumentAsync(documentId, request, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{documentId:int}")]
    public async Task<IActionResult> DeleteAsync(
        int documentId,
        CancellationToken cancellationToken)
    {
        await _documentWriteService.DeleteDocumentAsync(documentId, cancellationToken);
        return NoContent();
    }
}
