using Dystopian_Civil_Office.Models.Enums;

namespace Dystopian_Civil_Office.Dtos.Responses;

public class CaseResponseDto
{
    public int CaseId { get; set; }
    
    public CaseStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? InitiatorId { get; set; }
    public int? ResponderId { get; set; }
    public DateTime? ClosedAt { get; set; }
}