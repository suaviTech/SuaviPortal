using IzmPortal.Application.Abstractions.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IzmPortal.Api.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/public/announcements")]
public class PublicAnnouncementsController : ControllerBase
{
    private readonly IAnnouncementService _announcementService;

    public PublicAnnouncementsController(IAnnouncementService announcementService)
    {
        _announcementService = announcementService;
    }

    // GET: api/public/announcements
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _announcementService.GetPublicAsync(ct);
        return Ok(result.Data);
    }

    // GET: api/public/announcements/by-category/{categoryId}
    [HttpGet("by-category/{categoryId:guid}")]
    public async Task<IActionResult> GetByCategory(
        Guid categoryId,
        CancellationToken ct)
    {
        var result =
            await _announcementService.GetPublicByCategoryAsync(categoryId, ct);

        return Ok(result.Data);
    }
}

