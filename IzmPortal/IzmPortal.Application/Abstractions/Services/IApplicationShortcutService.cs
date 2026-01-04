using IzmPortal.Application.Common;
using IzmPortal.Application.DTOs.ApplicationShortcut;

namespace IzmPortal.Application.Abstractions.Services;

public interface IApplicationShortcutService
{
    Task<Result<List<ApplicationShortcutAdminDto>>> GetAllAsync(CancellationToken ct);
    Task<Result<ApplicationShortcutAdminDto>> GetByIdAsync(Guid id, CancellationToken ct);

    Task<Result> CreateAsync(CreateApplicationShortcutDto dto, CancellationToken ct);
    Task<Result> UpdateAsync(UpdateApplicationShortcutDto dto, CancellationToken ct);

    Task<Result> ActivateAsync(Guid id, CancellationToken ct);
    Task<Result> DeactivateAsync(Guid id, CancellationToken ct);
    Task<Result> DeleteAsync(Guid id, CancellationToken ct);
}


