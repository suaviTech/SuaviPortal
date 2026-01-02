using IzmPortal.Domain.Entities;

namespace IzmPortal.Application.Abstractions.Repositories;

public interface IApplicationShortcutRepository
{
    // -----------------------------
    // QUERIES
    // -----------------------------

    Task<List<ApplicationShortcut>> GetAllAsync();

    Task<List<ApplicationShortcut>> GetActiveAsync();

    Task<ApplicationShortcut?> GetByIdAsync(Guid id);

    Task<int> GetMaxOrderAsync();

    // -----------------------------
    // COMMANDS
    // -----------------------------

    Task AddAsync(ApplicationShortcut entity);

    void Update(ApplicationShortcut entity);

    void Delete(ApplicationShortcut entity);

    Task SaveChangesAsync(); // 👈 EKLENDİ
}
