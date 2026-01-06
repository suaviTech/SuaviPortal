using IzmPortal.Application.Abstractions.Services;
using IzmPortal.Application.Common;
using IzmPortal.Application.DTOs.MenuDocument;
using IzmPortal.Domain.Entities;
using IzmPortal.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IzmPortal.Infrastructure.Services;

public class MenuDocumentService : IMenuDocumentService
{
    private readonly PortalDbContext _db;

    public MenuDocumentService(PortalDbContext db)
    {
        _db = db;
    }

    // LIST – Dokümanlar (SADECE alt menü olan MenuId)
    public async Task<Result<List<MenuDocumentDto>>> GetByMenuAsync(
        Guid menuId,
        CancellationToken ct = default)
    {
        // Menü var mı + alt menü mü?
        var menu = await _db.Menus
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == menuId, ct);

        if (menu == null || menu.ParentId == null)
            return Result<List<MenuDocumentDto>>
                .Failure("Dokümanlar yalnızca alt menüler için listelenebilir.");

        var items = await _db.MenuDocuments
            .AsNoTracking()
            .Where(x => x.MenuId == menuId)
            .OrderBy(x => x.Order)
            .Select(x => new MenuDocumentDto
            {
                Id = x.Id,
                MenuId = x.MenuId,
                Title = x.Title,
                FilePath = x.FilePath,
                Order = x.Order,
                IsActive = x.IsActive,
                CreatedAt = x.CreatedAt
            })
            .ToListAsync(ct);

        return Result<List<MenuDocumentDto>>.Success(items);
    }

    // CREATE – Upload
    public async Task<Result> CreateAsync(
        CreateMenuDocumentDto dto,
        string filePath,
        CancellationToken ct = default)
    {
        // Menü var mı + alt menü mü?
        var menu = await _db.Menus
            .FirstOrDefaultAsync(x => x.Id == dto.MenuId, ct);

        if (menu == null)
            return Result.Failure("Menü bulunamadı.");

        if (menu.ParentId == null)
            return Result.Failure("Ana menülere doküman eklenemez.");

        if (!menu.IsActive)
            return Result.Failure("Pasif menüye doküman eklenemez.");

        var maxOrder = await _db.MenuDocuments
            .Where(x => x.MenuId == dto.MenuId)
            .Select(x => (int?)x.Order)
            .MaxAsync(ct) ?? 0;

        var nextOrder = maxOrder + 1;

        var entity = new MenuDocument(
            dto.Title,
            filePath,
            nextOrder,
            dto.MenuId);

        _db.MenuDocuments.Add(entity);
        await _db.SaveChangesAsync(ct);

        return Result.Success("Doküman eklendi.");
    }

    // UPDATE – Title
    public async Task<Result> UpdateAsync(
        UpdateMenuDocumentDto dto,
        CancellationToken ct = default)
    {
        var entity = await _db.MenuDocuments
            .FirstOrDefaultAsync(x => x.Id == dto.Id, ct);

        if (entity == null)
            return Result.Failure("Doküman bulunamadı.");

        entity.UpdateTitle(dto.Title);
        await _db.SaveChangesAsync(ct);

        return Result.Success("Doküman güncellendi.");
    }

    // ACTIVATE
    public async Task<Result> ActivateAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.MenuDocuments.FindAsync(new object[] { id }, ct);
        if (entity == null)
            return Result.Failure("Doküman bulunamadı.");

        entity.Activate();
        await _db.SaveChangesAsync(ct);

        return Result.Success("Doküman aktif edildi.");
    }

    // DEACTIVATE (Soft delete)
    public async Task<Result> DeactivateAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.MenuDocuments.FindAsync(new object[] { id }, ct);
        if (entity == null)
            return Result.Failure("Doküman bulunamadı.");

        entity.Deactivate();
        await _db.SaveChangesAsync(ct);

        return Result.Success("Doküman pasif edildi.");
    }
}
