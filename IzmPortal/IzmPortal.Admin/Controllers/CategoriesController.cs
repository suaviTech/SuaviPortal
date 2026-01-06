using IzmPortal.Admin.Extensions;
using IzmPortal.Application.DTOs.Category;
using Microsoft.AspNetCore.Mvc;

namespace IzmPortal.Admin.Controllers;

public class CategoriesController : BaseAdminController
{
    public CategoriesController(IHttpClientFactory factory)
        : base(factory)
    {
    }

    // =======================
    // LIST
    // =======================
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var response = await Api.GetAsync("/api/categories/admin", ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Kategoriler alınamadı.");

        if (failure != null)
            return failure;

        var items = await response
            .ReadContentAsync<List<CategoryDto>>() ?? new();

        return View(items);
    }

    // =======================
    // CREATE (GET)
    // =======================
    public IActionResult Create()
    {
        return View();
    }

    // =======================
    // CREATE (POST)
    // =======================
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        CreateCategoryDto dto,
        CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var response = await Api.PostAsJsonAsync(
            "/api/categories", dto, ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Kategori oluşturulamadı.");

        if (failure != null)
            return failure;

        return SuccessAndRedirect(
            "Kategori başarıyla oluşturuldu.");
    }

    // =======================
    // EDIT (GET)
    // =======================
    public async Task<IActionResult> Edit(Guid id, CancellationToken ct)
    {
        var response = await Api.GetAsync(
            $"/api/categories/{id}", ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Kategori bilgileri alınamadı.");

        if (failure != null)
            return failure;

        var item = await response
            .ReadContentAsync<CategoryDto>();

        if (item == null)
            return SuccessAndRedirect(
                "Kategori bulunamadı.",
                controller: "Categories");


        return View(new UpdateCategoryDto
        {
            Id = item.Id,
            Name = item.Name
        });
    }

    // =======================
    // EDIT (POST)
    // =======================
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        UpdateCategoryDto dto,
        CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var response = await Api.PutAsJsonAsync(
            $"/api/categories/{dto.Id}", dto, ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Kategori güncellenemedi.");

        if (failure != null)
            return failure;

        return SuccessAndRedirect(
            "Kategori başarıyla güncellendi.");
    }

    // =======================
    // ACTIVATE
    // =======================
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Activate(Guid id, CancellationToken ct)
    {
        var response = await Api.PutAsync(
            $"/api/categories/{id}/activate",
            null,
            ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Kategori aktifleştirilemedi.");

        if (failure != null)
            return failure;

        return SuccessAndRedirect(
            "Kategori aktifleştirildi.");
    }

    // =======================
    // DEACTIVATE
    // =======================
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken ct)
    {
        var response = await Api.PutAsync(
            $"/api/categories/{id}/deactivate",
            null,
            ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Kategori pasifleştirilemedi.");

        if (failure != null)
            return failure;

        return SuccessAndRedirect(
            "Kategori pasifleştirildi.");
    }
}
