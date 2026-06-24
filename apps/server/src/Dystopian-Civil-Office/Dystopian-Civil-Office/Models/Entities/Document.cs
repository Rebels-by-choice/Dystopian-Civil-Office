using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dystopian_Civil_Office.Models.Entities;

public class Document
{
    public int DocumentId { get; set; }
    public int PaperlessDocumentId { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Category { get; set; } = string.Empty;

    [Required]
    public DateTime ImportDate { get; set; } = DateTime.Now;
    
    public required Case Case { get; set; }
    
    public int DocumentIssuerId { get; set; }
    [ForeignKey(nameof(DocumentIssuerId))]
    public required Person DocumentIssuer { get; set; }
    
    public Person? Person { get; set; }
    public Address? Address { get; set; }
    public BirthRecord? BirthRecord { get; set; }
    public DeathRecord? DeathRecord { get; set; }
    public MarriageRecord? MarriageRecord { get; set; }
}