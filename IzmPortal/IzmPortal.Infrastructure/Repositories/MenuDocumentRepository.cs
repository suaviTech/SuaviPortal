using IzmPortal.Application.Abstractions.Repositories;
using IzmPortal.Domain.Entities;
using IzmPortal.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IzmPortal.Infrastructure.Repositories;

public class MenuDocumentRepository : IMenuDocumentRepository
{
    private readonly PortalDbContext _context;

    public MenuDocumentRepository(PortalDbContext context)
    {
        _context = context;
    }

    public async Task<List<MenuDocument>> GetBySubMenuIdAsync(
        Guid subMenuId,
        CancellationToken ct = default)
    {
        return await _context.MenuDocuments
            .Where(x => x.SubMenuId == subMenuId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(ct);
    }

    public async Task<MenuDocument?> GetByIdAsync(
        Guid id,
        CancellationToken ct = default)
    {
        return await _context.MenuDocuments
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task AddAsync(
        MenuDocument document,
        CancellationToken ct = default)
    {
        await _context.MenuDocuments.AddAsync(document, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(
        MenuDocument document,
        CancellationToken ct = default)
    {
        _context.MenuDocuments.Remove(document);
        await _context.SaveChangesAsync(ct);
    }
}
