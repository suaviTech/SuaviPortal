using Microsoft.EntityFrameworkCore;
using IzmPortal.Application.Abstractions.Repositories;
using IzmPortal.Domain.Entities;
using IzmPortal.Infrastructure.Persistence;

namespace IzmPortal.Infrastructure.Repositories;

public class AnnouncementRepository : IAnnouncementRepository
{
    private readonly PortalDbContext _context;

    public AnnouncementRepository(PortalDbContext context)
    {
        _context = context;
    }

    public async Task<List<Announcement>> GetByCategoryIdAsync(
        int categoryId,
        CancellationToken ct = default)
    {
        return await _context.Announcements
            .Where(x => x.CategoryId == categoryId && x.IsActive)
            .ToListAsync(ct);
    }

    public async Task UpdateRangeAsync(
        List<Announcement> announcements,
        CancellationToken ct = default)
    {
        _context.Announcements.UpdateRange(announcements);
        await _context.SaveChangesAsync(ct);
    }
}
