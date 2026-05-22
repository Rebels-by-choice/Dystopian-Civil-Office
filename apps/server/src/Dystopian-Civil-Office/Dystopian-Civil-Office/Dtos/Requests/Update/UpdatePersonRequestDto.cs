namespace Dystopian_Civil_Office.Dtos.Requests.Update;

public class UpdatePersonRequestDto
{
    public string? Pesel { get; set; }
    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
    public string? Gender { get; set; }
    public DateOnly? BirthDate { get; set; }
    public string? BirthPlace { get; set; }
    public int? AddressId { get; set; }
    public int? DocumentId { get; set; }
}
