using FieldCollectionRegister.Core.Dtos;
using FieldCollectionRegister.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FieldCollectionRegister.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EntriesController : ControllerBase
{
    private readonly IEntryService _entryService;

    public EntriesController(IEntryService entryService)
    {
        _entryService = entryService;
    }

    // GET /api/entries?regionId=&estateId=&fieldId=  (all optional)
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int? regionId, [FromQuery] int? estateId, [FromQuery] int? fieldId)
    {
        var filter = new EntryFilter
        {
            RegionId = regionId,
            EstateId = estateId,
            FieldId = fieldId
        };

        var entries = await _entryService.GetEntriesAsync(filter);
        return Ok(entries);
    }

    // POST /api/entries
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEntryRequest request)
    {
        var result = await _entryService.CreateEntryAsync(request);

        if (!result.Success)
        {
            // Validation failure - the client sent something we won't accept,
            // regardless of what the frontend form already checked.
            return BadRequest(new { error = result.Error });
        }

        return CreatedAtAction(nameof(Get), new { }, result.Value);
    }
}
