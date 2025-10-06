using MillionAPI.Application.DTOs;

namespace MillionAPI.Application.Interfaces;

public interface IOwnerUseCases
{
    Task<IEnumerable<OwnerDto>> GetAllOwnersAsync();
    Task<OwnerDto> CreateOwnerAsync(CreateOwnerDto createDto);
}
