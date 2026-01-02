using IzmPortal.Application.Abstractions.Services;
using IzmPortal.Application.DTOs.Menu;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IzmPortal.Api.Controllers;

[ApiController]
[Route("api/sub-menus")]
[Authorize(Policy = "AdminAccess")]
public class SubMenusController : ControllerBase
{
    private readonly ISubMenuService _subMenuService;

    public SubMenusController(ISubMenuService subMenuService)
    {
        _subMenuService = subMenuService;
    }

    // --------------------
    // GET BY MENU (ADMIN)
    // --------------------
    [HttpGet("by-menu/{menuId:guid}")]
    public async Task<IActionResult> GetByMenuId(Guid menuId, CancellationToken ct)
    {
        var result = await _subMenuService.GetByMenuIdAsync(menuId, ct);
        return Ok(result.Data);
    }

    // --------------------
    // GET BY ID (ADMIN)
    // --------------------
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _subMenuService.GetByIdAsync(id, ct);

        if (!result.Succeeded)
            return NotFound(result.Message);

        return Ok(result.Data);
    }

    // --------------------
    // CREATE (ADMIN)
    // --------------------
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateSubMenuDto dto,
        CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _subMenuService.CreateAsync(dto, ct);

        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }

    // --------------------
    // UPDATE (ADMIN)
    // --------------------
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateSubMenuDto dto,
        CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (id != dto.Id)
            return BadRequest("Id uyuşmazlığı.");

        var result = await _subMenuService.UpdateAsync(dto, ct);

        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }

    // --------------------
    // ACTIVATE (ADMIN)
    // --------------------
    [HttpPut("{id:guid}/activate")]
    public async Task<IActionResult> Activate(Guid id, CancellationToken ct)
    {
        var result = await _subMenuService.ActivateAsync(id, ct);

        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }

    // --------------------
    // DEACTIVATE (ADMIN)
    // --------------------
    [HttpPut("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken ct)
    {
        var result = await _subMenuService.DeactivateAsync(id, ct);

        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }
}
