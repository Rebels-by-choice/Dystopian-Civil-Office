namespace Dystopian_Civil_Office.Dtos.Requests.Update;

public class UpdateBirthRecordRequestDto
{
    public string? RegistryNumber { get; set; }
    public int? PersonId { get; set; }
    public int? MotherId { get; set; }
    public int? FatherId { get; set; }
    public DateOnly? RegistryDate { get; set; }
    public int? DocumentId { get; set; }
}
