using IzmPortal.Application.Abstractions.Services;
using IzmPortal.Application.Common;
using IzmPortal.Application.DTOs.Announcement;
using IzmPortal.Domain.Entities;
using IzmPortal.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IzmPortal.Infrastructure.Services;

public class AnnouncementService : IAnnouncementService
{
    private readonly PortalDbContext _db;

    private readonly IFileStorageService _fileStorage;

    public AnnouncementService(
        PortalDbContext db,
        IFileStorageService fileStorage)
    {
        _db = db;
        _fileStorage = fileStorage;
    }


    // ================= ADMIN =================

    public async Task<Result<List<AnnouncementDto>>> GetAllAsync(CancellationToken ct)
    {
        var items = await _db.Announcements
            .AsNoTracking()
            .Include(x => x.Category)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new AnnouncementDto
            {
                Id = x.Id,
                Title = x.Title,
                Content = x.Content,
                CategoryId = x.CategoryId,
                CategoryName = x.Category.Name,
                IsActive = x.IsActive,
                CreatedAt = x.CreatedAt,
                CreatedBy = x.CreatedBy,
                PdfUrl = x.PdfUrl
            })
            .ToListAsync(ct);

        return Result<List<AnnouncementDto>>.Success(items);
    }

    public async Task<Result<AnnouncementDto>> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var entity = await _db.Announcements
            .Include(x => x.Category)
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (entity == null)
            return Result<AnnouncementDto>.Failure("Duyuru bulunamadı.");

        return Result<AnnouncementDto>.Success(new AnnouncementDto
        {
            Id = entity.Id,
            Title = entity.Title,
            Content = entity.Content,
            CategoryId = entity.CategoryId,
            CategoryName = entity.Category.Name,
            IsActive = entity.IsActive,
            CreatedAt = entity.CreatedAt,
            CreatedBy = entity.CreatedBy,
            PdfUrl = entity.PdfUrl
        });
    }

    public async Task<Result> CreateAsync(
        CreateAnnouncementDto dto,
        string createdBy,
        string? pdfUrl,
        CancellationToken ct)
    {
        var entity = new Announcement(
            dto.Title,
            dto.Content,
            dto.CategoryId,
            createdBy,
            pdfUrl);

        _db.Announcements.Add(entity);
        await _db.SaveChangesAsync(ct);

        return Result.Success("Duyuru oluşturuldu.");
    }

    public async Task<Result> UpdateAsync(
        UpdateAnnouncementDto dto,
        string? pdfUrl,
        CancellationToken ct)
    {
        var entity = await _db.Announcements
            .FirstOrDefaultAsync(x => x.Id == dto.Id, ct);

        if (entity == null)
            return Result.Failure("Duyuru bulunamadı.");

        entity.Update(
            dto.Title,
            dto.Content,
            dto.CategoryId,
            dto.IsActive,
            pdfUrl);

        await _db.SaveChangesAsync(ct);

        return Result.Success("Duyuru güncellendi.");
    }

    public async Task<Result> ActivateAsync(Guid id, CancellationToken ct)
    {
        var entity = await _db.Announcements.FindAsync(new object[] { id }, ct);

        if (entity == null)
            return Result.Failure("Duyuru bulunamadı.");

        entity.Activate();
        await _db.SaveChangesAsync(ct);

        return Result.Success();
    }

    public async Task<Result> DeactivateAsync(Guid id, CancellationToken ct)
    {
        var entity = await _db.Announcements.FindAsync(new object[] { id }, ct);

        if (entity == null)
            return Result.Failure("Duyuru bulunamadı.");

        entity.Deactivate();
        await _db.SaveChangesAsync(ct);

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken ct)
    {
        var entity = await _db.Announcements
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (entity == null)
            return Result.Failure("Duyuru bulunamadı.");

        // 🔥 PDF varsa sil
        if (!string.IsNullOrWhiteSpace(entity.PdfUrl))
            await _fileStorage.DeleteAsync(entity.PdfUrl, ct);

        _db.Announcements.Remove(entity);
        await _db.SaveChangesAsync(ct);

        return Result.Success("Duyuru silindi.");
    }


    // ================= PUBLIC =================

    public async Task<Result<List<PublicAnnouncementDto>>> GetPublicAsync(CancellationToken ct)
    {
        var items = await _db.Announcements
            .AsNoTracking()
            .Where(x => x.IsActive)
            .Include(x => x.Category)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new PublicAnnouncementDto
            {
                Id = x.Id,
                Title = x.Title,
                Content = x.Content,
                CategoryName = x.Category.Name,
                CreatedAt = x.CreatedAt
            })
            .ToListAsync(ct);

        return Result<List<PublicAnnouncementDto>>.Success(items);
    }

    public async Task<Result<List<PublicAnnouncementDto>>> GetPublicByCategoryAsync(Guid categoryId, CancellationToken ct)
    {
        var items = await _db.Announcements
            .AsNoTracking()
            .Where(x => x.IsActive && x.CategoryId == categoryId)
            .Include(x => x.Category)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new PublicAnnouncementDto
            {
                Id = x.Id,
                Title = x.Title,
                Content = x.Content,
                CategoryName = x.Category.Name,
                CreatedAt = x.CreatedAt
            })
            .ToListAsync(ct);

        return Result<List<PublicAnnouncementDto>>.Success(items);
    }
}
