namespace Dystopian_Civil_Office.Dtos.Responses;

public class AddressResponseDto
{
    public int AddressId { get; set; }
    public string RegistryNumber { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string HouseNumber { get; set; } = string.Empty;
    public string? ApartmentNumber { get; set; }
    public string PostalCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string? DocumentName { get; set; }
}