using IzmPortal.Application.DTOs.MenuHierarchy;
using IzmPortal.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IzmPortal.Api.Controllers;

[ApiController]
[Route("api/menu-hierarchy")]
[Authorize(Policy = "AdminAccess")]
public class MenuHierarchyController : ControllerBase
{
    private readonly PortalDbContext _db;

    public MenuHierarchyController(PortalDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken ct)
    {
        var data = await _db.Menus
            .AsNoTracking()
            .OrderBy(m => m.Order)
            .Select(m => new MenuHierarchyDto
            {
                Id = m.Id,
                Title = m.Title,
                SubMenus = m.SubMenus
                    .OrderBy(sm => sm.Order)
                    .Select(sm => new SubMenuHierarchyDto
                    {
                        Id = sm.Id,
                        Title = sm.Title,
                        IsActive = sm.IsActive
                    })
                    .ToList()
            })
            .ToListAsync(ct);

        return Ok(data);
    }
}
