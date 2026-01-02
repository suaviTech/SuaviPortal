using IzmPortal.Application.Abstractions.Services;
using IzmPortal.Api.Models.Menu;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IzmPortal.Api.Controllers;

[ApiController]
[Route("api/menu-documents")]
public class MenuDocumentsController : ControllerBase
{
    private readonly IMenuDocumentService _menuDocumentService;

    public MenuDocumentsController(IMenuDocumentService menuDocumentService)
    {
        _menuDocumentService = menuDocumentService;
    }

    // --------------------
    // PUBLIC + ADMIN
    // SUBMENU'YA GÖRE PDF LİSTESİ
    // --------------------
    [HttpGet("by-submenu/{subMenuId:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetBySubMenu(
        Guid subMenuId,
        CancellationToken ct)
    {
        var result =
            await _menuDocumentService.GetBySubMenuIdAsync(subMenuId, ct);

        return Ok(result.Data);
    }

    // --------------------
    // ADMIN
    // PDF UPLOAD
    // --------------------
    [HttpPost]
    [Authorize(Policy = "AdminAccess")]
    public async Task<IActionResult> Upload(
        [FromForm] UploadMenuDocumentRequest request,
        CancellationToken ct)
    {
        if (request.File == null || request.File.Length == 0)
            return BadRequest("Dosya boş.");

        using var stream = request.File.OpenReadStream();

        var result = await _menuDocumentService.UploadAsync(
            request.SubMenuId,
            request.Title,
            stream,
            request.File.FileName,
            ct);

        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }

    // --------------------
    // ADMIN
    // PDF HARD DELETE
    // --------------------
    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "AdminAccess")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken ct)
    {
        var result = await _menuDocumentService.DeleteAsync(id, ct);

        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }
}
