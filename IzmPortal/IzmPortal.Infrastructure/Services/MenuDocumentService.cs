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

    public async Task<Result<List<MenuDocumentDto>>> GetBySubMenuAsync(
        Guid subMenuId,
        CancellationToken ct = default)
    {
        var items = await _db.MenuDocuments
            .AsNoTracking()
            .Where(x => x.SubMenuId == subMenuId)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new MenuDocumentDto
            {
                Id = x.Id,
                Title = x.Title,
                FilePath = x.FilePath,
                IsActive = x.IsActive,
                CreatedAt = x.CreatedAt
            })
            .ToListAsync(ct);

        return Result<List<MenuDocumentDto>>.Success(items);
    }

    public async Task<Result> CreateAsync(
        CreateMenuDocumentDto dto,
        string filePath,
        CancellationToken ct = default)
    {
        var entity = new MenuDocument(
            dto.Title,
            filePath,
            dto.SubMenuId);

        _db.MenuDocuments.Add(entity);
        await _db.SaveChangesAsync(ct);

        return Result.Success("Doküman eklendi.");
    }

    public async Task<Result> UpdateAsync(
        UpdateMenuDocumentDto dto,
        CancellationToken ct = default)
    {
        var entity = await _db.MenuDocuments
            .FirstOrDefaultAsync(x => x.Id == dto.Id, ct);

        if (entity == null)
            return Result.Failure("Doküman bulunamadı.");

        entity.Update(dto.Title);
        await _db.SaveChangesAsync(ct);

        return Result.Success("Doküman güncellendi.");
    }

    public async Task<Result> ActivateAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.MenuDocuments.FindAsync(new object[] { id }, ct);
        if (entity == null)
            return Result.Failure("Doküman bulunamadı.");

        entity.Activate();
        await _db.SaveChangesAsync(ct);

        return Result.Success("Doküman aktif edildi.");
    }

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
