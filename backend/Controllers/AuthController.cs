using Microsoft.AspNetCore.Mvc;
using Sfarma.Api.DTOs;
using Sfarma.Api.Interfaces;

namespace Sfarma.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _service;
    public AuthController(IAuthService service) => _service = service;

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
    {
        var result = await _service.LoginAsync(request);
        return result is null ? Unauthorized() : Ok(result);
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] UsuarioCreateDto request)
    {
        var result = await _service.RegisterAsync(request);
        return Ok(result);
    }
}
