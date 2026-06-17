namespace Dystopian_Civil_Office.Dtos.Responses;

public class DocumentResponseDto
{
    public int DocumentId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public DateTime ImportDate { get; set; }
    
    public int CaseId { get; set; }
    public int PaperlessDocumentId { get; set; }
    public int DocumentIssuerId { get; set; }
}