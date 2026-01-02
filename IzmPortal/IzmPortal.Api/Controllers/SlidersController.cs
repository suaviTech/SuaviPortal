using IzmPortal.Application.Abstractions.Services;
using IzmPortal.Application.Common;
using IzmPortal.Application.DTOs.Slider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IzmPortal.Api.Controllers;

[ApiController]
[Route("api/sliders")]
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

    // --------------------
    // PUBLIC - GET SLIDERS
    // --------------------
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _sliderService.GetAllAsync(ct);

        if (!result.Succeeded)
            return BadRequest(result.Message);

        var baseUrl = $"{Request.Scheme}://{Request.Host}";

        var list = result.Data!.Select(s => new SliderDto
        {
            Id = s.Id,
            ImageUrl = FileUrlBuilder.Build(baseUrl, s.ImagePath)
        });

        return Ok(list);
    }

    // --------------------
    // ADMIN - UPLOAD SLIDER
    // --------------------
    [HttpPost]
    [Authorize(Policy = "AdminAccess")]
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

        return Ok("Slider başarıyla yüklendi.");
    }

    // --------------------
    // ADMIN - DELETE SLIDER
    // --------------------
    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "AdminAccess")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken ct)
    {
        var result = await _sliderService.DeleteAsync(id, ct);

        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok("Slider silindi.");
    }
}
