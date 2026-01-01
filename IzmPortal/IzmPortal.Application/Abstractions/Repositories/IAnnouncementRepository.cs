using IzmPortal.Domain.Entities;

namespace IzmPortal.Application.Abstractions.Repositories;

public interface IAnnouncementRepository
{
    Task<List<Announcement>> GetAllAsync(
        CancellationToken ct = default);

    Task<Announcement?> GetByIdAsync(
        Guid id,
        CancellationToken ct = default);

    Task<List<Announcement>> GetByCategoryIdAsync(
        Guid categoryId,
        CancellationToken ct = default);

    Task AddAsync(
        Announcement announcement,
        CancellationToken ct = default);

    Task UpdateAsync(
        Announcement announcement,
        CancellationToken ct = default);

    Task UpdateRangeAsync(
        List<Announcement> announcements,
        CancellationToken ct = default);

    Task<List<Announcement>> GetPublicAsync(CancellationToken ct = default);
    Task<List<Announcement>> GetPublicByCategoryIdAsync(
        Guid categoryId,
        CancellationToken ct = default);



}
