using IzmPortal.Application.Abstractions.Services;
using IzmPortal.Application.DTOs.MenuDocument;
using IzmPortal.Infrastructure.Services;
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

    [HttpGet("by-submenu/{subMenuId:guid}")]
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

        return result.Succeeded
            ? Ok(result.Message)
            : BadRequest(result.Message);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var result = await _service.DeactivateAsync(id, ct);
        return result.Succeeded
            ? Ok(result.Message)
            : NotFound(result.Message);
    }
}
