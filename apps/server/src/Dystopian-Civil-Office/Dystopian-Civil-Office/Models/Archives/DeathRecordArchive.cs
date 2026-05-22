namespace Dystopian_Civil_Office.Models.Archives;

public class DeathRecordArchive
{
    public int DeathRecordArchiveId { get; set; }

    public string RegistryNumber { get; set; } = string.Empty;
    public DateOnly RegistryDate { get; set; }
    public string PersonPesel { get; set; } = string.Empty;
    public DateOnly DeathDate { get; set; }
    public string DeathPlace { get; set; } = string.Empty;
    public string CauseOfDeath { get; set; } = string.Empty;
    public string? DocumentName { get; set; }

    public DateTime DeletedAt { get; set; }
}