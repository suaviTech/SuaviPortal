using IzmPortal.Application.Common;
using IzmPortal.Application.DTOs.Announcement;

namespace IzmPortal.Application.Abstractions.Services;

public interface IAnnouncementService
{
    Task<Result<List<AnnouncementDto>>> GetAllAsync(CancellationToken ct = default);
    Task<Result<AnnouncementDto>> GetByIdAsync(Guid id, CancellationToken ct = default);

    Task<Result> CreateAsync(
        CreateAnnouncementDto dto,
        string createdBy,
        CancellationToken ct = default);

    Task<Result> UpdateAsync(
        UpdateAnnouncementDto dto,
        CancellationToken ct = default);

    Task<Result> ActivateAsync(Guid id, CancellationToken ct = default);
    Task<Result> DeactivateAsync(Guid id, CancellationToken ct = default);
    Task<Result<List<PublicAnnouncementDto>>> GetPublicAsync(
    CancellationToken ct = default);

    Task<Result<List<PublicAnnouncementDto>>> GetPublicByCategoryAsync(
        Guid categoryId,
        CancellationToken ct = default);

}

