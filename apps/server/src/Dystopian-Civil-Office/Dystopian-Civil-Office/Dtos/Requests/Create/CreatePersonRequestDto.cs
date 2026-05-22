namespace Dystopian_Civil_Office.Dtos.Requests.Create;

public class CreatePersonRequestDto
{
    public string Pesel { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public DateOnly BirthDate { get; set; }
    public string BirthPlace { get; set; } = string.Empty;
    public int AddressId { get; set; }
    public int? DocumentId { get; set; }
}
