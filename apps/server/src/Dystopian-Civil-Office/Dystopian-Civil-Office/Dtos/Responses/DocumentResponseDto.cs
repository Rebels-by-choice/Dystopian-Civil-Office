namespace Dystopian_Civil_Office.Dtos.Responses;

public class DocumentResponseDto
{
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public DateTime ImportDate { get; set; }
}