using IzmPortal.Application.Common;
using IzmPortal.Application.DTOs.Menu;

namespace IzmPortal.Application.Abstractions.Services;

public interface IMenuService
{
    // 🔹 Admin + Public (aktif filtre public'te uygulanır)
    Task<Result<List<MenuDto>>> GetAllAsync(
        CancellationToken ct = default);

    Task<Result<MenuDto>> GetByIdAsync(
        Guid id,
        CancellationToken ct = default);

    // 🔹 Admin
    Task<Result> CreateAsync(
        CreateMenuDto dto,
        CancellationToken ct = default);

    Task<Result> UpdateAsync(
        UpdateMenuDto dto,
        CancellationToken ct = default);

    Task<Result> ActivateAsync(
        Guid id,
        CancellationToken ct = default);

    Task<Result> DeactivateAsync(
        Guid id,
        CancellationToken ct = default);
}
