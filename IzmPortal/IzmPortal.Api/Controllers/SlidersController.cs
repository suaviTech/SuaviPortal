using IzmPortal.Application.Abstractions.Services;
using IzmPortal.Api.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IzmPortal.Api.Controllers;

[Authorize(Policy = "AdminAccess")]
[ApiController]
[Route("api/[controller]")]
public class SlidersController : ControllerBase
{
    private readonly ISliderService _sliderService;
    private readonly IFileStorageService _fileStorageService;

    public SlidersController(
        ISliderService sliderService,
        IFileStorageService fileStorageService)
    {
        _sliderService = sliderService;
        _fileStorageService = fileStorageService;
    }

    // GET (public)
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _sliderService.GetAllAsync(ct);

        var list = result.Data!.Select(x => new
        {
            x.Id,
            ImageUrl = x.ImageUrl.ToPublicUrl(Request)
        });

        return Ok(list);
    }

    // POST (admin)
    [HttpPost]
    public async Task<IActionResult> Upload(
        IFormFile file,
        CancellationToken ct)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Dosya boş.");

        using var stream = file.OpenReadStream();

        var relativePath = await _fileStorageService.SaveAsync(
            stream,
            file.FileName,
            "images/sliders",
            ct);

        var result = await _sliderService.CreateAsync(relativePath, ct);

        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }

    // DELETE (admin)
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var result = await _sliderService.DeleteAsync(id, ct);

        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }
}
