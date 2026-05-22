namespace Dystopian_Civil_Office.Models.Archives;

public class DocumentArchive
{
    public int DocumentArchiveId { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public DateTime ImportDate { get; set; }

    public DateTime DeletedAt { get; set; }
}