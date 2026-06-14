using Dystopian_Civil_Office.Services.Read.Interfaces;
using Dystopian_Civil_Office.Services.Write.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Dystopian_Civil_Office.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CaseController(
    ICaseService caseService
)
{
    // [HttpPost]
    // public async Task<IActionResult> CreateAsync()
    // {
    //     
    // }
}