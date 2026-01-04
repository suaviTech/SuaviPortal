using IzmPortal.Application.Abstractions.Services;
using IzmPortal.Application.DTOs.Slider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken ct)
        => Ok((await _service.GetAllAsync(ct)).Data);

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Create(
        [FromForm] CreateSliderDto dto,
        IFormFile file,
        CancellationToken ct)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Resim zorunludur.");

        var folder = Path.Combine("wwwroot", "uploads", "images");
        Directory.CreateDirectory(folder);

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var path = Path.Combine(folder, fileName);

        await using var stream = new FileStream(path, FileMode.Create);
        await file.CopyToAsync(stream, ct);

        var result = await _service.CreateAsync(
            dto,
            $"/uploads/images/{fileName}",
            ct);

        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var result = await _service.DeleteAsync(id, ct);
        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }
}
