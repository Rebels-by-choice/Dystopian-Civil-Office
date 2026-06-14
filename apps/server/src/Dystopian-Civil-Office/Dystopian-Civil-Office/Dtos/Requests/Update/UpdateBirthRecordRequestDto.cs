namespace Dystopian_Civil_Office.Dtos.Requests.Update;

public class UpdateBirthRecordRequestDto
{
    public string? RegistryNumber { get; set; }
    public string PersonPesel { get; set; } = string.Empty;
    public string MotherPesel { get; set; } = string.Empty;
    public string FatherPesel { get; set; } = string.Empty;
    public DateOnly? RegistryDate { get; set; }
    public string? DocumentName { get; set; }
}
