namespace Dystopian_Civil_Office.Dtos.Requests.Create;

public class CreateDocumentRequestDto
{
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public DateTimeOffset ImportDate { get; set; }
}