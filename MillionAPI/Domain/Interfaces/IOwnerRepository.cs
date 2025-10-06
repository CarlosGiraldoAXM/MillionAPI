using MillionAPI.Domain.Entities;

namespace MillionAPI.Domain.Interfaces;

public interface IOwnerRepository
{
    Task<IEnumerable<Owner>> GetAllAsync();
    Task<Owner> AddAsync(Owner owner);
    Task<bool> ExistsAsync(int ownerId);
}
