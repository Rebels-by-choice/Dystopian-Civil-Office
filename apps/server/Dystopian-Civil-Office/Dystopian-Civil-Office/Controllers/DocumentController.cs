using Dystopian_Civil_Office.Dtos.Requests.Create;
using Dystopian_Civil_Office.Dtos.Requests.Update;
using Dystopian_Civil_Office.Dtos.Responses;
using Dystopian_Civil_Office.Services.Read.Interfaces;
using Dystopian_Civil_Office.Services.Write.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using Npgsql.PostgresTypes;

namespace Dystopian_Civil_Office.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DocumentController : ControllerBase
{
    private readonly IDocumentReadService _documentReadService;
    private readonly IDocumentWriteService _documentWriteService;

    public DocumentController(
        IDocumentReadService documentReadService,
        IDocumentWriteService documentWriteService)
    {
        _documentReadService = documentReadService;
        _documentWriteService = documentWriteService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DocumentResponseDto>>> GetAsync()
    {
        var documents = await _documentReadService.GetDocumentsAsync();
        return Ok(documents);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreateDocumentRequestDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            await _documentWriteService.CreateDocumentAsync(request, cancellationToken);
            return StatusCode(StatusCodes.Status201Created);
        }
        catch (PostgresException ex) when (ex.SqlState == "23505")
        {
            return Conflict(new { message = ex.MessageText });
        }
        catch (PostgresException ex)
        {
            return BadRequest(new { message = ex.MessageText });
        }
    }

    [HttpPut("{documentId:int}")]
    public async Task<IActionResult> UpdateAsync(
        int documentId,
        [FromBody] UpdateDocumentRequestDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            await _documentWriteService.UpdateDocumentAsync(documentId, request, cancellationToken);
            return NoContent();
        }
        catch (PostgresException ex) when (ex.MessageText.Contains("not found", StringComparison.OrdinalIgnoreCase))
        {
            return NotFound(new { message = ex.MessageText });
        }
        catch (PostgresException ex) when (ex.SqlState == "23505")
        {
            return Conflict(new { message = ex.MessageText });
        }
        catch (PostgresException ex)
        {
            return BadRequest(new { message = ex.MessageText });
        }
    }

    [HttpDelete("{documentId:int}")]
    public async Task<IActionResult> DeleteAsync(
        int documentId,
        CancellationToken cancellationToken)
    {
        try
        {
            await _documentWriteService.DeleteDocumentAsync(documentId, cancellationToken);
            return NoContent();
        }
        catch (PostgresException ex) when (ex.MessageText.Contains("not found", StringComparison.OrdinalIgnoreCase))
        {
            return NotFound(new { message = ex.MessageText });
        }
        catch (PostgresException ex) when (ex.SqlState == "23503")
        {
            return Conflict(new { message = ex.MessageText });
        }
        catch (PostgresException ex)
        {
            return BadRequest(new { message = ex.MessageText });
        }
    }
}