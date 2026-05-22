namespace Dystopian_Civil_Office.Dtos.Requests.Create;

public class CreateDeathRecordRequestDto
{
    public string RegistryNumber { get; set; } = string.Empty;
    public int PersonId { get; set; }
    public DateOnly DeathDate { get; set; }
    public string DeathPlace { get; set; } = string.Empty;
    public DateOnly RegistryDate { get; set; }
    public string CauseOfDeath { get; set; } = string.Empty;
    public int? DocumentId { get; set; }
}
