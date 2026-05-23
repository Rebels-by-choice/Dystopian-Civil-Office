using Dystopian_Civil_Office.DataSource;
using Dystopian_Civil_Office.Dtos.Requests.Create;
using Dystopian_Civil_Office.Dtos.Requests.Update;
using Dystopian_Civil_Office.Services.Write.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Dystopian_Civil_Office.Services.Write.Impls;

public class DocumentWriteService : IDocumentWriteService
{
    private readonly ApplicationDbContext _dbContext;

    public DocumentWriteService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateDocumentAsync(
        CreateDocumentRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var parameters = new[]
        {
            new NpgsqlParameter("p_name", request.Name),
            new NpgsqlParameter("p_category", request.Category),
            new NpgsqlParameter("p_import_date", request.ImportDate)
        };

        await _dbContext.Database.ExecuteSqlRawAsync(
            "CALL public.create_document(@p_name, @p_category, @p_import_date)",
            parameters,
            cancellationToken);
    }

    public async Task UpdateDocumentAsync(
        int documentId,
        UpdateDocumentRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var parameters = new[]
        {
            new NpgsqlParameter("p_document_id", documentId),
            new NpgsqlParameter("p_name", (object?)request.Name ?? DBNull.Value),
            new NpgsqlParameter("p_category", (object?)request.Category ?? DBNull.Value),
            new NpgsqlParameter("p_import_date", (object?)request.ImportDate ?? DBNull.Value)
        };

        await _dbContext.Database.ExecuteSqlRawAsync(
            "CALL public.update_document(@p_document_id, @p_name, @p_category, @p_import_date)",
            parameters,
            cancellationToken);
    }

    public async Task DeleteDocumentAsync(
        int documentId,
        CancellationToken cancellationToken = default)
    {
        var parameters = new[]
        {
            new NpgsqlParameter("p_document_id", documentId)
        };

        await _dbContext.Database.ExecuteSqlRawAsync(
            "CALL public.delete_document(@p_document_id)",
            parameters,
            cancellationToken);
    }
}