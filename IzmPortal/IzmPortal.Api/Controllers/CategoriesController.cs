using IzmPortal.Application.Abstractions.Services;
using IzmPortal.Application.DTOs.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IzmPortal.Api.Controllers;

[ApiController]
[Route("api/categories")]
[Authorize(Policy = "AdminAccess")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    // --------------------
    // GET ALL (ADMIN)
    // --------------------
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _categoryService.GetAllAsync(ct);
        return Ok(result.Data);
    }

    // --------------------
    // GET BY ID (ADMIN)
    // --------------------
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _categoryService.GetByIdAsync(id, ct);

        if (!result.Succeeded)
            return NotFound(result.Message);

        return Ok(result.Data);
    }

    // --------------------
    // CREATE (ADMIN)
    // --------------------
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateCategoryDto dto,
        CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _categoryService.CreateAsync(dto, ct);

        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok("Kategori oluşturuldu.");
    }

    // --------------------
    // UPDATE (ADMIN)
    // --------------------
    [HttpPut("{id:guid}")]
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

        return Ok("Kategori güncellendi.");
    }
}
