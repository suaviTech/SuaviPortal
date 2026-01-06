using IzmPortal.Application.Abstractions.Services;
using IzmPortal.Application.Common;
using IzmPortal.Application.DTOs.Audit;
using IzmPortal.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IzmPortal.Infrastructure.Services;

public class AuditQueryService : IAuditQueryService
{
    private readonly PortalDbContext _db;

    public AuditQueryService(PortalDbContext db)
    {
        _db = db;
    }

    public async Task<Result<List<AuditLogItemDto>>> GetAsync(
     DateTime? from,
     DateTime? to,
     string? action,
     string? entity,
     string? user,
     int take,
     CancellationToken ct = default)
    {
        if (take is < 1 or > 500)
            return Result<List<AuditLogItemDto>>
                .Failure("Take değeri 1–500 aralığında olmalıdır.");

        var query = _db.AdminAuditLogs.AsNoTracking();

        if (from.HasValue)
            query = query.Where(x => x.CreatedAt >= from.Value.Date);

        if (to.HasValue)
        {
            var toEnd = to.Value.Date.AddDays(1).AddTicks(-1);
            query = query.Where(x => x.CreatedAt <= toEnd);
        }

        if (!string.IsNullOrWhiteSpace(action))
            query = query.Where(x => x.Action == action);

        if (!string.IsNullOrWhiteSpace(entity))
            query = query.Where(x => x.EntityName == entity);

        if (!string.IsNullOrWhiteSpace(user))
            query = query.Where(x => x.UserName.Contains(user));

        var items = await query
            .OrderByDescending(x => x.CreatedAt)
            .Take(take)
            .Select(x => new AuditLogItemDto
            {
                UserName = x.UserName,
                Action = x.Action,
                EntityName = x.EntityName,
                EntityId = x.EntityId,
                IpAddress = x.IpAddress,
                CreatedAt = x.CreatedAt
            })
            .ToListAsync(ct);

        return Result<List<AuditLogItemDto>>.Success(items);
    }

}
