using DroneDelivery.WebApi.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace DroneDelivery.WebApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    public ActionResult<LoginResponse> Login([FromBody] LoginRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Email) || string.IsNullOrWhiteSpace(req.Password))
            return BadRequest("Email and password are required.");

        // Dev token - doar ca MAUI sa poata salva un token si sa continue flow-ul.
        return Ok(new LoginResponse(Token: "dev-token"));
    }
}
