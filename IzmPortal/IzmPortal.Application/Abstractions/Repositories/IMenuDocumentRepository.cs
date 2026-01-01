using IzmPortal.Domain.Entities;

namespace IzmPortal.Application.Abstractions.Repositories;

public interface IMenuDocumentRepository
{
    Task<List<MenuDocument>> GetBySubMenuIdAsync(
        Guid subMenuId,
        CancellationToken ct = default);

    Task<MenuDocument?> GetByIdAsync(
        Guid id,
        CancellationToken ct = default);

    Task AddAsync(
        MenuDocument document,
        CancellationToken ct = default);

    Task DeleteAsync(
        MenuDocument document,
        CancellationToken ct = default);
}

