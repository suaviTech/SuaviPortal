using IzmPortal.Domain.Entities;

namespace IzmPortal.Application.Abstractions.Repositories;

public interface IAnnouncementRepository
{
    Task<List<Announcement>> GetByCategoryIdAsync(int categoryId, CancellationToken ct = default);
    Task UpdateRangeAsync(List<Announcement> announcements, CancellationToken ct = default);
}
