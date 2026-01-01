using IzmPortal.Application.Abstractions.Repositories;
using IzmPortal.Application.Abstractions.Services;
using IzmPortal.Application.Common;
using IzmPortal.Application.DTOs.Category;
using IzmPortal.Domain.Entities;

namespace IzmPortal.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IAnnouncementRepository _announcementRepository;

    public CategoryService(
        ICategoryRepository categoryRepository,
        IAnnouncementRepository announcementRepository)
    {
        _categoryRepository = categoryRepository;
        _announcementRepository = announcementRepository;
    }

    public async Task<Result<List<CategoryDto>>> GetAllAsync(CancellationToken ct = default)
    {
        var categories = await _categoryRepository.GetAllAsync(ct);

        var dtoList = categories
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                IsActive = c.IsActive
            })
            .ToList();

        return Result<List<CategoryDto>>.Success(dtoList);
    }

    public async Task<Result<CategoryDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var category = await _categoryRepository.GetByIdAsync(id, ct);
        if (category is null)
            return Result<CategoryDto>.Failure("Kategori bulunamadı.");

        var dto = new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            IsActive = category.IsActive
        };

        return Result<CategoryDto>.Success(dto);
    }

    public async Task<Result> CreateAsync(CreateCategoryDto dto, CancellationToken ct = default)
    {
        var category = new Category(dto.Name);
        await _categoryRepository.AddAsync(category, ct);

        return Result.Success("Kategori oluşturuldu.");
    }

    public async Task<Result> UpdateAsync(UpdateCategoryDto dto, CancellationToken ct = default)
    {
        var category = await _categoryRepository.GetByIdAsync(dto.Id, ct);
        if (category is null)
            return Result.Failure("Kategori bulunamadı.");

        // İsim değişimi
        if (category.Name != dto.Name)
        {
            typeof(Category)
                .GetProperty(nameof(Category.Name))!
                .SetValue(category, dto.Name);
        }

        // AKTİF → PASİF geçişi
        if (category.IsActive && !dto.IsActive)
        {
            category.Deactivate();

            // 🔥 KRİTİK İŞ KURALI
            var announcements =
                await _announcementRepository.GetByCategoryIdAsync(category.Id, ct);

            foreach (var announcement in announcements)
            {
                announcement.Deactivate();
            }

            await _announcementRepository.UpdateRangeAsync(announcements, ct);
        }

        // PASİF → AKTİF (duyurular otomatik açılmaz)
        if (!category.IsActive && dto.IsActive)
        {
            category.Activate();
        }

        await _categoryRepository.UpdateAsync(category, ct);

        return Result.Success("Kategori güncellendi.");
    }
}

