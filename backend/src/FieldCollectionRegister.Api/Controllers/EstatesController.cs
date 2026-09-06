using FieldCollectionRegister.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FieldCollectionRegister.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EstatesController : ControllerBase
{
    private readonly IEstateService _estateService;

    public EstatesController(IEstateService estateService)
    {
        _estateService = estateService;
    }

    // GET /api/estates?regionId=1
    [HttpGet]
    public async Task<IActionResult> GetByRegion([FromQuery] int regionId)
    {
        if (regionId <= 0)
        {
            return BadRequest("A valid regionId query parameter is required.");
        }

        var estates = await _estateService.GetEstatesForRegionAsync(regionId);
        return Ok(estates);
    }
}
