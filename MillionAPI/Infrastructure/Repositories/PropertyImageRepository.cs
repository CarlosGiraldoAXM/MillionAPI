using Microsoft.EntityFrameworkCore;
using MillionAPI.Domain.Entities;
using MillionAPI.Domain.Interfaces;
using MillionAPI.Infrastructure.Data;

namespace MillionAPI.Infrastructure.Repositories;

public class PropertyImageRepository : IPropertyImageRepository
{
    private readonly ContextDB _context;

    public PropertyImageRepository(ContextDB context)
    {
        _context = context;
    }

    public async Task<PropertyImage?> GetByIdAsync(int id)
    {
        return await _context.PropertyImage
            .Include(pi => pi.Property)
            .FirstOrDefaultAsync(pi => pi.Id == id);
    }

    public async Task<IEnumerable<PropertyImage>> GetByPropertyIdAsync(int propertyId)
    {
        return await _context.PropertyImage
            .Include(pi => pi.Property)
            .Where(pi => pi.PropertyId == propertyId)
            .ToListAsync();
    }

    public async Task<PropertyImage> AddAsync(PropertyImage image)
    {
        _context.PropertyImage.Add(image);
        await _context.SaveChangesAsync();
        return image;
    }

    public async Task<PropertyImage> UpdateAsync(PropertyImage image)
    {
        _context.PropertyImage.Update(image);
        await _context.SaveChangesAsync();
        return image;
    }

    public async Task DeleteAsync(int id)
    {
        var image = await _context.PropertyImage.FindAsync(id);
        if (image != null)
        {
            _context.PropertyImage.Remove(image);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.PropertyImage.AnyAsync(pi => pi.Id == id);
    }

    public async Task<IEnumerable<PropertyImage>> GetEnabledByPropertyIdAsync(int propertyId)
    {
        return await _context.PropertyImage
            .Include(pi => pi.Property)
            .Where(pi => pi.PropertyId == propertyId && pi.Enabled == true)
            .ToListAsync();
    }
}
