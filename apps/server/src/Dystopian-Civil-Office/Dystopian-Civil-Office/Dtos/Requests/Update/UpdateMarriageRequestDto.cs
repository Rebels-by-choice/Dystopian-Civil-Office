namespace Dystopian_Civil_Office.Dtos.Requests.Update;

public class UpdateMarriageRequestDto
{
    public string? RegistryNumber { get; set; }
    public DateOnly? RegistryDate { get; set; }
    public string? Spouse1Pesel { get; set; }
    public string? Spouse2Pesel { get; set; }
    public DateOnly? MarriageDate { get; set; }
    public string? MarriagePlace { get; set; }
    public string? DocumentName { get; set; }
}
