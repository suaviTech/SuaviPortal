using IzmPortal.Application.DTOs.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IzmPortal.Admin.Controllers;

[Authorize]
public class CategoriesController : Controller
{
    private readonly HttpClient _api;

    public CategoriesController(IHttpClientFactory factory)
    {
        _api = factory.CreateClient("ApiClient");
    }

    // LIST
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var items = await _api.GetFromJsonAsync<List<CategoryDto>>(
            "/api/categories", ct);

        return View(items);
    }

    // CREATE
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateCategoryDto dto, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var res = await _api.PostAsJsonAsync("/api/categories", dto, ct);

        if (!res.IsSuccessStatusCode)
        {
            ModelState.AddModelError("", "Kategori oluşturulamadı.");
            return View(dto);
        }

        return RedirectToAction(nameof(Index));
    }

    // EDIT
    public async Task<IActionResult> Edit(Guid id, CancellationToken ct)
    {
        var item = await _api.GetFromJsonAsync<CategoryDto>(
            $"/api/categories/{id}", ct);

        if (item == null)
            return NotFound();

        return View(new UpdateCategoryDto
        {
            Id = item.Id,
            Name = item.Name
        });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UpdateCategoryDto dto, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var res = await _api.PutAsJsonAsync(
            $"/api/categories/{dto.Id}", dto, ct);

        if (!res.IsSuccessStatusCode)
        {
            ModelState.AddModelError("", "Kategori güncellenemedi.");
            return View(dto);
        }

        return RedirectToAction(nameof(Index));
    }
}
