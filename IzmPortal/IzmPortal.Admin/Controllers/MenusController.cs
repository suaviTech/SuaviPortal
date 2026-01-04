using IzmPortal.Application.Abstractions.Services;
using IzmPortal.Application.DTOs.Menu;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IzmPortal.Api.Controllers;

[ApiController]
[Route("api/menus")]
[Authorize(Policy = "AdminAccess")]
public class MenusController : ControllerBase
{
    private readonly IMenuService _menuService;

    public MenusController(IMenuService menuService)
    {
        _menuService = menuService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok((await _menuService.GetAllAsync(ct)).Data);

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _menuService.GetByIdAsync(id, ct);
        return result.Succeeded ? Ok(result.Data) : NotFound(result.Message);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateMenuDto dto, CancellationToken ct)
        => Ok((await _menuService.CreateAsync(dto, ct)).Message);

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateMenuDto dto, CancellationToken ct)
    {
        if (id != dto.Id)
            return BadRequest("Id uyuşmazlığı.");

        return Ok((await _menuService.UpdateAsync(dto, ct)).Message);
    }

    [HttpPut("{id:guid}/activate")]
    public async Task<IActionResult> Activate(Guid id, CancellationToken ct)
        => Ok((await _menuService.ActivateAsync(id, ct)).Message);

    [HttpPut("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken ct)
        => Ok((await _menuService.DeactivateAsync(id, ct)).Message);
}
