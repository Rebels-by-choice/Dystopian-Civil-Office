namespace Dystopian_Civil_Office.Dtos.Responses;

public class PersonResponseDto
{
    public int PersonId { get; set; }
    public string PersonPesel { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public DateOnly BirthDate { get; set; }
    public string BirthPlace { get; set; } = string.Empty;
    public string? AddressRegistryNumber { get; set; }
    public string? DocumentName { get; set; }
    public bool? IsFunctionary { get; set; }
}