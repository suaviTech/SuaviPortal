using System.Security.Claims;
using IzmPortal.Application.Abstractions.Services;
using IzmPortal.Application.DTOs.Announcement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IzmPortal.Api.Controllers;

[Authorize(Policy = "AdminAccess")]
[ApiController]
[Route("api/announcements")]
public class AnnouncementsController : ControllerBase
{
    private readonly IAnnouncementService _announcementService;

    public AnnouncementsController(IAnnouncementService announcementService)
    {
        _announcementService = announcementService;
    }

    // --------------------------------------------------
    // GET: api/announcements
    // --------------------------------------------------
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _announcementService.GetAllAsync(ct);
        return Ok(result.Data);
    }

    // --------------------------------------------------
    // GET: api/announcements/{id}
    // --------------------------------------------------
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _announcementService.GetByIdAsync(id, ct);

        if (!result.Succeeded || result.Data == null)
            return NotFound(result.Message);

        return Ok(result.Data);
    }

    // --------------------------------------------------
    // POST: api/announcements
    // --------------------------------------------------
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateAnnouncementDto dto,
        CancellationToken ct)
    {
        var createdBy =
            User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.Identity?.Name;

        if (string.IsNullOrWhiteSpace(createdBy))
            return Unauthorized("Kullanıcı bilgisi alınamadı.");

        var result = await _announcementService
            .CreateAsync(dto, createdBy, ct);

        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }

    // --------------------------------------------------
    // PUT: api/announcements/{id}
    // --------------------------------------------------
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateAnnouncementDto dto,
        CancellationToken ct)
    {
        if (id != dto.Id)
            return BadRequest("Id uyuşmazlığı.");

        var result = await _announcementService
            .UpdateAsync(dto, ct);

        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }

    // --------------------------------------------------
    // PUT: api/announcements/{id}/activate
    // --------------------------------------------------
    [HttpPut("{id:guid}/activate")]
    public async Task<IActionResult> Activate(Guid id, CancellationToken ct)
    {
        var result = await _announcementService
            .ActivateAsync(id, ct);

        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }

    // --------------------------------------------------
    // PUT: api/announcements/{id}/deactivate
    // --------------------------------------------------
    [HttpPut("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken ct)
    {
        var result = await _announcementService
            .DeactivateAsync(id, ct);

        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }
}
