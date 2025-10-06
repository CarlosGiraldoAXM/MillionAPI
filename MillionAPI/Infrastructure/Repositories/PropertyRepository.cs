using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MillionAPI.Domain.Entities;
using MillionAPI.Domain.Interfaces;
using MillionAPI.Infrastructure.Data;

namespace MillionAPI.Infrastructure.Repositories;

public class PropertyRepository : IPropertyRepository
{
    private readonly ContextDB _context;

    public PropertyRepository(ContextDB context)
    {
        _context = context;
    }

    public Property? GetByIdAsync(int id)
    {
        Property? query = (from p in _context.Property
                          where p.Id == id
                          select p).FirstOrDefault();
        return query;
    }

    public async Task<IEnumerable<Property>> GetAllAsync()
    {
        return await _context.Property
            .Include(p => p.Owner)
            .Include(p => p.Images)
            .Include(p => p.Traces)
            .ToListAsync();
    }

    public async Task<Property> AddAsync(Property property)
    {
        _context.Property.Add(property);
        await _context.SaveChangesAsync();
        return property;
    }

    public async Task<Property> UpdateAsync(Property property)
    {
        _context.Property.Update(property);
        await _context.SaveChangesAsync();
        return property;
    }    

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Property.AnyAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Property>> GetByOwnerIdAsync(int ownerId)
    {
        return await _context.Property
            .Include(p => p.Owner)
            .Include(p => p.Images)
            .Include(p => p.Traces)
            .Where(p => p.OwnerId == ownerId)
            .ToListAsync();
    }

    public Property? GetByCodeInternal(string codeInternal)
    {
        return _context.Property
            .Include(p => p.Owner)
            .Include(p => p.Images)
            .Include(p => p.Traces)
            .FirstOrDefault(p => p.CodeInternal == codeInternal);
    }

    public bool ExistsByCodeInternal(string codeInternal)
    {
        return _context.Property
            .Any(p => p.CodeInternal == codeInternal);
    }

    public IEnumerable<Property> GetByPriceRange(decimal minPrice, decimal maxPrice)
    {
        return _context.Property
            .Include(p => p.Owner)
            .Include(p => p.Images)
            .Include(p => p.Traces)
            .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
            .ToList();
    }

    public async Task<IEnumerable<Property>> GetByFiltersAsync(string? name, string? address, string? codeInternal, decimal? minPrice, decimal? maxPrice, int? year, int? ownerId)
    {
        var query = _context.Property
            .Include(p => p.Owner)
            .Include(p => p.Images)
            .Include(p => p.Traces)
            .AsQueryable();

        // Aplicar filtros solo si tienen valor
        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(p => p.Name.Contains(name));

        if (!string.IsNullOrWhiteSpace(address))
            query = query.Where(p => p.Address.Contains(address));

        if (!string.IsNullOrWhiteSpace(codeInternal))
            query = query.Where(p => p.CodeInternal.Contains(codeInternal));

        if (minPrice.HasValue)
            query = query.Where(p => p.Price >= minPrice.Value);

        if (maxPrice.HasValue)
            query = query.Where(p => p.Price <= maxPrice.Value);

        if (year.HasValue)
            query = query.Where(p => p.Year == year.Value);

        if (ownerId.HasValue)
            query = query.Where(p => p.OwnerId == ownerId.Value);

        return await query.ToListAsync();
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _context.Database.BeginTransactionAsync();
    }

}
