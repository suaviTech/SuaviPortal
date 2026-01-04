using IzmPortal.Application.Abstractions.Services;
using IzmPortal.Application.DTOs.SubMenu;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IzmPortal.Api.Controllers;

[ApiController]
[Route("api/submenus")]
[Authorize(Policy = "AdminAccess")]
public class SubMenusController : ControllerBase
{
    private readonly ISubMenuService _service;

    public SubMenusController(ISubMenuService service)
    {
        _service = service;
    }

    // GET /api/submenus/by-menu/{menuId}
    [HttpGet("by-menu/{menuId:guid}")]
    public async Task<IActionResult> GetByMenu(
        Guid menuId,
        CancellationToken ct)
    {
        var result = await _service.GetByMenuIdAsync(menuId, ct);
        return Ok(result.Data);
    }

    // GET /api/submenus/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct)
    {
        var result = await _service.GetByIdAsync(id, ct);
        if (!result.Succeeded)
            return NotFound(result.Message);

        return Ok(result.Data);
    }

    // POST
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateSubMenuDto dto,
        CancellationToken ct)
    {
        var result = await _service.CreateAsync(dto, ct);
        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }

    // PUT
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateSubMenuDto dto,
        CancellationToken ct)
    {
        if (id != dto.Id)
            return BadRequest("Id uyuşmazlığı.");

        var result = await _service.UpdateAsync(dto, ct);
        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }

    // ACTIVATE
    [HttpPut("{id:guid}/activate")]
    public async Task<IActionResult> Activate(Guid id, CancellationToken ct)
    {
        var result = await _service.ActivateAsync(id, ct);
        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }

    // DEACTIVATE
    [HttpPut("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken ct)
    {
        var result = await _service.DeactivateAsync(id, ct);
        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }
}
