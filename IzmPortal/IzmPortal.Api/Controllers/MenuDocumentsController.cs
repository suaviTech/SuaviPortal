using IzmPortal.Application.Abstractions.Services;
using IzmPortal.Api.Models.Menu;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IzmPortal.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MenuDocumentsController : ControllerBase
{
    private readonly IMenuDocumentService _menuDocumentService;

    public MenuDocumentsController(IMenuDocumentService menuDocumentService)
    {
        _menuDocumentService = menuDocumentService;
    }

    // 🔓 Public + Admin
    // Alt menüye bağlı PDF listesi
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

    // 🔐 Admin
    // PDF Upload
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

    // 🔐 Admin
    // PDF Hard Delete
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

