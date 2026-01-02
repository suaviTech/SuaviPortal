using IzmPortal.Application.Abstractions.Services;
using IzmPortal.Domain.Entities;
using IzmPortal.Domain.Enums;
using IzmPortal.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;

namespace IzmPortal.Infrastructure.Services;

public class AuditService : IAuditService
{
    private readonly PortalDbContext _db;
    private readonly IHttpContextAccessor _http;

    public AuditService(
        PortalDbContext db,
        IHttpContextAccessor http)
    {
        _db = db;
        _http = http;
    }

    public async Task LogAsync(
        AuditAction action,
        AuditEntity entity,
        string? entityId = null)
    {
        var httpContext = _http.HttpContext;

        var userId =
            httpContext?.User?.FindFirst("sub")?.Value
            ?? httpContext?.User?.FindFirst("userId")?.Value
            ?? "system";

        var userName =
            httpContext?.User?.Identity?.Name ?? "system";

        var ipAddress =
            httpContext?.Connection.RemoteIpAddress?.ToString()
            ?? "unknown";

        var log = new AdminAuditLog
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            UserName = userName,
            Action = action.ToString(),
            EntityName = entity.ToString(),
            EntityId = entityId,
            IpAddress = ipAddress,
            CreatedAt = DateTime.UtcNow
        };

        _db.AdminAuditLogs.Add(log);
        await _db.SaveChangesAsync();
    }
}
