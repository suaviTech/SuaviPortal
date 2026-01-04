using IzmPortal.Application.Abstractions.Services;
using IzmPortal.Application.DTOs.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IzmPortal.Api.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    // ==================================================
    // PUBLIC — SADECE AKTİF KATEGORİLER
    // ==================================================
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetPublic(CancellationToken ct)
    {
        var result = await _categoryService.GetPublicAsync(ct);
        return Ok(result.Data);
    }

    // ==================================================
    // ADMIN — TÜM KATEGORİLER
    // ==================================================
    [HttpGet("admin")]
    [Authorize(Policy = "AdminAccess")]
    public async Task<IActionResult> GetAllAdmin(CancellationToken ct)
    {
        var result = await _categoryService.GetAllAsync(ct);
        return Ok(result.Data);
    }

    // ==================================================
    // GET BY ID (ADMIN)
    // ==================================================
    [HttpGet("{id:guid}")]
    [Authorize(Policy = "AdminAccess")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _categoryService.GetByIdAsync(id, ct);

        if (!result.Succeeded)
            return NotFound(result.Message);

        return Ok(result.Data);
    }

    // ==================================================
    // CREATE (ADMIN)
    // ==================================================
    [HttpPost]
    [Authorize(Policy = "AdminAccess")]
    public async Task<IActionResult> Create(
        [FromBody] CreateCategoryDto dto,
        CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _categoryService.CreateAsync(dto, ct);

        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(new { message = "Kategori oluşturuldu." });
    }

    // ==================================================
    // UPDATE (ADMIN)
    // ==================================================
    [HttpPut("{id:guid}")]
    [Authorize(Policy = "AdminAccess")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateCategoryDto dto,
        CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (id != dto.Id)
            return BadRequest("Id uyuşmazlığı.");

        var result = await _categoryService.UpdateAsync(dto, ct);

        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(new { message = "Kategori güncellendi." });
    }

    // ==================================================
    // ACTIVATE (ADMIN)
    // ==================================================
    [HttpPut("{id:guid}/activate")]
    [Authorize(Policy = "AdminAccess")]
    public async Task<IActionResult> Activate(Guid id, CancellationToken ct)
    {
        var result = await _categoryService.ActivateAsync(id, ct);

        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(new { message = "Kategori aktifleştirildi." });
    }

    // ==================================================
    // DEACTIVATE (ADMIN)
    // ==================================================
    [HttpPut("{id:guid}/deactivate")]
    [Authorize(Policy = "AdminAccess")]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken ct)
    {
        var result = await _categoryService.DeactivateAsync(id, ct);

        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(new { message = "Kategori pasifleştirildi." });
    }
}
