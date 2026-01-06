using IzmPortal.Application.Abstractions.Services;
using IzmPortal.Application.Common;
using IzmPortal.Application.DTOs.Category;
using IzmPortal.Domain.Entities;
using IzmPortal.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IzmPortal.Infrastructure.Services;

public class CategoryService : ICategoryService
{
    private readonly PortalDbContext _db;

    public CategoryService(PortalDbContext db)
    {
        _db = db;
    }

    // ================= PUBLIC =================
    public async Task<Result<List<CategoryDto>>> GetPublicAsync(
        CancellationToken ct = default)
    {
        var items = await _db.Categories
            .AsNoTracking()
            .Where(x => x.IsActive)
            .OrderBy(x => x.Name)
            .Select(x => new CategoryDto
            {
                Id = x.Id,
                Name = x.Name,
                IsActive = x.IsActive
            })
            .ToListAsync(ct);

        return Result<List<CategoryDto>>.Success(items);
    }

    // ================= ADMIN =================
    public async Task<Result<List<CategoryDto>>> GetAllAsync(
        CancellationToken ct = default)
    {
        var items = await _db.Categories
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .Select(x => new CategoryDto
            {
                Id = x.Id,
                Name = x.Name,
                IsActive = x.IsActive
            })
            .ToListAsync(ct);

        return Result<List<CategoryDto>>.Success(items);
    }

    public async Task<Result<CategoryDto>> GetByIdAsync(
        Guid id,
        CancellationToken ct = default)
    {
        var entity = await _db.Categories
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (entity == null)
            return Result<CategoryDto>.Failure("Kategori bulunamadı.");

        var dto = new CategoryDto
        {
            Id = entity.Id,
            Name = entity.Name,
            IsActive = entity.IsActive
        };

        return Result<CategoryDto>.Success(dto);
    }

    public async Task<Result> CreateAsync(
        CreateCategoryDto dto,
        CancellationToken ct = default)
    {
        var exists = await _db.Categories
            .AnyAsync(x => x.Name == dto.Name, ct);

        if (exists)
            return Result.Failure("Bu kategori zaten mevcut.");

        _db.Categories.Add(new Category(dto.Name));
        await _db.SaveChangesAsync(ct);

        return Result.Success();
    }

    public async Task<Result> UpdateAsync(
        UpdateCategoryDto dto,
        CancellationToken ct = default)
    {
        var entity = await _db.Categories
            .FirstOrDefaultAsync(x => x.Id == dto.Id, ct);

        if (entity == null)
            return Result.Failure("Kategori bulunamadı.");

        entity.Update(dto.Name);
        await _db.SaveChangesAsync(ct);

        return Result.Success();
    }

    public async Task<Result> ActivateAsync(
       Guid id,
       CancellationToken ct = default)
    {
        var category = await _db.Categories
            .FirstOrDefaultAsync(c => c.Id == id, ct);

        if (category == null)
            return Result.Failure("Kategori bulunamadı.");

        // 🔓 SADECE kategori aktif
        category.Activate();

        await _db.SaveChangesAsync(ct);

        return Result.Success("Kategori aktifleştirildi.");
    }


    public async Task<Result> DeactivateAsync(
        Guid id,
        CancellationToken ct = default)
    {
        var category = await _db.Categories
            .Include(c => c.Announcements)
            .FirstOrDefaultAsync(c => c.Id == id, ct);

        if (category == null)
            return Result.Failure("Kategori bulunamadı.");

        // 1️⃣ Kategori pasif
        category.Deactivate();

        // 2️⃣ Bağlı duyurular pasif
        foreach (var announcement in category.Announcements)
        {
            if (announcement.IsActive)
                announcement.Deactivate();
        }

        await _db.SaveChangesAsync(ct);

        return Result.Success(
            "Kategori pasifleştirildi. Bağlı duyurular da pasif hale getirildi.");
    }


}
