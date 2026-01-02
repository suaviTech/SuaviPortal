using IzmPortal.Application.Abstractions.Repositories;
using IzmPortal.Application.Abstractions.Services;
using IzmPortal.Application.Common;
using IzmPortal.Application.DTOs.Menu;
using IzmPortal.Domain.Entities;
using IzmPortal.Domain.Enums;

namespace IzmPortal.Application.Services;

public class MenuService : IMenuService
{
    private readonly IMenuRepository _menuRepository;
    private readonly IAuditService _auditService;

    public MenuService(
        IMenuRepository menuRepository,
        IAuditService auditService)
    {
        _menuRepository = menuRepository;
        _auditService = auditService;
    }

    public async Task<Result<List<MenuDto>>> GetAllAsync(
        CancellationToken ct = default)
    {
        var menus = await _menuRepository.GetAllAsync(ct);

        var list = menus
            .Where(x => x.IsActive)
            .Select(x => new MenuDto
            {
                Id = x.Id,
                Title = x.Title,
                Order = x.Order,
                IsActive = x.IsActive,
                SubMenus = x.SubMenus
                    .Where(sm => sm.IsActive)
                    .OrderBy(sm => sm.Order)
                    .Select(sm => new SubMenuDto
                    {
                        Id = sm.Id,
                        Title = sm.Title,
                        Order = sm.Order,
                        IsActive = sm.IsActive
                    })
                    .ToList()
            })
            .ToList();

        return Result<List<MenuDto>>.Success(list);
    }

    public async Task<Result<MenuDto>> GetByIdAsync(
        Guid id,
        CancellationToken ct = default)
    {
        var menu = await _menuRepository.GetByIdAsync(id, ct);
        if (menu is null)
            return Result<MenuDto>.Failure("Menü bulunamadı.");

        var dto = new MenuDto
        {
            Id = menu.Id,
            Title = menu.Title,
            Order = menu.Order,
            IsActive = menu.IsActive,
            SubMenus = menu.SubMenus
                .OrderBy(x => x.Order)
                .Select(sm => new SubMenuDto
                {
                    Id = sm.Id,
                    Title = sm.Title,
                    Order = sm.Order,
                    IsActive = sm.IsActive
                })
                .ToList()
        };

        return Result<MenuDto>.Success(dto);
    }

    public async Task<Result> CreateAsync(
        CreateMenuDto dto,
        CancellationToken ct = default)
    {
        var menu = new Menu(
            dto.Title,
            dto.Order);

        await _menuRepository.AddAsync(menu, ct);

        await _auditService.LogAsync(
            AuditAction.Create,
            AuditEntity.Menu,
            menu.Id.ToString());

        return Result.Success("Menü oluşturuldu.");
    }

    public async Task<Result> UpdateAsync(
        UpdateMenuDto dto,
        CancellationToken ct = default)
    {
        var menu = await _menuRepository.GetByIdAsync(dto.Id, ct);
        if (menu is null)
            return Result.Failure("Menü bulunamadı.");

        menu.Update(
            dto.Title,
            dto.Order);

        await _menuRepository.UpdateAsync(menu, ct);

        await _auditService.LogAsync(
            AuditAction.Update,
            AuditEntity.Menu,
            menu.Id.ToString());

        return Result.Success("Menü güncellendi.");
    }

    public async Task<Result> ActivateAsync(
        Guid id,
        CancellationToken ct = default)
    {
        var menu = await _menuRepository.GetByIdAsync(id, ct);
        if (menu is null)
            return Result.Failure("Menü bulunamadı.");

        menu.Activate();
        await _menuRepository.UpdateAsync(menu, ct);

        await _auditService.LogAsync(
            AuditAction.Activate,
            AuditEntity.Menu,
            menu.Id.ToString());

        return Result.Success("Menü aktif edildi.");
    }

    public async Task<Result> DeactivateAsync(
        Guid id,
        CancellationToken ct = default)
    {
        var menu = await _menuRepository.GetByIdAsync(id, ct);
        if (menu is null)
            return Result.Failure("Menü bulunamadı.");

        menu.Deactivate();
        await _menuRepository.UpdateAsync(menu, ct);

        await _auditService.LogAsync(
            AuditAction.Deactivate,
            AuditEntity.Menu,
            menu.Id.ToString());

        return Result.Success("Menü pasif edildi.");
    }
}
