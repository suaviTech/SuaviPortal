using IzmPortal.Application.Abstractions.Services;
using IzmPortal.Application.Common;
using IzmPortal.Application.DTOs.Menu;
using IzmPortal.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


public class MenuService : IMenuService
{
    private readonly PortalDbContext _db;

    public MenuService(PortalDbContext db)
    {
        _db = db;
    }

    public async Task<Result<List<MenuDto>>> GetAllAsync(CancellationToken ct = default)
    {
        var items = await _db.Menus
            .AsNoTracking()
            .OrderBy(x => x.Order)
            .Select(x => new MenuDto
            {
                Id = x.Id,
                Title = x.Title,
                Order = x.Order,
                IsActive = x.IsActive
            })
            .ToListAsync(ct);

        return Result<List<MenuDto>>.Success(items);
    }

    public async Task<Result<MenuDto>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var menu = await _db.Menus.FindAsync(new object[] { id }, ct);

        if (menu == null)
            return Result<MenuDto>.Failure("Menü bulunamadı.");

        return Result<MenuDto>.Success(new MenuDto
        {
            Id = menu.Id,
            Title = menu.Title,
            Order = menu.Order,
            IsActive = menu.IsActive
        });
    }

    public async Task<Result> CreateAsync(CreateMenuDto dto, CancellationToken ct = default)
    {
        var nextOrder = await _db.Menus.AnyAsync(ct)
            ? await _db.Menus.MaxAsync(x => x.Order, ct) + 1
            : 1;

        var menu = new Menu(dto.Title, nextOrder);

        _db.Menus.Add(menu);
        await _db.SaveChangesAsync(ct);

        return Result.Success("Menü oluşturuldu.");
    }

    public async Task<Result> UpdateAsync(UpdateMenuDto dto, CancellationToken ct = default)
    {
        var menu = await _db.Menus.FindAsync(new object[] { dto.Id }, ct);

        if (menu == null)
            return Result.Failure("Menü bulunamadı.");

        // Order değişmiyor
        menu.Update(dto.Title, menu.Order);

        await _db.SaveChangesAsync(ct);
        return Result.Success("Menü güncellendi.");
    }

    public async Task<Result> ActivateAsync(Guid id, CancellationToken ct = default)
    {
        var menu = await _db.Menus.FindAsync(new object[] { id }, ct);
        if (menu == null)
            return Result.Failure("Menü bulunamadı.");

        menu.Activate();
        await _db.SaveChangesAsync(ct);

        return Result.Success("Menü aktif edildi.");
    }

    public async Task<Result> DeactivateAsync(Guid id, CancellationToken ct = default)
    {
        var menu = await _db.Menus.FindAsync(new object[] { id }, ct);
        if (menu == null)
            return Result.Failure("Menü bulunamadı.");

        menu.Deactivate();
        await _db.SaveChangesAsync(ct);

        return Result.Success("Menü pasif edildi.");
    }
}
