using IzmPortal.Application.Common;
using IzmPortal.Application.DTOs.Announcement;

namespace IzmPortal.Application.Abstractions.Services;

public interface IAnnouncementService
{
    // ADMIN
    Task<Result<List<AnnouncementDto>>> GetAllAsync(CancellationToken ct);
    Task<Result<AnnouncementDto>> GetByIdAsync(Guid id, CancellationToken ct);
    Task<Result> CreateAsync(
       CreateAnnouncementDto dto,
       string createdBy,
       string? pdfUrl,
       CancellationToken ct);

    Task<Result> UpdateAsync(
        UpdateAnnouncementDto dto,
        string? pdfUrl,
        CancellationToken ct);

    Task<Result> ActivateAsync(Guid id, CancellationToken ct);
    Task<Result> DeactivateAsync(Guid id, CancellationToken ct);
    Task<Result> DeleteAsync(Guid id, CancellationToken ct);

    // PUBLIC
    Task<Result<List<PublicAnnouncementDto>>> GetPublicAsync(CancellationToken ct);
    Task<Result<List<PublicAnnouncementDto>>> GetPublicByCategoryAsync(Guid categoryId, CancellationToken ct);
    

}
