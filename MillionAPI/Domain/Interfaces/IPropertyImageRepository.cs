using MillionAPI.Domain.Entities;

namespace MillionAPI.Domain.Interfaces;

public interface IPropertyImageRepository
{
    Task<PropertyImage?> GetByIdAsync(int id);
    Task<IEnumerable<PropertyImage>> GetByPropertyIdAsync(int propertyId);
    Task<PropertyImage> AddAsync(PropertyImage image);
    Task<PropertyImage> UpdateAsync(PropertyImage image);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<IEnumerable<PropertyImage>> GetEnabledByPropertyIdAsync(int propertyId);
}
