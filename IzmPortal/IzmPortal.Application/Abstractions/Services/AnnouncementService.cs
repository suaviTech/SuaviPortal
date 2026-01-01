using IzmPortal.Application.Abstractions.Repositories;
using IzmPortal.Application.Abstractions.Services;
using IzmPortal.Application.Common;
using IzmPortal.Application.DTOs.Announcement;
using IzmPortal.Domain.Entities;


namespace IzmPortal.Application.Services;

public class AnnouncementService : IAnnouncementService
{
    private readonly IAnnouncementRepository _announcementRepository;
    private readonly ICategoryRepository _categoryRepository;

    public AnnouncementService(
        IAnnouncementRepository announcementRepository,
        ICategoryRepository categoryRepository)
    {
        _announcementRepository = announcementRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<Result> CreateAsync(
        CreateAnnouncementDto dto,
        string createdBy,
        CancellationToken ct = default)
    {
        var category = await _categoryRepository.GetByIdAsync(dto.CategoryId, ct);
        if (category is null)
            return Result.Failure("Kategori bulunamadı.");

        if (!category.IsActive)
            return Result.Failure("Pasif kategoriye duyuru eklenemez.");

        var announcement = new Announcement(
            dto.Title,
            dto.Content,
            dto.CategoryId,
            createdBy);

        await _announcementRepository.AddAsync(announcement, ct);

        return Result.Success("Duyuru oluşturuldu.");
    }

    public async Task<Result> UpdateAsync(
        UpdateAnnouncementDto dto,
        CancellationToken ct = default)
    {
        var announcement = await _announcementRepository.GetByIdAsync(dto.Id, ct);
        if (announcement is null)
            return Result.Failure("Duyuru bulunamadı.");

        if (announcement.CategoryId != dto.CategoryId)
        {
            var category = await _categoryRepository.GetByIdAsync(dto.CategoryId, ct);
            if (category is null || !category.IsActive)
                return Result.Failure("Pasif kategoriye taşınamaz.");
        }

        announcement.Update(
            dto.Title,
            dto.Content,
            dto.CategoryId,
            dto.IsActive);

        await _announcementRepository.UpdateAsync(announcement, ct);

        return Result.Success("Duyuru güncellendi.");
    }

    public async Task<Result> ActivateAsync(Guid id, CancellationToken ct = default)
    {
        var announcement = await _announcementRepository.GetByIdAsync(id, ct);
        if (announcement is null)
            return Result.Failure("Duyuru bulunamadı.");

        announcement.Activate();
        await _announcementRepository.UpdateAsync(announcement, ct);

        return Result.Success("Duyuru aktif edildi.");
    }

    public async Task<Result> DeactivateAsync(Guid id, CancellationToken ct = default)
    {
        var announcement = await _announcementRepository.GetByIdAsync(id, ct);
        if (announcement is null)
            return Result.Failure("Duyuru bulunamadı.");

        announcement.Deactivate();
        await _announcementRepository.UpdateAsync(announcement, ct);

        return Result.Success("Duyuru pasif edildi.");
    }

    public async Task<Result<List<AnnouncementDto>>> GetAllAsync(
    CancellationToken ct = default)
    {
        var announcements = await _announcementRepository.GetAllAsync(ct);

        var dtoList = announcements
    .Select(a => new AnnouncementDto
    {
        Id = a.Id,
        Title = a.Title,
        Content = a.Content,
        CategoryId = a.CategoryId,
        CategoryName = a.Category.Name,
        IsActive = a.IsActive,
        CreatedAt = a.CreatedAt,
        CreatedBy = a.CreatedBy
    })
    .ToList();


        return Result<List<AnnouncementDto>>.Success(dtoList);
    }

    public async Task<Result<AnnouncementDto>> GetByIdAsync(
    Guid id,
    CancellationToken ct = default)
    {
        var announcement = await _announcementRepository.GetByIdAsync(id, ct);
        if (announcement is null)
            return Result<AnnouncementDto>.Failure("Duyuru bulunamadı.");

        var dto = new AnnouncementDto
        {
            Id = announcement.Id,
            Title = announcement.Title,
            Content = announcement.Content,
            CategoryId = announcement.CategoryId,
            CategoryName = announcement.Category.Name,
            IsActive = announcement.IsActive,
            CreatedAt = announcement.CreatedAt,
            CreatedBy = announcement.CreatedBy
        };


        return Result<AnnouncementDto>.Success(dto);
    }

    public async Task<Result<List<PublicAnnouncementDto>>> GetPublicAsync(
    CancellationToken ct = default)
    {
        var announcements = await _announcementRepository.GetPublicAsync(ct);

        var dtoList = announcements.Select(a => new PublicAnnouncementDto
        {
            Id = a.Id,
            Title = a.Title,
            Content = a.Content,
            CategoryName = a.Category.Name,
            CreatedAt = a.CreatedAt
        }).ToList();

        return Result<List<PublicAnnouncementDto>>.Success(dtoList);
    }

    public async Task<Result<List<PublicAnnouncementDto>>> GetPublicByCategoryAsync(
        Guid categoryId,
        CancellationToken ct = default)
    {
        var announcements =
            await _announcementRepository.GetPublicByCategoryIdAsync(categoryId, ct);

        var dtoList = announcements.Select(a => new PublicAnnouncementDto
        {
            Id = a.Id,
            Title = a.Title,
            Content = a.Content,
            CategoryName = a.Category.Name,
            CreatedAt = a.CreatedAt
        }).ToList();

        return Result<List<PublicAnnouncementDto>>.Success(dtoList);
    }


}
