using IzmPortal.Application.Abstractions.Services;
using IzmPortal.Application.DTOs.ApplicationShortcut;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

    [HttpGet("admin")]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok((await _service.GetAllAsync(ct)).Data);

    [HttpGet("admin/{id}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct)
        => Ok((await _service.GetByIdAsync(id, ct)).Data);

    [HttpPost]
    public async Task<IActionResult> Create(
     [FromBody] CreateApplicationShortcutDto dto,
     CancellationToken ct)
    {
        var result = await _service.CreateAsync(dto, ct);

        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
      Guid id,
      [FromBody] UpdateApplicationShortcutDto dto,
      CancellationToken ct)
    {
        if (id != dto.Id)
            return BadRequest("Id uyuşmazlığı.");

        var result = await _service.UpdateAsync(dto, ct);

        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }


    [HttpPut("{id}/activate")]
    public async Task<IActionResult> Activate(Guid id, CancellationToken ct)
        => Ok((await _service.ActivateAsync(id, ct)).Message);

    [HttpPut("{id}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken ct)
        => Ok((await _service.DeactivateAsync(id, ct)).Message);
}
