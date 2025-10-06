using AutoMapper;
using MillionAPI.Application.DTOs;
using MillionAPI.Application.Interfaces;
using MillionAPI.Application.Services;
using MillionAPI.Domain.Entities;
using MillionAPI.Domain.Interfaces;

namespace MillionAPI.Application.UseCases;

public class OwnerUseCases : IOwnerUseCases
{
    private readonly IOwnerRepository _ownerRepository;
    private readonly IMapper _mapper;
    private readonly IFileService _fileService;

    public OwnerUseCases(IOwnerRepository ownerRepository, IMapper mapper, IFileService fileService)
    {
        _ownerRepository = ownerRepository;
        _mapper = mapper;
        _fileService = fileService;
    }

    public async Task<IEnumerable<OwnerDto>> GetAllOwnersAsync()
    {
        var owners = await _ownerRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<OwnerDto>>(owners);
    }

    public async Task<OwnerDto> CreateOwnerAsync(CreateOwnerDto createDto)
    {
        // Procesar imagen si se proporciona
        var photoBytes = await _fileService.ProcessImageAsync(createDto.Photo);
        
        // Crear Owner manualmente
        var owner = new Owner(createDto.Name, createDto.Address, photoBytes, createDto.Birthday);
        
        var createdOwner = await _ownerRepository.AddAsync(owner);
        return _mapper.Map<OwnerDto>(createdOwner);
    }

}
