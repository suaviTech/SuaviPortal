using IzmPortal.Application.Abstractions.Services;
using IzmPortal.Application.Common;
using IzmPortal.Application.DTOs.ApplicationShortcut;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IzmPortal.Api.Controllers;

[ApiController]
[Route("api/application-shortcuts")]
[Authorize(Policy = "AdminAccess")]
public class ApplicationShortcutsController : ControllerBase
{
    private readonly IApplicationShortcutService _service;

    public ApplicationShortcutsController(IApplicationShortcutService service)
    {
        _service = service;
    }

    // ==================================================
    // GET: api/application-shortcuts/admin
    // ==================================================
    [HttpGet("admin")]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _service.GetAllAsync(ct);
        return Ok(result);
    }

    // ==================================================
    // GET: api/application-shortcuts/admin/{id}
    // ==================================================
    [HttpGet("admin/{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _service.GetByIdAsync(id, ct);

        return result.Succeeded
            ? Ok(result)
            : NotFound(result);
    }

    // ==================================================
    // POST: api/application-shortcuts
    // ==================================================
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateApplicationShortcutDto dto,
        CancellationToken ct)
    {
        var result = await _service.CreateAsync(dto, ct);

        return result.Succeeded
            ? Ok(result)
            : BadRequest(result);
    }

    // ==================================================
    // PUT: api/application-shortcuts/{id}
    // ==================================================
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateApplicationShortcutDto dto,
        CancellationToken ct)
    {
        if (id != dto.Id)
        {
            return BadRequest(
                Result.Failure("Id uyuşmazlığı."));
        }

        var result = await _service.UpdateAsync(dto, ct);

        return result.Succeeded
            ? Ok(result)
            : BadRequest(result);
    }

    // ==================================================
    // PUT: api/application-shortcuts/{id}/activate
    // ==================================================
    [HttpPut("{id:guid}/activate")]
    public async Task<IActionResult> Activate(
        Guid id,
        CancellationToken ct)
    {
        var result = await _service.ActivateAsync(id, ct);

        return result.Succeeded
            ? Ok(result)
            : BadRequest(result);
    }

    // ==================================================
    // PUT: api/application-shortcuts/{id}/deactivate
    // ==================================================
    [HttpPut("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(
        Guid id,
        CancellationToken ct)
    {
        var result = await _service.DeactivateAsync(id, ct);

        return result.Succeeded
            ? Ok(result)
            : BadRequest(result);
    }
}
