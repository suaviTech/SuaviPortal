using IzmPortal.Application.Abstractions.Services;
using IzmPortal.Application.Common;
using IzmPortal.Application.DTOs.Menu;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IzmPortal.Api.Controllers;

[ApiController]
[Route("api/menus")]
[Authorize(Policy = "AdminAccess")]
public class MenusController : ControllerBase
{
    private readonly IMenuService _service;

    public MenusController(IMenuService service)
    {
        _service = service;
    }

    // ==================================================
    // GET: api/menus
    // ==================================================
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _service.GetAllAsync(ct);
        return Ok(result);
    }

    // ==================================================
    // GET: api/menus/{id}
    // ==================================================
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _service.GetByIdAsync(id, ct);

        return result.Succeeded
            ? Ok(result)
            : NotFound(result);
    }

    // ==================================================
    // POST: api/menus
    // ==================================================
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateMenuDto dto,
        CancellationToken ct)
    {
        var result = await _service.CreateAsync(dto, ct);

        return result.Succeeded
            ? Ok(result)
            : BadRequest(result);
    }

    // ==================================================
    // PUT: api/menus/{id}
    // ==================================================
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateMenuDto dto,
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
    // PUT: api/menus/{id}/activate
    // ==================================================
    [HttpPut("{id:guid}/activate")]
    public async Task<IActionResult> Activate(Guid id, CancellationToken ct)
    {
        var result = await _service.ActivateAsync(id, ct);

        return result.Succeeded
            ? Ok(result)
            : BadRequest(result);
    }

    // ==================================================
    // PUT: api/menus/{id}/deactivate
    // ==================================================
    [HttpPut("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken ct)
    {
        var result = await _service.DeactivateAsync(id, ct);

        return result.Succeeded
            ? Ok(result)
            : BadRequest(result);
    }
}
