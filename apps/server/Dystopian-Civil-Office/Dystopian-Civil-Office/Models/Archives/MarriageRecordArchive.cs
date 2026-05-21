namespace Dystopian_Civil_Office.Models.Archives;

public class MarriageRecordArchive
{
    public int MarriageRecordArchiveId { get; set; }

    public string RegistryNumber { get; set; } = string.Empty;
    public DateOnly RegistryDate { get; set; }
    public string Spouse1Pesel { get; set; } = string.Empty;
    public string Spouse2Pesel { get; set; } = string.Empty;
    public DateOnly MarriageDate { get; set; }
    public string MarriagePlace { get; set; } = string.Empty;
    public string? DocumentName { get; set; }

    public DateTime DeletedAt { get; set; }
}