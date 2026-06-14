namespace Dystopian_Civil_Office.Dtos.Requests.Update;

public class UpdateDeathRecordRequestDto
{
    public string? RegistryNumber { get; set; }
    public int PersonPesel { get; set; }
    public DateOnly? DeathDate { get; set; }
    public string? DeathPlace { get; set; }
    public DateOnly? RegistryDate { get; set; }
    public string? CauseOfDeath { get; set; }
    public string? DocumentName { get; set; }
}
