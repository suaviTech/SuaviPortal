using Microsoft.EntityFrameworkCore;
using IzmPortal.Application.Abstractions.Repositories;
using IzmPortal.Domain.Entities;
using IzmPortal.Infrastructure.Persistence;

namespace IzmPortal.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly PortalDbContext _context;

    public CategoryRepository(PortalDbContext context)
    {
        _context = context;
    }

    public async Task<Category?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.Categories
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<List<Category>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.Categories
            .OrderBy(x => x.Name)
            .ToListAsync(ct);
    }

    public async Task AddAsync(Category category, CancellationToken ct = default)
    {
        await _context.Categories.AddAsync(category, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Category category, CancellationToken ct = default)
    {
        _context.Categories.Update(category);
        await _context.SaveChangesAsync(ct);
    }
}
