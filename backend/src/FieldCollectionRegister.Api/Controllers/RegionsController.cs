using FieldCollectionRegister.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FieldCollectionRegister.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RegionsController : ControllerBase
{
    private readonly IRegionService _regionService;

    public RegionsController(IRegionService regionService)
    {
        _regionService = regionService;
    }

    // GET /api/regions
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var regions = await _regionService.GetAllRegionsAsync();
        return Ok(regions);
    }
}
