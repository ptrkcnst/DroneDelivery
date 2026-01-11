using DroneDelivery.WebApi.Data;
using DroneDelivery.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DroneDelivery.WebApi.Controllers;

[ApiController]
[Route("api/notification-rules")]
public class NotificationRulesController : ControllerBase
{
    private readonly AppDbContext _db;

    public NotificationRulesController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<List<NotificationRule>>> Get()
    {
        var items = await _db.NotificationRules.OrderByDescending(r => r.Id).ToListAsync();
        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<NotificationRule>> GetById(int id)
    {
        var item = await _db.NotificationRules.FindAsync(id);
        if (item is null) return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<NotificationRule>> Create([FromBody] NotificationRule dto)
    {
        dto.Id = 0;
        dto.CreatedUtc = DateTime.UtcNow;
        _db.NotificationRules.Add(dto);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] NotificationRule dto)
    {
        var item = await _db.NotificationRules.FindAsync(id);
        if (item is null) return NotFound();

        item.Name = dto.Name;
        item.Enabled = dto.Enabled;
        item.Condition = dto.Condition;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _db.NotificationRules.FindAsync(id);
        if (item is null) return NotFound();
        _db.NotificationRules.Remove(item);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
