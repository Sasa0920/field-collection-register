using FieldCollectionRegister.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FieldCollectionRegister.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FieldsController : ControllerBase
{
    private readonly IFieldService _fieldService;

    public FieldsController(IFieldService fieldService)
    {
        _fieldService = fieldService;
    }

    // GET /api/fields?estateId=1
    [HttpGet]
    public async Task<IActionResult> GetByEstate([FromQuery] int estateId)
    {
        if (estateId <= 0)
        {
            return BadRequest("A valid estateId query parameter is required.");
        }

        var fields = await _fieldService.GetFieldsForEstateAsync(estateId);
        return Ok(fields);
    }
}
