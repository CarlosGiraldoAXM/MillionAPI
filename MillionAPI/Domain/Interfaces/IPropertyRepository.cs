using MillionAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace MillionAPI.Domain.Interfaces;

public interface IPropertyRepository
{
    Property? GetByIdAsync(int id);
    Task<IEnumerable<Property>> GetAllAsync();
    Task<Property> AddAsync(Property property);
    Task<Property> UpdateAsync(Property property);
    Task<bool> ExistsAsync(int id);
    Task<IEnumerable<Property>> GetByOwnerIdAsync(int ownerId);
    Property? GetByCodeInternal(string codeInternal);
    bool ExistsByCodeInternal(string codeInternal);
    IEnumerable<Property> GetByPriceRange(decimal minPrice, decimal maxPrice);
    Task<IEnumerable<Property>> GetByFiltersAsync(string? name, string? address, string? codeInternal, decimal? minPrice, decimal? maxPrice, int? year, int? ownerId);
    Task<IDbContextTransaction> BeginTransactionAsync();
}
