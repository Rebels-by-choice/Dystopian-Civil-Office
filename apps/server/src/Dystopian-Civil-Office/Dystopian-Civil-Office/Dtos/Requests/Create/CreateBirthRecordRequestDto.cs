namespace Dystopian_Civil_Office.Dtos.Requests.Create;

public class CreateBirthRecordRequestDto
{
    public string RegistryNumber { get; set; } = string.Empty;
    public int PersonId { get; set; }
    public int? MotherId { get; set; }
    public int? FatherId { get; set; }
    public DateOnly RegistryDate { get; set; }
    public int? DocumentId { get; set; }
}
