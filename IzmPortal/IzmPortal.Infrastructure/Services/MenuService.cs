using IzmPortal.Application.Abstractions.Repositories;
using IzmPortal.Application.Abstractions.Services;
using IzmPortal.Application.Common;
using IzmPortal.Application.DTOs.Menu;
using IzmPortal.Domain.Entities;
using IzmPortal.Domain.Enums;
using IzmPortal.Application.DTOs.SubMenu;

namespace IzmPortal.Infrastructure.Services;

public class MenuService : IMenuService
{
    private readonly IMenuRepository _menuRepository;
    private readonly IAuditService _audit;

    public MenuService(
        IMenuRepository menuRepository,
        IAuditService audit)
    {
        _menuRepository = menuRepository;
        _audit = audit;
    }

    // ================= ADMIN =================

    public async Task<Result<List<MenuDto>>> GetAllAsync(CancellationToken ct = default)
    {
        var menus = await _menuRepository.GetAllAsync(ct);

        var list = menus
            .OrderBy(x => x.Order)
            .Select(m => new MenuDto
            {
                Id = m.Id,
                Title = m.Title,
                Order = m.Order,
                IsActive = m.IsActive,
                SubMenus = m.SubMenus
                    .Where(sm => sm.IsActive)
                    .OrderBy(sm => sm.Order)
                    .Select(sm => new SubMenuDto
                    {
                        Id = sm.Id,
                        Title = sm.Title,
                        Order = sm.Order,
                        IsActive = sm.IsActive
                    }).ToList()
            }).ToList();

        return Result<List<MenuDto>>.Success(list);
    }

    public async Task<Result<MenuDto>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var menu = await _menuRepository.GetByIdAsync(id, ct);

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
        var menu = new Menu(dto.Title, dto.Order);

        await _menuRepository.AddAsync(menu, ct);

        await _audit.LogAsync(
            AuditAction.Create,
            AuditEntity.Menu,
            menu.Id.ToString());

        return Result.Success("Menü oluşturuldu.");
    }

    public async Task<Result> UpdateAsync(UpdateMenuDto dto, CancellationToken ct = default)
    {
        var menu = await _menuRepository.GetByIdAsync(dto.Id, ct);

        if (menu == null)
            return Result.Failure("Menü bulunamadı.");

        menu.Update(dto.Title, dto.Order);
        await _menuRepository.UpdateAsync(menu, ct);

        await _audit.LogAsync(
            AuditAction.Update,
            AuditEntity.Menu,
            menu.Id.ToString());

        return Result.Success("Menü güncellendi.");
    }

    public async Task<Result> ActivateAsync(Guid id, CancellationToken ct = default)
    {
        var menu = await _menuRepository.GetByIdAsync(id, ct);
        if (menu == null)
            return Result.Failure("Menü bulunamadı.");

        menu.Activate();
        await _menuRepository.UpdateAsync(menu, ct);

        return Result.Success();
    }

    public async Task<Result> DeactivateAsync(Guid id, CancellationToken ct = default)
    {
        var menu = await _menuRepository.GetByIdAsync(id, ct);
        if (menu == null)
            return Result.Failure("Menü bulunamadı.");

        menu.Deactivate();
        await _menuRepository.UpdateAsync(menu, ct);

        return Result.Success();
    }
}
