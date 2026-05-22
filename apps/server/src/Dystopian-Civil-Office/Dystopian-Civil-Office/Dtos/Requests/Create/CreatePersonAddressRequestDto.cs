namespace Dystopian_Civil_Office.Dtos.Requests.Create;

public class CreatePersonAddressRequestDto
{
    public string City { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string HouseNumber { get; set; } = string.Empty;
    public string? ApartmentNumber { get; set; }
    public string PostalCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public int? DocumentId { get; set; }
}
