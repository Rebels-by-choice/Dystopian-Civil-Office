using Dystopian_Civil_Office.Models.Enums;

namespace Dystopian_Civil_Office.Dtos.Requests.Update;

public class UpdateCaseRequestDto
{
    public int CaseId { get; set; }
    public CaseStatus NewStatus { get; set; }
    public int PartyId { get; set; }
}