namespace Dystopian_Civil_Office.Dtos.Responses;

public class DeathRecordResponseDto
{
    public string RegistryNumber { get; set; } = string.Empty;
    public DateOnly RegistryDate { get; set; }
    public string PersonPesel { get; set; } = string.Empty;
    public DateOnly DeathDate { get; set; }
    public string DeathPlace { get; set; } = string.Empty;
    public string CauseOfDeath { get; set; } = string.Empty;
    public string? DocumentName { get; set; }
}