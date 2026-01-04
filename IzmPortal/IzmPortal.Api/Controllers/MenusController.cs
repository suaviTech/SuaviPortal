using IzmPortal.Application.Abstractions.Services;
using IzmPortal.Application.DTOs.Menu;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _service.GetAllAsync(ct);
        return result.Succeeded
            ? Ok(result.Data)
            : BadRequest(result.Message);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _service.GetByIdAsync(id, ct);
        return result.Succeeded
            ? Ok(result.Data)
            : NotFound(result.Message);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMenuDto dto, CancellationToken ct)
    {
        var result = await _service.CreateAsync(dto, ct);
        return result.Succeeded
            ? Ok(result.Message)
            : BadRequest(result.Message);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateMenuDto dto, CancellationToken ct)
    {
        if (id != dto.Id)
            return BadRequest("Id uyuşmazlığı.");

        var result = await _service.UpdateAsync(dto, ct);
        return result.Succeeded
            ? Ok(result.Message)
            : BadRequest(result.Message);
    }

    [HttpPut("{id:guid}/activate")]
    public async Task<IActionResult> Activate(Guid id, CancellationToken ct)
    {
        var result = await _service.ActivateAsync(id, ct);
        return result.Succeeded
            ? Ok(result.Message)
            : BadRequest(result.Message);
    }

    [HttpPut("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken ct)
    {
        var result = await _service.DeactivateAsync(id, ct);
        return result.Succeeded
            ? Ok(result.Message)
            : BadRequest(result.Message);
    }
}
