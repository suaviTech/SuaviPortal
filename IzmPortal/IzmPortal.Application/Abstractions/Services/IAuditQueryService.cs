using IzmPortal.Application.Common;
using IzmPortal.Application.DTOs.Audit;

namespace IzmPortal.Application.Abstractions.Services;

public interface IAuditQueryService
{
    Task<Result<List<AuditLogItemDto>>> GetAsync(
        DateTime? from,
        DateTime? to,
        string? action,
        string? entity,
        string? user,
        int take,
        CancellationToken ct = default);

}

