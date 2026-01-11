using System.Security.Claims;
using DroneDelivery.WebApi.Data;
using DroneDelivery.WebApi.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DroneDelivery.WebApi.Controllers;

[Route("admin")]
public class AdminController : Controller
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _config;

    public AdminController(AppDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    [HttpGet("login")]
    [AllowAnonymous]
    public IActionResult Login() => View();

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> LoginPost([FromForm] string username, [FromForm] string password)
    {
        var adminUser = _config["AdminAuth:Username"] ?? "admin";
        var adminPass = _config["AdminAuth:Password"] ?? "admin123";

        if (!string.Equals(username, adminUser, StringComparison.Ordinal) ||
            !string.Equals(password, adminPass, StringComparison.Ordinal))
        {
            ViewData["Error"] = "Invalid credentials.";
            return View("Login");
        }

        var claims = new List<Claim> { new(ClaimTypes.Name, username), new(ClaimTypes.Role, "Admin") };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

        return RedirectToAction(nameof(Orders));
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction(nameof(Login));
    }

    [HttpGet("orders")]
    [Authorize]
    public async Task<IActionResult> Orders()
    {
        var items = await _db.Orders
            .OrderByDescending(o => o.Id)
            .Select(o => new AdminOrderRow(
                o.Id,
                o.Email,
                o.ScheduledAt,
                o.Status,
                o.TotalWeightKg,
                _db.Addresses.FirstOrDefault(a => a.Id == o.AddressId)
            ))
            .ToListAsync();

        return View(items);
    }
}

public record AdminOrderRow(
    int Id,
    string Email,
    DateTime? ScheduledAt,
    string Status,
    double TotalWeightKg,
    Address? Address
);
