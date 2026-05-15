using System.ComponentModel.DataAnnotations;

namespace Dystopian_Civil_Office.Models.Entities;

public class BirthRecord
{
    public int BirthRecordId { get; set; }

    [Required]
    [MaxLength(50)]
    public string RegistryNumber { get; set; } = string.Empty;

    public int PersonId { get; set; }
    public int? MotherId { get; set; }
    public int? FatherId { get; set; }

    public DateOnly RegistryDate { get; set; }

    public Person Person { get; set; } = null!;
    public Person? Mother { get; set; }
    public Person? Father { get; set; }
}