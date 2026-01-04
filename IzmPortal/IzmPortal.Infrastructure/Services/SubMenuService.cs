using IzmPortal.Application.Abstractions.Repositories;
using IzmPortal.Application.Abstractions.Services;
using IzmPortal.Application.Common;
using IzmPortal.Application.DTOs.SubMenu;
using IzmPortal.Domain.Entities;
using IzmPortal.Domain.Enums;

namespace IzmPortal.Application.Services;

public class SubMenuService : ISubMenuService
{
    private readonly ISubMenuRepository _subMenuRepository;
    private readonly IMenuRepository _menuRepository;
    private readonly IAuditService _auditService;

    public SubMenuService(
        ISubMenuRepository subMenuRepository,
        IMenuRepository menuRepository,
        IAuditService auditService)
    {
        _subMenuRepository = subMenuRepository;
        _menuRepository = menuRepository;
        _auditService = auditService;
    }

    public async Task<Result<List<SubMenuDto>>> GetByMenuIdAsync(
        Guid menuId,
        CancellationToken ct = default)
    {
        var subMenus = await _subMenuRepository.GetByMenuIdAsync(menuId, ct);

        var list = subMenus
            .Where(x => x.IsActive)
            .Select(x => new SubMenuDto
            {
                Id = x.Id,
                Title = x.Title,
                Order = x.Order,
                IsActive = x.IsActive
            })
            .ToList();

        return Result<List<SubMenuDto>>.Success(list);
    }

    public async Task<Result<SubMenuDto>> GetByIdAsync(
        Guid id,
        CancellationToken ct = default)
    {
        var subMenu = await _subMenuRepository.GetByIdAsync(id, ct);
        if (subMenu is null)
            return Result<SubMenuDto>.Failure("Alt menü bulunamadı.");

        var dto = new SubMenuDto
        {
            Id = subMenu.Id,
            Title = subMenu.Title,
            Order = subMenu.Order,
            IsActive = subMenu.IsActive
        };

        return Result<SubMenuDto>.Success(dto);
    }

    public async Task<Result> CreateAsync(
        CreateSubMenuDto dto,
        CancellationToken ct = default)
    {
        var menu = await _menuRepository.GetByIdAsync(dto.MenuId, ct);
        if (menu is null)
            return Result.Failure("Üst menü bulunamadı.");

        var subMenu = new SubMenu(
            dto.Title,
            dto.Order,
            dto.MenuId);

        await _subMenuRepository.AddAsync(subMenu, ct);

        await _auditService.LogAsync(
            AuditAction.Create,
            AuditEntity.SubMenu,
            subMenu.Id.ToString());

        return Result.Success("Alt menü oluşturuldu.");
    }

    public async Task<Result> UpdateAsync(
        UpdateSubMenuDto dto,
        CancellationToken ct = default)
    {
        var subMenu = await _subMenuRepository.GetByIdAsync(dto.Id, ct);
        if (subMenu is null)
            return Result.Failure("Alt menü bulunamadı.");

        subMenu.Update(
            dto.Title,
            dto.Order);

        await _subMenuRepository.UpdateAsync(subMenu, ct);

        await _auditService.LogAsync(
            AuditAction.Update,
            AuditEntity.SubMenu,
            subMenu.Id.ToString());

        return Result.Success("Alt menü güncellendi.");
    }

    public async Task<Result> ActivateAsync(
        Guid id,
        CancellationToken ct = default)
    {
        var subMenu = await _subMenuRepository.GetByIdAsync(id, ct);
        if (subMenu is null)
            return Result.Failure("Alt menü bulunamadı.");

        subMenu.Activate();
        await _subMenuRepository.UpdateAsync(subMenu, ct);

        await _auditService.LogAsync(
            AuditAction.Activate,
            AuditEntity.SubMenu,
            subMenu.Id.ToString());

        return Result.Success("Alt menü aktif edildi.");
    }

    public async Task<Result> DeactivateAsync(
        Guid id,
        CancellationToken ct = default)
    {
        var subMenu = await _subMenuRepository.GetByIdAsync(id, ct);
        if (subMenu is null)
            return Result.Failure("Alt menü bulunamadı.");

        subMenu.Deactivate();
        await _subMenuRepository.UpdateAsync(subMenu, ct);

        await _auditService.LogAsync(
            AuditAction.Deactivate,
            AuditEntity.SubMenu,
            subMenu.Id.ToString());

        return Result.Success("Alt menü pasif edildi.");
    }
}
