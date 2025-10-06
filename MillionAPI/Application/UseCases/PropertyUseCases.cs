using AutoMapper;
using MillionAPI.Application.DTOs;
using MillionAPI.Application.Interfaces;
using MillionAPI.Application.Services;
using MillionAPI.Domain.Entities;
using MillionAPI.Domain.Interfaces;

namespace MillionAPI.Application.UseCases;

public class PropertyUseCases : IPropertyUseCases
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IOwnerRepository _ownerRepository;
    private readonly IPropertyImageRepository _propertyImageRepository;
    private readonly IMapper _mapper;
    private readonly IFileService _fileService;

    public PropertyUseCases(
        IPropertyRepository propertyRepository, 
        IOwnerRepository ownerRepository,
        IPropertyImageRepository propertyImageRepository,
        IMapper mapper,
        IFileService fileService)
    {
        _propertyRepository = propertyRepository;
        _ownerRepository = ownerRepository;
        _propertyImageRepository = propertyImageRepository;
        _mapper = mapper;
        _fileService = fileService;
    }

    public async Task<PropertyDto?> GetPropertyByIdAsync(int id)
    {
        var property = _propertyRepository.GetByIdAsync(id);
        return property != null ? _mapper.Map<PropertyDto>(property) : null;
    }

    public async Task<IEnumerable<PropertyDto>> GetAllPropertiesAsync()
    {
        var properties = await _propertyRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<PropertyDto>>(properties);
    }

    public async Task<PropertyDto> CreatePropertyAsync(CreatePropertyDto createDto)
    {
        // Verificar que el propietario existe
        var ownerExists = await _ownerRepository.ExistsAsync(createDto.OwnerId);
        if (!ownerExists)
            throw new ArgumentException("El propietario especificado no existe");

        // Verificar que el código interno no esté en uso
        var codeExists = _propertyRepository.ExistsByCodeInternal(createDto.CodeInternal);
        if (codeExists)
            throw new ArgumentException("El código interno ya está en uso");

        // Iniciar transacción
        using var transaction = await _propertyRepository.BeginTransactionAsync();
        
        try
        {
            // Crear Property
            var property = new Property(createDto.Name, createDto.Address, createDto.Price, createDto.CodeInternal, createDto.OwnerId, createDto.Year);
            var createdProperty = await _propertyRepository.AddAsync(property);

            // Procesar y guardar imágenes si se proporcionan
            if (createDto.Images != null && createDto.Images.Any())
            {
                foreach (var imageFile in createDto.Images)
                {
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        try
                        {
                            // Procesar imagen y obtener el byte array
                            var imageBytes = await _fileService.ProcessImageAsync(imageFile);
                            
                            if (imageBytes != null)
                            {
                                // Crear PropertyImage
                                var propertyImage = new PropertyImage(createdProperty.Id, imageBytes, true);
                                await _propertyImageRepository.AddAsync(propertyImage);
                            }
                        }
                        catch (Exception ex)
                        {
                            // Si falla el procesamiento de una imagen, hacer rollback
                            await transaction.RollbackAsync();
                            throw new ArgumentException($"Error procesando imagen: {ex.Message}");
                        }
                    }
                }
            }

            // Confirmar transacción
            await transaction.CommitAsync();
            return _mapper.Map<PropertyDto>(createdProperty);
        }
        catch (Exception)
        {
            // Si falla cualquier operación, hacer rollback
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<PropertyDto?> UpdatePropertyAsync(int id, UpdatePropertyDto updateDto)
    {
        var property = _propertyRepository.GetByIdAsync(id);
        if (property == null) return null;

        // Verificar que el código interno no esté en uso por otra propiedad
        var codeExists = _propertyRepository.ExistsByCodeInternal(updateDto.CodeInternal);
        if (codeExists)
        {
            var existingProperty = _propertyRepository.GetByCodeInternal(updateDto.CodeInternal);
            if (existingProperty != null && existingProperty.Id != id)
                throw new ArgumentException("El código interno ya está en uso");
        }

        _mapper.Map(updateDto, property);
        var updatedProperty = await _propertyRepository.UpdateAsync(property);
        return _mapper.Map<PropertyDto>(updatedProperty);
    }

    public async Task<bool> UpdatePropertyPriceAsync(int id, UpdatePropertyPriceDto updatePriceDto)
    {
        var property = _propertyRepository.GetByIdAsync(id);
        if (property == null) return false;

        // Validar que el precio sea válido
        if (updatePriceDto.Price <= 0)
            throw new ArgumentException("El precio debe ser mayor a cero");

        // Actualizar solo el precio
        property.UpdatePrice(updatePriceDto.Price);
        await _propertyRepository.UpdateAsync(property);
        return true;
    }


    public async Task<IEnumerable<PropertyDto>> GetPropertiesWithFiltersAsync(PropertyFilterDto filters)
    {
        var properties = await _propertyRepository.GetByFiltersAsync(
            filters.Name, 
            filters.Address, 
            filters.CodeInternal,
            filters.MinPrice, 
            filters.MaxPrice, 
            filters.Year, 
            filters.IdOwner);
        
        return _mapper.Map<IEnumerable<PropertyDto>>(properties);
    }

    public async Task<bool> AddImagesToPropertyAsync(int propertyId, List<IFormFile> imageFiles)
    {
        // Verificar que la propiedad existe
        var propertyExists = await _propertyRepository.ExistsAsync(propertyId);
        if (!propertyExists)
            return false;

        // Validar que se proporcionaron archivos
        if (imageFiles == null || !imageFiles.Any())
            throw new ArgumentException("No se proporcionaron archivos de imagen");

        // Validar que todos los archivos son válidos
        foreach (var imageFile in imageFiles)
        {
            if (imageFile == null || imageFile.Length == 0)
                throw new ArgumentException("Uno o más archivos de imagen están vacíos");
        }

        try
        {
            // Procesar y guardar cada imagen
            foreach (var imageFile in imageFiles)
            {
                // Procesar imagen y obtener el byte array
                var imageBytes = await _fileService.ProcessImageAsync(imageFile);
                
                if (imageBytes == null)
                    throw new ArgumentException($"No se pudo procesar la imagen: {imageFile.FileName}");

                // Crear PropertyImage
                var propertyImage = new PropertyImage(propertyId, imageBytes, true);
                await _propertyImageRepository.AddAsync(propertyImage);
            }

            return true;
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"Error procesando imágenes: {ex.Message}");
        }
    }
}
