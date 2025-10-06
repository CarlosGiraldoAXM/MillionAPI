using Microsoft.EntityFrameworkCore;
using MillionAPI.Domain.Entities;
using MillionAPI.Domain.Interfaces;
using MillionAPI.Infrastructure.Data;

namespace MillionAPI.Infrastructure.Repositories;

public class OwnerRepository : IOwnerRepository
{
    private readonly ContextDB _context;

    public OwnerRepository(ContextDB context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Owner>> GetAllAsync()
    {
        return await _context.Owner
            .Include(o => o.Properties)
            .ToListAsync();
    }

    public async Task<Owner> AddAsync(Owner owner)
    {
        _context.Owner.Add(owner);
        await _context.SaveChangesAsync();
        return owner;
    }

    public async Task<bool> ExistsAsync(int ownerId)
    {
        return await _context.Owner.AnyAsync(o => o.Id == ownerId);
    }

}
