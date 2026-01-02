using IzmPortal.Application.Abstractions.Repositories;
using IzmPortal.Domain.Entities;
using IzmPortal.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IzmPortal.Infrastructure.Repositories;

public class ApplicationShortcutRepository : IApplicationShortcutRepository
{
    private readonly PortalDbContext _context;

    public ApplicationShortcutRepository(PortalDbContext context)
    {
        _context = context;
    }

    // -----------------------------
    // QUERIES
    // -----------------------------

    public async Task<List<ApplicationShortcut>> GetAllAsync()
    {
        return await _context.ApplicationShortcuts
            .OrderBy(x => x.Order)
            .ToListAsync();
    }

    public async Task<List<ApplicationShortcut>> GetActiveAsync()
    {
        return await _context.ApplicationShortcuts
            .Where(x => x.IsActive)
            .OrderBy(x => x.Order)
            .ToListAsync();
    }

    public async Task<ApplicationShortcut?> GetByIdAsync(Guid id)
    {
        return await _context.ApplicationShortcuts
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<int> GetMaxOrderAsync()
    {
        return await _context.ApplicationShortcuts
            .MaxAsync(x => (int?)x.Order) ?? 0;
    }

    // -----------------------------
    // COMMANDS
    // -----------------------------

    public async Task AddAsync(ApplicationShortcut entity)
    {
        await _context.ApplicationShortcuts.AddAsync(entity);
    }

    public void Update(ApplicationShortcut entity)
    {
        _context.ApplicationShortcuts.Update(entity);
    }

    public void Delete(ApplicationShortcut entity)
    {
        _context.ApplicationShortcuts.Remove(entity);
    }
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

}
