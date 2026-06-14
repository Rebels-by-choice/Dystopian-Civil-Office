using System.ComponentModel.DataAnnotations;

namespace Dystopian_Civil_Office.Models.Entities;

public class Person
{
    public int PersonId { get; set; }
    public bool? IsFunctionary { get; set; }

    [Required]
    [StringLength(11)]
    public string Pesel { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? MiddleName { get; set; }

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Gender { get; set; } = string.Empty;

    public DateOnly BirthDate { get; set; }

    [Required]
    [MaxLength(100)]
    public string BirthPlace { get; set; } = string.Empty;

    public int AddressId { get; set; }
    public Address Address { get; set; } = null!;

    public int? DocumentId { get; set; }
    public Document? Document { get; set; }

    public BirthRecord? BirthRecord { get; set; }
    public DeathRecord? DeathRecord { get; set; }

    public ICollection<BirthRecord> BirthRecordsAsMother { get; set; } = new List<BirthRecord>();
    public ICollection<BirthRecord> BirthRecordsAsFather { get; set; } = new List<BirthRecord>();

    public ICollection<MarriageRecord> MarriagesAsSpouse1 { get; set; } = new List<MarriageRecord>();
    public ICollection<MarriageRecord> MarriagesAsSpouse2 { get; set; } = new List<MarriageRecord>();
}