namespace Dystopian_Civil_Office.Models.Archives;

public class AddressArchive
{
    public int AddressArchiveId { get; set; }

    public string City { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string HouseNumber { get; set; } = string.Empty;
    public string? ApartmentNumber { get; set; }
    public string PostalCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string? DocumentName { get; set; }

    public DateTime DeletedAt { get; set; }
}