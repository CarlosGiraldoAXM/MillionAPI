using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MillionAPI.Application.DTOs;
using MillionAPI.Application.Interfaces;

namespace MillionAPI.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Requiere autenticación para todos los endpoints
public class OwnersController : ControllerBase
{
    private readonly IOwnerUseCases _ownerUseCases;

    public OwnersController(IOwnerUseCases ownerUseCases)
    {
        _ownerUseCases = ownerUseCases;
    }

    /// <summary>
    /// Obtiene todos los propietarios
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OwnerDto>>> GetOwners()
    {
        var owners = await _ownerUseCases.GetAllOwnersAsync();
        return Ok(owners);
    }

    /// <summary>
    /// Crea un nuevo propietario
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateOwner([FromForm] CreateOwnerDto createDto)
    {
        try
        {
            var owner = await _ownerUseCases.CreateOwnerAsync(createDto);
            return Ok(new { message = "Usuario creado exitosamente", id = owner.Id });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

}
