namespace Dystopian_Civil_Office.Dtos.Requests.Create;

public class CreateBirthRecordRequestDto
{
    public string RegistryNumber { get; set; } = string.Empty;
    public string PersonPesel { get; set; } = string.Empty;
    public string MotherPesel { get; set; } = string.Empty;
    public string FatherPesel { get; set; } = string.Empty;
    public DateOnly RegistryDate { get; set; }
    public string? DocumentName { get; set; }
}
