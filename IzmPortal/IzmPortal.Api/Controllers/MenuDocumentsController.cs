using IzmPortal.Application.Abstractions.Services;
using IzmPortal.Application.Common;
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

    // ==================================================
    // GET: api/menu-documents/by-menu/{menuId}
    // ==================================================
    [HttpGet("by-menu/{menuId:guid}")]
    public async Task<IActionResult> GetByMenu(
        Guid menuId,
        CancellationToken ct)
    {
        var result = await _service.GetByMenuAsync(menuId, ct);

        return result.Succeeded
            ? Ok(result)
            : NotFound(result);
    }

    // ==================================================
    // POST: api/menu-documents
    // ==================================================
    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Create(
        [FromForm] CreateMenuDocumentDto dto,
        IFormFile file,
        CancellationToken ct)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(
                Result.Failure("Dosya zorunludur."));
        }

        var uploads = Path.Combine(
            "wwwroot",
            "docs");

        Directory.CreateDirectory(uploads);

        var fileName =
            $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

        var filePath =
            Path.Combine(uploads, fileName);

        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream, ct);
        }

        var documentPath = $"/docs/{fileName}";

        var result = await _service.CreateAsync(
            dto,
            documentPath,
            ct);

        return result.Succeeded
            ? Ok(result)
            : BadRequest(result);
    }

    // ==================================================
    // DELETE: api/menu-documents/{id}
    // (Soft delete)
    // ==================================================
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken ct)
    {
        var result = await _service.DeactivateAsync(id, ct);

        return result.Succeeded
            ? Ok(result)
            : NotFound(result);
    }
}
