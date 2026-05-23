namespace Dystopian_Civil_Office.Dtos.Requests.Update;

public class UpdateDocumentRequestDto
{
    public string? Name { get; set; }
    public string? Category { get; set; }
    public DateTimeOffset? ImportDate { get; set; }
}