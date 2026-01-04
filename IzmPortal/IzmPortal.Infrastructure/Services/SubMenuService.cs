using IzmPortal.Application.Abstractions.Services;
using IzmPortal.Application.Common;
using IzmPortal.Application.DTOs.SubMenu;
using IzmPortal.Domain.Entities;
using IzmPortal.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IzmPortal.Infrastructure.Services;

public class SubMenuService : ISubMenuService
{
    private readonly PortalDbContext _db;

    public SubMenuService(PortalDbContext db)
    {
        _db = db;
    }

    public async Task<Result<List<SubMenuDto>>> GetByMenuIdAsync(
        Guid menuId,
        CancellationToken ct = default)
    {
        var items = await _db.SubMenus
            .AsNoTracking()
            .Where(x => x.MenuId == menuId)
            .OrderBy(x => x.Order)
            .Select(x => new SubMenuDto
            {
                Id = x.Id,
                Title = x.Title,
                Order = x.Order,
                IsActive = x.IsActive,
                MenuId = x.MenuId
            })
            .ToListAsync(ct);

        return Result<List<SubMenuDto>>.Success(items);
    }

    public async Task<Result<SubMenuDto>> GetByIdAsync(
        Guid id,
        CancellationToken ct = default)
    {
        var subMenu = await _db.SubMenus.FindAsync(new object[] { id }, ct);

        if (subMenu == null)
            return Result<SubMenuDto>.Failure("Alt menü bulunamadı.");

        return Result<SubMenuDto>.Success(new SubMenuDto
        {
            Id = subMenu.Id,
            Title = subMenu.Title,
            Order = subMenu.Order,
            IsActive = subMenu.IsActive,
            MenuId = subMenu.MenuId
        });
    }

    public async Task<Result> CreateAsync(
        CreateSubMenuDto dto,
        CancellationToken ct = default)
    {
        var menu = await _db.Menus.FindAsync(new object[] { dto.MenuId }, ct);
        if (menu == null)
            return Result.Failure("Üst menü bulunamadı.");

        if (!menu.IsActive)
            return Result.Failure("Pasif menü altına alt menü eklenemez.");

        var maxOrder = await _db.SubMenus
            .Where(x => x.MenuId == dto.MenuId)
            .Select(x => (int?)x.Order)
            .MaxAsync(ct) ?? 0;

        var nextOrder = maxOrder + 1;

        var subMenu = new SubMenu(dto.Title, nextOrder, dto.MenuId);

        _db.SubMenus.Add(subMenu);
        await _db.SaveChangesAsync(ct);

        return Result.Success("Alt menü oluşturuldu.");
    }

    public async Task<Result> UpdateAsync(
        UpdateSubMenuDto dto,
        CancellationToken ct = default)
    {
        var subMenu = await _db.SubMenus.FindAsync(new object[] { dto.Id }, ct);
        if (subMenu == null)
            return Result.Failure("Alt menü bulunamadı.");

        subMenu.UpdateTitle(dto.Title);
        await _db.SaveChangesAsync(ct);

        return Result.Success("Alt menü güncellendi.");
    }

    public async Task<Result> ActivateAsync(
        Guid id,
        CancellationToken ct = default)
    {
        var subMenu = await _db.SubMenus.FindAsync(new object[] { id }, ct);
        if (subMenu == null)
            return Result.Failure("Alt menü bulunamadı.");

        subMenu.Activate();
        await _db.SaveChangesAsync(ct);

        return Result.Success("Alt menü aktif edildi.");
    }

    public async Task<Result> DeactivateAsync(
        Guid id,
        CancellationToken ct = default)
    {
        var subMenu = await _db.SubMenus.FindAsync(new object[] { id }, ct);
        if (subMenu == null)
            return Result.Failure("Alt menü bulunamadı.");

        subMenu.Deactivate();
        await _db.SaveChangesAsync(ct);

        return Result.Success("Alt menü pasif edildi.");
    }
}

