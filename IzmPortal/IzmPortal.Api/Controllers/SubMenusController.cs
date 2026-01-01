using IzmPortal.Application.Abstractions.Services;
using IzmPortal.Application.DTOs.Menu;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IzmPortal.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "AdminAccess")]
public class SubMenusController : ControllerBase
{
    private readonly ISubMenuService _subMenuService;

    public SubMenusController(ISubMenuService subMenuService)
    {
        _subMenuService = subMenuService;
    }

    // 🔐 Admin – Menüye göre alt menüler
    [HttpGet("by-menu/{menuId:guid}")]
    public async Task<IActionResult> GetByMenuId(Guid menuId, CancellationToken ct)
    {
        var result = await _subMenuService.GetByMenuIdAsync(menuId, ct);
        return Ok(result.Data);
    }

    // 🔐 Admin – Tek alt menü
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _subMenuService.GetByIdAsync(id, ct);

        if (!result.Succeeded)
            return NotFound(result.Message);

        return Ok(result.Data);
    }

    // 🔐 Admin – Oluştur
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateSubMenuDto dto,
        CancellationToken ct)
    {
        var result = await _subMenuService.CreateAsync(dto, ct);

        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }

    // 🔐 Admin – Güncelle
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateSubMenuDto dto,
        CancellationToken ct)
    {
        if (id != dto.Id)
            return BadRequest("Id uyuşmazlığı.");

        var result = await _subMenuService.UpdateAsync(dto, ct);

        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }

    // 🔐 Admin – Aktifleştir
    [HttpPut("{id:guid}/activate")]
    public async Task<IActionResult> Activate(Guid id, CancellationToken ct)
    {
        var result = await _subMenuService.ActivateAsync(id, ct);
        return result.Succeeded ? Ok(result.Message) : BadRequest(result.Message);
    }

    // 🔐 Admin – Pasifleştir
    [HttpPut("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken ct)
    {
        var result = await _subMenuService.DeactivateAsync(id, ct);
        return result.Succeeded ? Ok(result.Message) : BadRequest(result.Message);
    }
}

