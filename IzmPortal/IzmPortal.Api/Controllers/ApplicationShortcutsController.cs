using IzmPortal.Application.Abstractions.Services;
using IzmPortal.Application.DTOs.ApplicationShortcut;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IzmPortal.Api.Controllers;

[ApiController]
[Route("api/application-shortcuts")]
public class ApplicationShortcutsController : ControllerBase
{
    private readonly IApplicationShortcutService _service;

    public ApplicationShortcutsController(IApplicationShortcutService service)
    {
        _service = service;
    }

    // hookup --------------------------------------------------
    // PUBLIC
    // --------------------------------------------------

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetPublic()
    {
        var items = await _service.GetPublicAsync();
        return Ok(items);
    }

    // --------------------------------------------------
    // ADMIN
    // --------------------------------------------------

    [HttpGet("admin")]
    [Authorize(Policy = "AdminAccess")]
    public async Task<IActionResult> GetAdmin()
    {
        var items = await _service.GetAdminAsync();
        return Ok(items);
    }

    [HttpGet("admin/{id:guid}")]
    [Authorize(Policy = "AdminAccess")]
    public async Task<IActionResult> GetAdminById(Guid id)
    {
        var item = await _service.GetAdminByIdAsync(id);

        if (item == null)
            return NotFound();

        return Ok(item);
    }

    [HttpPost]
    [Authorize(Policy = "AdminAccess")]
    public async Task<IActionResult> Create(
        [FromBody] CreateUpdateApplicationShortcutDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _service.CreateAsync(dto);
        return Ok("Uygulama kısayolu oluşturuldu.");
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = "AdminAccess")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] CreateUpdateApplicationShortcutDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _service.UpdateAsync(id, dto);
        return Ok("Uygulama kısayolu güncellendi.");
    }

    [HttpPut("{id:guid}/activate")]
    [Authorize(Policy = "AdminAccess")]
    public async Task<IActionResult> Activate(Guid id)
    {
        await _service.ActivateAsync(id);
        return Ok("Uygulama kısayolu aktifleştirildi.");
    }

    [HttpPut("{id:guid}/deactivate")]
    [Authorize(Policy = "AdminAccess")]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        await _service.DeactivateAsync(id);
        return Ok("Uygulama kısayolu pasifleştirildi.");
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "AdminAccess")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);
        return Ok("Uygulama kısayolu silindi.");
    }
}
