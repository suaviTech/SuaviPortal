using IzmPortal.Domain.Entities;
using IzmPortal.Domain.Enums;
using IzmPortal.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IzmPortal.Api.Controllers;

[ApiController]
[Route("api/audit-logs")]
[Authorize(Policy = "SuperAdminOnly")]
public class AuditLogsController : ControllerBase
{
    private readonly PortalDbContext _db;

    public AuditLogsController(PortalDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> Get(
        DateTime? from,
        DateTime? to,
        int take = 200)
    {
        if (take is < 1 or > 500)
            return BadRequest("Take değeri 1–500 aralığında olmalıdır.");

        var query = _db.AdminAuditLogs.AsNoTracking();

        if (from.HasValue)
            query = query.Where(x => x.CreatedAt >= from.Value.Date);

        if (to.HasValue)
        {
            var toEnd = to.Value.Date.AddDays(1).AddTicks(-1);
            query = query.Where(x => x.CreatedAt <= toEnd);
        }

        var items = await query
            .OrderByDescending(x => x.CreatedAt)
            .Take(take)
            .Select(x => new
            {
                x.UserName,
                x.Action,
                x.EntityName,
                x.EntityId,
                x.IpAddress,
                x.CreatedAt
            })
            .ToListAsync();

        return Ok(items);
    }
}
