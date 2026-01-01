using IzmPortal.Application.Common;
using IzmPortal.Application.DTOs.Menu;

namespace IzmPortal.Application.Abstractions.Services;

public interface ISubMenuService
{
    Task<Result<List<SubMenuDto>>> GetByMenuIdAsync(
        Guid menuId,
        CancellationToken ct = default);

    Task<Result<SubMenuDto>> GetByIdAsync(
        Guid id,
        CancellationToken ct = default);

    Task<Result> CreateAsync(
        CreateSubMenuDto dto,
        CancellationToken ct = default);

    Task<Result> UpdateAsync(
        UpdateSubMenuDto dto,
        CancellationToken ct = default);

    Task<Result> ActivateAsync(
        Guid id,
        CancellationToken ct = default);

    Task<Result> DeactivateAsync(
        Guid id,
        CancellationToken ct = default);
}

