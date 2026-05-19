using System.ComponentModel.DataAnnotations;

namespace Dystopian_Civil_Office.Models.Entities;

public class Address
{
    public int AddressId { get; set; }

    [Required]
    [MaxLength(100)]
    public string City { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Street { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string HouseNumber { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? ApartmentNumber { get; set; }

    [Required]
    [MaxLength(15)]
    public string PostalCode { get; set; } = string.Empty;

    [Required]
    [MaxLength(60)]
    public string Country { get; set; } = string.Empty;

    public ICollection<Person> Persons { get; set; } = new List<Person>();
}