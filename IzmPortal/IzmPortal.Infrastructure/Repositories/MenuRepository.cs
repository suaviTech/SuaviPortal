using IzmPortal.Application.Abstractions.Repositories;
using IzmPortal.Domain.Entities;
using IzmPortal.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IzmPortal.Infrastructure.Repositories;

public class MenuRepository : IMenuRepository
{
    private readonly PortalDbContext _context;

    public MenuRepository(PortalDbContext context)
    {
        _context = context;
    }

    public async Task<List<Menu>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.Menus
            .Include(x => x.SubMenus)
            .OrderBy(x => x.Order)
            .ToListAsync(ct);
    }

    public async Task<Menu?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.Menus
            .Include(x => x.SubMenus)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task AddAsync(Menu menu, CancellationToken ct = default)
    {
        await _context.Menus.AddAsync(menu, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Menu menu, CancellationToken ct = default)
    {
        _context.Menus.Update(menu);
        await _context.SaveChangesAsync(ct);
    }
}
