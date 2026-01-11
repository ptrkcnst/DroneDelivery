using DroneDelivery.WebApi.Data;
using DroneDelivery.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DroneDelivery.WebApi.Controllers;

[ApiController]
[Route("api/addresses")]
public class AddressesController : ControllerBase
{
    private readonly AppDbContext _db;

    public AddressesController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<List<Address>>> Get()
    {
        var items = await _db.Addresses.OrderByDescending(a => a.Id).ToListAsync();
        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Address>> GetById(int id)
    {
        var item = await _db.Addresses.FindAsync(id);
        if (item is null) return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<Address>> Create([FromBody] Address dto)
    {
        dto.Id = 0;
        dto.Label ??= string.Empty;
        dto.Street ??= string.Empty;
        dto.City ??= string.Empty;
        dto.Country ??= string.Empty;
        dto.CreatedUtc = DateTime.UtcNow;
        _db.Addresses.Add(dto);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Address dto)
    {
        var item = await _db.Addresses.FindAsync(id);
        if (item is null) return NotFound();

        item.Label = dto.Label ?? string.Empty;
        item.Street = dto.Street ?? string.Empty;
        item.City = dto.City ?? string.Empty;
        item.Country = dto.Country ?? string.Empty;
        item.Latitude = dto.Latitude;
        item.Longitude = dto.Longitude;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _db.Addresses.FindAsync(id);
        if (item is null) return NotFound();
        _db.Addresses.Remove(item);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
