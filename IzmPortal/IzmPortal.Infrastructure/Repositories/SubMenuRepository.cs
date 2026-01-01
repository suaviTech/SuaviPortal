using IzmPortal.Application.Abstractions.Repositories;
using IzmPortal.Domain.Entities;
using IzmPortal.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IzmPortal.Infrastructure.Repositories;

public class SubMenuRepository : ISubMenuRepository
{
    private readonly PortalDbContext _context;

    public SubMenuRepository(PortalDbContext context)
    {
        _context = context;
    }

    public async Task<List<SubMenu>> GetByMenuIdAsync(
        Guid menuId,
        CancellationToken ct = default)
    {
        return await _context.SubMenus
            .Where(x => x.MenuId == menuId)
            .OrderBy(x => x.Order)
            .ToListAsync(ct);
    }

    public async Task<SubMenu?> GetByIdAsync(
        Guid id,
        CancellationToken ct = default)
    {
        return await _context.SubMenus
            .Include(x => x.Documents)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task AddAsync(
        SubMenu subMenu,
        CancellationToken ct = default)
    {
        await _context.SubMenus.AddAsync(subMenu, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(
        SubMenu subMenu,
        CancellationToken ct = default)
    {
        _context.SubMenus.Update(subMenu);
        await _context.SaveChangesAsync(ct);
    }
}

