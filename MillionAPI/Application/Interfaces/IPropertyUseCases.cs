using MillionAPI.Application.DTOs;

namespace MillionAPI.Application.Interfaces;

public interface IPropertyUseCases
{
    /// <summary>
    /// Obtiene una propiedad por su ID
    /// </summary>
    /// <param name="id">ID de la propiedad</param>
    /// <returns>PropertyDto o null si no se encuentra</returns>
    Task<PropertyDto?> GetPropertyByIdAsync(int id);

    /// <summary>
    /// Obtiene todas las propiedades
    /// </summary>
    /// <returns>Lista de PropertyDto</returns>
    Task<IEnumerable<PropertyDto>> GetAllPropertiesAsync();

    /// <summary>
    /// Crea una nueva propiedad
    /// </summary>
    /// <param name="createDto">Datos para crear la propiedad</param>
    /// <returns>PropertyDto de la propiedad creada</returns>
    /// <exception cref="ArgumentException">Si el propietario no existe o el código interno ya está en uso</exception>
    Task<PropertyDto> CreatePropertyAsync(CreatePropertyDto createDto);

    /// <summary>
    /// Actualiza una propiedad existente
    /// </summary>
    /// <param name="id">ID de la propiedad a actualizar</param>
    /// <param name="updateDto">Datos para actualizar la propiedad</param>
    /// <returns>PropertyDto actualizada o null si no se encuentra</returns>
    /// <exception cref="ArgumentException">Si el código interno ya está en uso</exception>
    Task<PropertyDto?> UpdatePropertyAsync(int id, UpdatePropertyDto updateDto);

    /// <summary>
    /// Actualiza únicamente el precio de una propiedad
    /// </summary>
    /// <param name="id">ID de la propiedad</param>
    /// <param name="updatePriceDto">Datos del nuevo precio</param>
    /// <returns>True si se actualizó correctamente, false si no se encontró la propiedad</returns>
    /// <exception cref="ArgumentException">Si el precio no es válido</exception>
    Task<bool> UpdatePropertyPriceAsync(int id, UpdatePropertyPriceDto updatePriceDto);



    /// <summary>
    /// Obtiene propiedades con filtros aplicados
    /// </summary>
    /// <param name="filters">Filtros a aplicar</param>
    /// <returns>Lista de PropertyDto que coinciden con los filtros</returns>
    Task<IEnumerable<PropertyDto>> GetPropertiesWithFiltersAsync(PropertyFilterDto filters);

    /// <summary>
    /// Agrega una o varias imágenes a una propiedad existente
    /// </summary>
    /// <param name="propertyId">ID de la propiedad</param>
    /// <param name="imageFiles">Archivos de imagen</param>
    /// <returns>True si se agregaron correctamente, false si no se encontró la propiedad</returns>
    /// <exception cref="ArgumentException">Si alguna imagen no es válida</exception>
    Task<bool> AddImagesToPropertyAsync(int propertyId, List<IFormFile> imageFiles);
}
