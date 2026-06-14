namespace Dystopian_Civil_Office.Dtos.Responses;

public class MarriageResponseDto
{
    public int MarriageRecordId { get; set; }
    public string RegistryNumber { get; set; } = string.Empty;
    public DateOnly RegistryDate { get; set; }
    public string Spouse1Pesel { get; set; } = string.Empty;
    public string Spouse2Pesel { get; set; } = string.Empty;
    public DateOnly MarriageDate { get; set; }
    public string MarriagePlace { get; set; } = string.Empty;
    public string? DocumentName { get; set; }
}