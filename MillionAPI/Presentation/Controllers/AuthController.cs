using Microsoft.AspNetCore.Mvc;
using MillionAPI.Application.DTOs;
using MillionAPI.Application.Interfaces;
using System.Security.Claims;

namespace MillionAPI.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Autentica un usuario y devuelve un token JWT
    /// </summary>
    /// <param name="loginRequest">Credenciales del usuario</param>
    /// <returns>Token JWT y información del usuario</returns>
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto loginRequest)
    {
        try
        {
            var result = await _authService.LoginAsync(loginRequest);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Error interno del servidor", details = ex.Message });
        }
    }

}
