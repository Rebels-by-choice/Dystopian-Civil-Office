namespace Dystopian_Civil_Office.Models.Archives;

public class BirthRecordArchive
{
    public int BirthRecordArchiveId { get; set; }

    public string RegistryNumber { get; set; } = string.Empty;
    public DateOnly RegistryDate { get; set; }
    public string BornPersonPesel { get; set; } = string.Empty;
    public string? MotherPesel { get; set; }
    public string? FatherPesel { get; set; }
    public DateOnly BirthDate { get; set; }
    public string BirthPlace { get; set; } = string.Empty;
    public string? DocumentName { get; set; }

    public DateTime DeletedAt { get; set; }
}