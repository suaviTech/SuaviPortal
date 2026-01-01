using IzmPortal.Domain.Entities;

namespace IzmPortal.Application.Abstractions.Repositories;

public interface ISubMenuRepository
{
    Task<List<SubMenu>> GetByMenuIdAsync(
        Guid menuId,
        CancellationToken ct = default);

    Task<SubMenu?> GetByIdAsync(
        Guid id,
        CancellationToken ct = default);

    Task AddAsync(
        SubMenu subMenu,
        CancellationToken ct = default);

    Task UpdateAsync(
        SubMenu subMenu,
        CancellationToken ct = default);
}

