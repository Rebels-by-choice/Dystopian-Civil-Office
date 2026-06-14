using System.ComponentModel.DataAnnotations;
using Dystopian_Civil_Office.Models.Enums;

namespace Dystopian_Civil_Office.Models.Entities;

public class Case
{
    public int CaseId { get; set; }
    public CaseStatus Status { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime? ClosedAt { get; set; }
    
    public int InitiatorId { get; set; }
    public required Person Initiator { get; set; }
    
    public int? ResponderId { get; set; }
    public Person? Responder { get; set; }
}