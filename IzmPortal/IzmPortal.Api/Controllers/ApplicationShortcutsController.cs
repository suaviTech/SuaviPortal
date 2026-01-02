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

    // --------------------------------------------------
    // PUBLIC
    // --------------------------------------------------

    [HttpGet]
    public async Task<IActionResult> GetPublic()
    {
        var items = await _service.GetPublicAsync();
        return Ok(items);
    }

    // --------------------------------------------------
    // ADMIN
    // --------------------------------------------------

    [Authorize(Policy = "AdminAccess")]
    [HttpGet("admin")]
    public async Task<IActionResult> GetAdmin()
    {
        var items = await _service.GetAdminAsync();
        return Ok(items);
    }

    [Authorize(Policy = "AdminAccess")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateUpdateApplicationShortcutDto dto)
    {
        await _service.CreateAsync(dto);
        return Ok();
    }

    [Authorize(Policy = "AdminAccess")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, CreateUpdateApplicationShortcutDto dto)
    {
        await _service.UpdateAsync(id, dto);
        return NoContent();
    }

    [Authorize(Policy = "AdminAccess")]
    [HttpPut("{id:guid}/activate")]
    public async Task<IActionResult> Activate(Guid id)
    {
        await _service.ActivateAsync(id);
        return NoContent();
    }

    [Authorize(Policy = "AdminAccess")]
    [HttpPut("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        await _service.DeactivateAsync(id);
        return NoContent();
    }

    [Authorize(Policy = "AdminAccess")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
