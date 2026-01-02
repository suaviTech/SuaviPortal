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

    // --------------------
    // GET AUDIT LOGS
    // --------------------
    [HttpGet]
    public async Task<IActionResult> Get(
        DateTime? from,
        DateTime? to,
        int take = 200)
    {
        if (take <= 0 || take > 500)
            return BadRequest("Take değeri 1–500 aralığında olmalıdır.");

        IQueryable<AdminAuditLog> query = _db.AdminAuditLogs.AsNoTracking();

        if (from.HasValue)
            query = query.Where(x => x.CreatedAt >= from.Value);

        if (to.HasValue)
            query = query.Where(x => x.CreatedAt <= to.Value);

        var items = await query
            .OrderByDescending(x => x.CreatedAt)
            .Take(take)
            .ToListAsync();

        return Ok(items);
    }
}
