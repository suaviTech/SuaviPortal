using IzmPortal.Domain.Entities;

namespace IzmPortal.Application.Abstractions.Repositories;

public interface IMenuRepository
{
    Task<List<Menu>> GetAllAsync(CancellationToken ct = default);
    Task<Menu?> GetByIdAsync(Guid id, CancellationToken ct = default);

    Task AddAsync(Menu menu, CancellationToken ct = default);
    Task UpdateAsync(Menu menu, CancellationToken ct = default);
}
