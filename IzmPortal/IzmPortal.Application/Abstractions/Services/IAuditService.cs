using IzmPortal.Domain.Enums;

namespace IzmPortal.Application.Abstractions.Services;

public interface IAuditService
{
    Task LogAsync(
        AuditAction action,
        AuditEntity entity,
        string? entityId = null);
}

