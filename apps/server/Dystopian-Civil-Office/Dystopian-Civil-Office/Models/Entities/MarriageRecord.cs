using System.ComponentModel.DataAnnotations;

namespace Dystopian_Civil_Office.Models.Entities;

public class MarriageRecord
{
    public int MarriageRecordId { get; set; }

    [Required]
    [MaxLength(50)]
    public string RegistryNumber { get; set; } = string.Empty;

    public int Spouse1Id { get; set; }
    public int Spouse2Id { get; set; }

    public DateOnly MarriageDate { get; set; }

    [Required]
    [MaxLength(100)]
    public string MarriagePlace { get; set; } = string.Empty;

    public DateOnly RegistryDate { get; set; }

    public Person Spouse1 { get; set; } = null!;
    public Person Spouse2 { get; set; } = null!;
}