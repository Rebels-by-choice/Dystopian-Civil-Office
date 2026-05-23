namespace Dystopian_Civil_Office.Dtos.Requests.Create;

public class CreateMarriageRequestDto
{
    public string RegistryNumber { get; set; } = string.Empty;
    public DateOnly RegistryDate { get; set; }
    public string Spouse1Pesel { get; set; } = string.Empty;
    public string Spouse2Pesel { get; set; } = string.Empty;
    public DateOnly MarriageDate { get; set; }
    public string MarriagePlace { get; set; } = string.Empty;
    public string? DocumentName { get; set; }
}
