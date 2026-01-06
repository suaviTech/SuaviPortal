using IzmPortal.Application.Abstractions.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IzmPortal.Api.Controllers;

[ApiController]
[Route("api/audit-logs")]
[Authorize(Policy = "SuperAdminOnly")]
public class AuditLogsController : ControllerBase
{
    private readonly IAuditQueryService _query;

    public AuditLogsController(IAuditQueryService query)
    {
        _query = query;
    }

    [HttpGet]
    public async Task<IActionResult> Get(
    DateTime? from,
    DateTime? to,
    string? action,
    string? entity,
    string? user,
    int take = 200,
    CancellationToken ct = default)
    {
        var result = await _query.GetAsync(
            from, to, action, entity, user, take, ct);

        return result.Succeeded
            ? Ok(result)
            : BadRequest(result);
    }

}
