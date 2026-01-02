using IzmPortal.Application.Abstractions.Services;
using IzmPortal.Application.DTOs.Menu;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IzmPortal.Api.Controllers;

[ApiController]
[Route("api/menus")]
public class MenusController : ControllerBase
{
    private readonly IMenuService _menuService;

    public MenusController(IMenuService menuService)
    {
        _menuService = menuService;
    }

    // --------------------
    // PUBLIC - GET MENUS
    // --------------------
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _menuService.GetAllAsync(ct);
        return Ok(result.Data);
    }

    // --------------------
    // ADMIN - GET BY ID
    // --------------------
    [HttpGet("{id:guid}")]
    [Authorize(Policy = "AdminAccess")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _menuService.GetByIdAsync(id, ct);

        if (!result.Succeeded)
            return NotFound(result.Message);

        return Ok(result.Data);
    }

    // --------------------
    // ADMIN - CREATE
    // --------------------
    [HttpPost]
    [Authorize(Policy = "AdminAccess")]
    public async Task<IActionResult> Create(
        [FromBody] CreateMenuDto dto,
        CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _menuService.CreateAsync(dto, ct);

        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }

    // --------------------
    // ADMIN - UPDATE
    // --------------------
    [HttpPut("{id:guid}")]
    [Authorize(Policy = "AdminAccess")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateMenuDto dto,
        CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (id != dto.Id)
            return BadRequest("Id uyuşmazlığı.");

        var result = await _menuService.UpdateAsync(dto, ct);

        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }

    // --------------------
    // ADMIN - ACTIVATE
    // --------------------
    [HttpPut("{id:guid}/activate")]
    [Authorize(Policy = "AdminAccess")]
    public async Task<IActionResult> Activate(Guid id, CancellationToken ct)
    {
        var result = await _menuService.ActivateAsync(id, ct);

        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }

    // --------------------
    // ADMIN - DEACTIVATE
    // --------------------
    [HttpPut("{id:guid}/deactivate")]
    [Authorize(Policy = "AdminAccess")]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken ct)
    {
        var result = await _menuService.DeactivateAsync(id, ct);

        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }
}
