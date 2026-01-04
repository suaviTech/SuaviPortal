using IzmPortal.Application.Abstractions.Services;
using IzmPortal.Application.DTOs.MenuDocument;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IzmPortal.Api.Controllers;

[ApiController]
[Route("api/menu-documents")]
[Authorize(Policy = "AdminAccess")]
public class MenuDocumentsController : ControllerBase
{
    private readonly IMenuDocumentService _service;

    public MenuDocumentsController(IMenuDocumentService service)
    {
        _service = service;
    }

    [HttpGet("by-submenu/{subMenuId}")]
    public async Task<IActionResult> GetBySubMenu(
        Guid subMenuId,
        CancellationToken ct)
    {
        var result = await _service.GetBySubMenuAsync(subMenuId, ct);
        return Ok(result.Data);
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Create(
        [FromForm] CreateMenuDocumentDto dto,
        IFormFile file,
        CancellationToken ct)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Dosya zorunludur.");

        var uploads = Path.Combine("wwwroot", "docs");
        Directory.CreateDirectory(uploads);

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine(uploads, fileName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream, ct);

        var result = await _service.CreateAsync(
            dto,
            $"/docs/{fileName}",
            ct);

        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
    Guid id,
    [FromBody] UpdateMenuDocumentDto dto,
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
    {
        var result = await _service.ActivateAsync(id, ct);

        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }

    [HttpPut("{id}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken ct)
    {
        var result = await _service.DeactivateAsync(id, ct);

        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }

}
