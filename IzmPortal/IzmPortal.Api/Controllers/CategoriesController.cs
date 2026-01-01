using IzmPortal.Application.Abstractions.Services;
using IzmPortal.Application.DTOs.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IzmPortal.Api.Controllers;

[Authorize(Policy = "AdminAccess")]
[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    // GET: api/categories
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _categoryService.GetAllAsync(ct);
        return Ok(result.Data);
    }

    // GET: api/categories/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _categoryService.GetByIdAsync(id, ct);

        if (!result.Succeeded)
            return NotFound(result.Message);

        return Ok(result.Data);
    }

    // POST: api/categories
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateCategoryDto dto,
        CancellationToken ct)
    {
        var result = await _categoryService.CreateAsync(dto, ct);

        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }

    // PUT: api/categories/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateCategoryDto dto,
        CancellationToken ct)
    {
        if (id != dto.Id)
            return BadRequest("Id uyuşmazlığı.");

        var result = await _categoryService.UpdateAsync(dto, ct);

        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }
}
