namespace Dystopian_Civil_Office.ViewModels;

public class ApiOperationLog
{
    public int Id { get; set; }
    public string Method { get; set; } = string.Empty; // POST, PUT and DELETE
    public string Path { get; set; } = string.Empty; // ex. /api/documents
    public int StatusCode { get; set; }
    public DateTime ExecutedAt { get; set; }
}