using IzmPortal.Application.Abstractions.Services;
using IzmPortal.Application.Common;
using IzmPortal.Application.DTOs.Slider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IzmPortal.Api.Controllers;

[ApiController]
[Route("api/sliders")]
[Authorize(Policy = "AdminAccess")]
public class SlidersController : ControllerBase
{
    private readonly ISliderService _service;

    public SlidersController(ISliderService service)
    {
        _service = service;
    }

    // ==================================================
    // GET: api/sliders
    // ==================================================
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken ct)
    {
        var result = await _service.GetAllAsync(ct);
        return Ok(result);
    }

    // ==================================================
    // POST: api/sliders
    // ==================================================
    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Create(
        [FromForm] CreateSliderDto dto,
        IFormFile file,
        CancellationToken ct)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(
                Result.Failure("Resim zorunludur."));
        }

        var folder = Path.Combine(
            "wwwroot",
            "uploads",
            "images");

        Directory.CreateDirectory(folder);

        var fileName =
            $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

        var path = Path.Combine(folder, fileName);

        await using (var stream = new FileStream(path, FileMode.Create))
        {
            await file.CopyToAsync(stream, ct);
        }

        var imagePath = $"/uploads/images/{fileName}";

        var result = await _service.CreateAsync(
            dto,
            imagePath,
            ct);

        return result.Succeeded
            ? Ok(result)
            : BadRequest(result);
    }

    // ==================================================
    // DELETE: api/sliders/{id}
    // ==================================================
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken ct)
    {
        var result = await _service.DeleteAsync(id, ct);

        return result.Succeeded
            ? Ok(result)
            : BadRequest(result);
    }
}
