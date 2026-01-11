using DroneDelivery.WebApi.Data;
using DroneDelivery.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DroneDelivery.WebApi.Controllers;

[ApiController]
[Route("api/notifications")]
public class NotificationsController : ControllerBase
{
    private readonly AppDbContext _db;

    public NotificationsController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<List<Notification>>> Get()
    {
        var items = await _db.Notifications.OrderByDescending(n => n.Id).ToListAsync();
        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Notification>> GetById(int id)
    {
        var item = await _db.Notifications.FindAsync(id);
        if (item is null) return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<Notification>> Create([FromBody] Notification dto)
    {
        dto.Id = 0;
        dto.CreatedUtc = DateTime.UtcNow;
        _db.Notifications.Add(dto);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Notification dto)
    {
        var item = await _db.Notifications.FindAsync(id);
        if (item is null) return NotFound();

        item.Title = dto.Title;
        item.Message = dto.Message;
        item.Read = dto.Read;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _db.Notifications.FindAsync(id);
        if (item is null) return NotFound();
        _db.Notifications.Remove(item);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
