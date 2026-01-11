using DroneDelivery.WebApi.Data;
using DroneDelivery.WebApi.Dtos;
using DroneDelivery.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DroneDelivery.WebApi.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly AppDbContext _db;

    public OrdersController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<List<OrderAdminDto>>> Get()
    {
        var items = await _db.Orders
            .OrderByDescending(o => o.Id)
            .Select(o => new OrderAdminDto(
                o.Id,
                o.AddressId,
                o.Email,
                o.CreatedAt,
                o.TotalWeightKg,
                o.Status,
                o.ScheduledAt,
                _db.Addresses.FirstOrDefault(a => a.Id == o.AddressId)
            ))
            .ToListAsync();

        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Order>> GetById(int id)
    {
        var item = await _db.Orders.FindAsync(id);
        if (item is null) return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<Order>> Create([FromBody] Order dto)
    {
        dto.Id = 0;
        dto.CreatedAt = DateTime.UtcNow;
        dto.Email ??= string.Empty;
        _db.Orders.Add(dto);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Order dto)
    {
        var item = await _db.Orders.FindAsync(id);
        if (item is null) return NotFound();

        item.AddressId = dto.AddressId;
        item.TotalWeightKg = dto.TotalWeightKg;
        item.Status = dto.Status;
        item.Email = dto.Email ?? string.Empty;
        item.ScheduledAt = dto.ScheduledAt;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _db.Orders.FindAsync(id);
        if (item is null) return NotFound();
        _db.Orders.Remove(item);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
