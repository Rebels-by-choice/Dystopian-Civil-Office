namespace Dystopian_Civil_Office.Dtos.Requests.Update;

public class UpdatePersonAddressRequestDto
{
    public string? City { get; set; }
    public string? Street { get; set; }
    public string? HouseNumber { get; set; }
    public string? ApartmentNumber { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }
    public int? DocumentId { get; set; }
}
