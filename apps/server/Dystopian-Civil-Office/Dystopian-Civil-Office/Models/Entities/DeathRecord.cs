using System.ComponentModel.DataAnnotations;

namespace Dystopian_Civil_Office.Models.Entities;

public class DeathRecord
{
    public int DeathRecordId { get; set; }

    [Required]
    [MaxLength(50)]
    public string RegistryNumber { get; set; } = string.Empty;

    public int PersonId { get; set; }

    public DateOnly DeathDate { get; set; }

    [Required]
    [MaxLength(100)]
    public string DeathPlace { get; set; } = string.Empty;

    public DateOnly RegistryDate { get; set; }

    [Required]
    [MaxLength(200)]
    public string CauseOfDeath { get; set; } = string.Empty;

    public Person Person { get; set; } = null!;
}