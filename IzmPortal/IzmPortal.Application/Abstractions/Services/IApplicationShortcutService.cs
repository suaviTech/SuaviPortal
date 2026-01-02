using IzmPortal.Application.DTOs.ApplicationShortcut;

namespace IzmPortal.Application.Abstractions.Services;

public interface IApplicationShortcutService
{
    // -----------------------------
    // PUBLIC
    // -----------------------------

    Task<List<ApplicationShortcutPublicDto>> GetPublicAsync();

    // -----------------------------
    // ADMIN
    // -----------------------------

    Task<List<ApplicationShortcutAdminDto>> GetAdminAsync();

    Task CreateAsync(CreateUpdateApplicationShortcutDto dto);

    Task UpdateAsync(Guid id, CreateUpdateApplicationShortcutDto dto);

    Task ActivateAsync(Guid id);

    Task DeactivateAsync(Guid id);

    Task DeleteAsync(Guid id);
}
