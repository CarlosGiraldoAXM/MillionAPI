using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MillionAPI.Application.DTOs;
using MillionAPI.Application.Interfaces;

namespace MillionAPI.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Requiere autenticación para todos los endpoints
public class PropertiesController : ControllerBase
{
    private readonly IPropertyUseCases _propertyUseCases;

    public PropertiesController(IPropertyUseCases propertyUseCases)
    {
        _propertyUseCases = propertyUseCases;
    }

    /// <summary>
    /// Obtiene propiedades con filtros opcionales
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PropertyDto>>> GetProperties([FromQuery] PropertyFilterDto filters)
    {
        var properties = await _propertyUseCases.GetPropertiesWithFiltersAsync(filters);
        return Ok(properties);
    }

    /// <summary>
    /// Obtiene una propiedad por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<PropertyDto>> GetProperty(int id)
    {
        var property = await _propertyUseCases.GetPropertyByIdAsync(id);
        if (property == null)
            return NotFound();

        return Ok(property);
    }


    /// <summary>
    /// Crea una nueva propiedad con múltiples imágenes
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<PropertyDto>> CreateProperty([FromForm] CreatePropertyDto createDto)
    {
        try
        {
            var property = await _propertyUseCases.CreatePropertyAsync(createDto);
            return CreatedAtAction(nameof(GetProperty), new { id = property.Id }, property);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Actualiza una propiedad existente
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProperty(int id, UpdatePropertyDto updateDto)
    {
        try
        {
            var property = await _propertyUseCases.UpdatePropertyAsync(id, updateDto);
            if (property == null)
                return NotFound();

            return Ok(property);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Actualiza únicamente el precio de una propiedad
    /// </summary>
    [HttpPatch("{id}/price")]
    public async Task<IActionResult> UpdatePropertyPrice(int id, UpdatePropertyPriceDto updatePriceDto)
    {
        try
        {
            var result = await _propertyUseCases.UpdatePropertyPriceAsync(id, updatePriceDto);
            if (!result)
                return NotFound();

            return Ok(new { 
                message = "Precio actualizado exitosamente", 
                propertyId = id, 
                newPrice = updatePriceDto.Price.ToString("F2")
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Agrega una o varias imágenes a una propiedad existente
    /// </summary>
    [HttpPost("{id}/images")]
    public async Task<IActionResult> AddImagesToProperty(int id, List<IFormFile> images)
    {
        try
        {
            var result = await _propertyUseCases.AddImagesToPropertyAsync(id, images);
            if (!result)
                return NotFound();

            return Ok(new { 
                message = $"Se agregaron {images.Count} imagen(es) exitosamente", 
                propertyId = id,
                imagesCount = images.Count
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
        
}
