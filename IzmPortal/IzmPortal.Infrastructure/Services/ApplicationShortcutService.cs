using IzmPortal.Application.Abstractions.Services;
using IzmPortal.Application.Common;
using IzmPortal.Application.DTOs.ApplicationShortcut;
using IzmPortal.Domain.Entities;
using IzmPortal.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IzmPortal.Infrastructure.Services;

public class ApplicationShortcutService : IApplicationShortcutService
{
    private readonly PortalDbContext _db;

    public ApplicationShortcutService(PortalDbContext db)
    {
        _db = db;
    }

    public async Task<Result<List<ApplicationShortcutAdminDto>>> GetAllAsync(CancellationToken ct = default)
    {
        var items = await _db.ApplicationShortcuts
            .AsNoTracking()
            .OrderBy(x => x.Order)
            .Select(x => new ApplicationShortcutAdminDto
            {
                Id = x.Id,
                Title = x.Title,
                Url = x.Url,
                Icon = x.Icon,
                IsActive = x.IsActive,
                Order = x.Order
            })
            .ToListAsync(ct);

        return Result<List<ApplicationShortcutAdminDto>>.Success(items);
    }

    public async Task<Result<ApplicationShortcutAdminDto>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var x = await _db.ApplicationShortcuts.FindAsync(new object[] { id }, ct);

        if (x == null)
            return Result<ApplicationShortcutAdminDto>.Failure("Kayıt bulunamadı.");

        return Result<ApplicationShortcutAdminDto>.Success(new ApplicationShortcutAdminDto
        {
            Id = x.Id,
            Title = x.Title,
            Url = x.Url,
            Icon = x.Icon,
            IsActive = x.IsActive,
            Order = x.Order
        });
    }

    public async Task<Result> CreateAsync(
        CreateApplicationShortcutDto dto,
        CancellationToken ct)
    {
        var entity = new ApplicationShortcut(
            dto.Title,
            dto.Url,
            dto.Icon,
            true,
            dto.Order);


        _db.ApplicationShortcuts.Add(entity);
        await _db.SaveChangesAsync(ct);

        return Result.Success("Kısayol oluşturuldu.");
    }


    public async Task<Result> UpdateAsync(
        UpdateApplicationShortcutDto dto,
        CancellationToken ct)
    {
        var entity = await _db.ApplicationShortcuts
            .FirstOrDefaultAsync(x => x.Id == dto.Id, ct);

        if (entity == null)
            return Result.Failure("Kayıt bulunamadı.");

        entity.Update(
            dto.Title,
            dto.Url,
            dto.Icon,
            dto.Order);

        await _db.SaveChangesAsync(ct);
        return Result.Success("Kısayol güncellendi.");
    }


    public async Task<Result> ActivateAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.ApplicationShortcuts.FindAsync(new object[] { id }, ct);
        if (entity == null)
            return Result.Failure("Kayıt bulunamadı.");

        entity.Activate();
        await _db.SaveChangesAsync(ct);

        return Result.Success();
    }

    public async Task<Result> DeactivateAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.ApplicationShortcuts.FindAsync(new object[] { id }, ct);
        if (entity == null)
            return Result.Failure("Kayıt bulunamadı.");

        entity.Deactivate();
        await _db.SaveChangesAsync(ct);

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.ApplicationShortcuts.FindAsync(new object[] { id }, ct);
        if (entity == null)
            return Result.Failure("Kayıt bulunamadı.");

        _db.ApplicationShortcuts.Remove(entity);
        await _db.SaveChangesAsync(ct);

        return Result.Success("Silindi.");
    }
}
