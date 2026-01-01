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

    public async Task<List<Announcement>> GetAllAsync(
        CancellationToken ct = default)
    {
        return await _context.Announcements
            .Include(x => x.Category)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(ct);
    }

    public async Task<Announcement?> GetByIdAsync(
        Guid id,
        CancellationToken ct = default)
    {
        return await _context.Announcements
            .Include(x => x.Category)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<List<Announcement>> GetByCategoryIdAsync(
        Guid categoryId,
        CancellationToken ct = default)
    {
        return await _context.Announcements
            .Where(x => x.CategoryId == categoryId)
            .ToListAsync(ct);
    }

    public async Task AddAsync(
        Announcement announcement,
        CancellationToken ct = default)
    {
        await _context.Announcements.AddAsync(announcement, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(
        Announcement announcement,
        CancellationToken ct = default)
    {
        _context.Announcements.Update(announcement);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateRangeAsync(
        List<Announcement> announcements,
        CancellationToken ct = default)
    {
        _context.Announcements.UpdateRange(announcements);
        await _context.SaveChangesAsync(ct);
    }
    public async Task<List<Announcement>> GetPublicAsync(CancellationToken ct = default)
    {
        return await _context.Announcements
            .Include(x => x.Category)
            .Where(x => x.IsActive && x.Category.IsActive)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(ct);
    }

    public async Task<List<Announcement>> GetPublicByCategoryIdAsync(
        Guid categoryId,
        CancellationToken ct = default)
    {
        return await _context.Announcements
            .Include(x => x.Category)
            .Where(x =>
                x.IsActive &&
                x.Category.IsActive &&
                x.CategoryId == categoryId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(ct);
    }

}
