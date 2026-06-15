using Dystopian_Civil_Office.Models.Entities;
using Dystopian_Civil_Office.Models.Enums;

namespace Dystopian_Civil_Office.Dtos.Requests.Create;

public class CreateCaseRequestDto
{
    public CaseStatus Status { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public required int InitiatorId { get; set; }
}