using Dystopian_Civil_Office.Models.Entities;
using Dystopian_Civil_Office.Models.Enums;

namespace Dystopian_Civil_Office.Dtos.Requests.Create;

public class CreateCaseRequestDto
{
    public required int InitiatorId { get; set; }
}