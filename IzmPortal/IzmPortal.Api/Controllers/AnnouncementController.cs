using System.Security.Claims;
using IzmPortal.Application.Abstractions.Services;
using IzmPortal.Application.DTOs.Announcement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IzmPortal.Api.Controllers;

[ApiController]
[Route("api/announcements")]
[Authorize(Policy = "AdminAccess")]
public class AnnouncementsController : ControllerBase
{
    private readonly IAnnouncementService _announcementService;
    private readonly IFileStorageService _fileStorage;

    public AnnouncementsController(
        IAnnouncementService announcementService,
        IFileStorageService fileStorage)
    {
        _announcementService = announcementService;
        _fileStorage = fileStorage;
    }

    // ==================================================
    // GET: api/announcements (ADMIN)
    // ==================================================
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _announcementService.GetAllAsync(ct);
        return Ok(result);
    }

    // ==================================================
    // GET: api/announcements/{id}
    // ==================================================
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _announcementService.GetByIdAsync(id, ct);

        return result.Succeeded
            ? Ok(result)
            : NotFound(result);
    }

    // ==================================================
    // POST: api/announcements
    // ==================================================
    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Create(
        [FromForm] CreateAnnouncementDto dto,
        IFormFile? pdfFile,
        CancellationToken ct)
    {
        var createdBy =
            User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.Identity?.Name
            ?? "system";

        string? pdfUrl = null;

        if (pdfFile != null && pdfFile.Length > 0)
        {
            var folder = Path.Combine(
                "docs",
                DateTime.UtcNow.Year.ToString(),
                DateTime.UtcNow.Month.ToString("D2"));

            pdfUrl = await _fileStorage.SaveAsync(
                pdfFile.OpenReadStream(),
                pdfFile.FileName,
                folder,
                ct);
        }

        var result = await _announcementService.CreateAsync(
            dto,
            createdBy,
            pdfUrl,
            ct);

        return result.Succeeded
            ? Ok(result)
            : BadRequest(result);
    }

    // ==================================================
    // PUT: api/announcements/{id}
    // ==================================================
    [HttpPut("{id:guid}")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromForm] UpdateAnnouncementDto dto,
        IFormFile? pdfFile,
        CancellationToken ct)
    {
        if (id != dto.Id)
            return BadRequest(
                IzmPortal.Application.Common.Result.Failure("Id uyuşmazlığı."));

        string? pdfUrl = dto.ExistingPdfUrl;

        if (pdfFile != null && pdfFile.Length > 0)
        {
            if (!string.IsNullOrWhiteSpace(dto.ExistingPdfUrl))
            {
                await _fileStorage.DeleteAsync(
                    dto.ExistingPdfUrl,
                    ct);
            }

            var folder = Path.Combine(
                "docs",
                DateTime.UtcNow.Year.ToString(),
                DateTime.UtcNow.Month.ToString("D2"));

            pdfUrl = await _fileStorage.SaveAsync(
                pdfFile.OpenReadStream(),
                pdfFile.FileName,
                folder,
                ct);
        }

        var result = await _announcementService.UpdateAsync(
            dto,
            pdfUrl,
            ct);

        return result.Succeeded
            ? Ok(result)
            : BadRequest(result);
    }

    // ==================================================
    // PUT: api/announcements/{id}/activate
    // ==================================================
    [HttpPut("{id:guid}/activate")]
    public async Task<IActionResult> Activate(Guid id, CancellationToken ct)
    {
        var result = await _announcementService
            .ActivateAsync(id, ct);

        return result.Succeeded
            ? Ok(result)
            : BadRequest(result);
    }

    // ==================================================
    // PUT: api/announcements/{id}/deactivate
    // ==================================================
    [HttpPut("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken ct)
    {
        var result = await _announcementService
            .DeactivateAsync(id, ct);

        return result.Succeeded
            ? Ok(result)
            : BadRequest(result);
    }

    // ==================================================
    // DELETE: api/announcements/{id}
    // ==================================================
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var result = await _announcementService
            .DeleteAsync(id, ct);

        return result.Succeeded
            ? Ok(result)
            : BadRequest(result);
    }
}
