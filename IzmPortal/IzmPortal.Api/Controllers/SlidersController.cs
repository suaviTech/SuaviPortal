using IzmPortal.Application.Abstractions.Services;
using IzmPortal.Application.Common;
using IzmPortal.Application.DTOs.Slider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IzmPortal.Api.Controllers;

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

        if (!result.Succeeded)
            return BadRequest(result);

        var baseUrl = $"{Request.Scheme}://{Request.Host}";

        var list = result.Data!.Select(s => new SliderDto
        {
            Id = s.Id,
            ImageUrl = FileUrlBuilder.Build(baseUrl, s.ImagePath)
        });

        return Ok(list);
    }


    // POST (admin)
    [Authorize(Policy = "AdminAccess")]
    [HttpPost]
    public async Task<IActionResult> Upload(
        IFormFile file,
        CancellationToken ct)
    {
        if (file == null || file.Length == 0)
            return BadRequest(
                Result.Failure("Dosya boş.", "ERR_EMPTY_FILE"));

        using var stream = file.OpenReadStream();

        var relativePath = await _fileStorageService.SaveAsync(
            stream,
            file.FileName,
            "images/sliders",
            ct);

        var result = await _sliderService.CreateAsync(relativePath, ct);

        if (!result.Succeeded)
            return BadRequest(result);

        return Ok(Result.Success("Slider başarıyla yüklendi."));
    }

    // DELETE (admin)
    [Authorize(Policy = "AdminAccess")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken ct)
    {
        var result = await _sliderService.DeleteAsync(id, ct);

        if (!result.Succeeded)
            return BadRequest(result);

        return Ok(Result.Success("Slider silindi."));
    }
}
