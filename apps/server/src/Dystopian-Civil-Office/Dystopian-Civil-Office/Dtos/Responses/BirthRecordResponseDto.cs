namespace Dystopian_Civil_Office.Dtos.Responses;

public class BirthRecordResponseDto
{
    public string RegistryNumber { get; set; } = string.Empty;
    public DateOnly RegistryDate { get; set; }
    public string BornPersonPesel { get; set; } = string.Empty;
    public string? MotherPesel { get; set; }
    public string? FatherPesel { get; set; }
    public DateOnly BirthDate { get; set; }
    public string BirthPlace { get; set; } = string.Empty;
    public string? DocumentName { get; set; }
}