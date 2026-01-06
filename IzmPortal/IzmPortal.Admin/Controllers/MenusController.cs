using IzmPortal.Admin.Extensions;
using IzmPortal.Application.DTOs.Menu;
using Microsoft.AspNetCore.Mvc;

namespace IzmPortal.Admin.Controllers;

public class MenusController : BaseAdminController
{
    public MenusController(IHttpClientFactory factory)
        : base(factory)
    {
    }

    // =======================
    // LIST
    // =======================
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var response = await Api.GetAsync("/api/menus", ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Menüler alınamadı.");

        if (failure != null)
            return failure;

        var items = await response
            .ReadContentAsync<List<MenuDto>>() ?? new();

        return View(items);
    }

    // =======================
    // CREATE (GET)
    // =======================
    public async Task<IActionResult> Create(CancellationToken ct)
    {
        var parents = await LoadParentMenusAsync(ct);
        if (parents.failure != null)
            return parents.failure;

        ViewBag.ParentMenus = parents.items;
        return View();
    }

    // =======================
    // CREATE (POST)
    // =======================
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        CreateMenuDto dto,
        CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            var parents = await LoadParentMenusAsync(ct);
            ViewBag.ParentMenus = parents.items;
            return View(dto);
        }

        var response = await Api.PostAsJsonAsync(
            "/api/menus", dto, ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Menü oluşturulamadı.");

        if (failure != null)
            return failure;

        return SuccessAndRedirect(
            "Menü oluşturuldu.");
    }

    // =======================
    // EDIT (GET)
    // =======================
    public async Task<IActionResult> Edit(Guid id, CancellationToken ct)
    {
        var response = await Api.GetAsync(
            $"/api/menus/{id}", ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Menü bilgileri alınamadı.");

        if (failure != null)
            return failure;

        var item = await response
            .ReadContentAsync<MenuDto>();

        if (item == null)
            return SuccessAndRedirect(
                "Menü bulunamadı.",
                controller: "Menus");

        var parents = await LoadParentMenusAsync(ct);
        if (parents.failure != null)
            return parents.failure;

        ViewBag.ParentMenus = parents.items;

        return View(new UpdateMenuDto
        {
            Id = item.Id,
            Title = item.Title,
            Order = item.Order,
            ParentId = item.ParentId
        });
    }

    // =======================
    // EDIT (POST)
    // =======================
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        UpdateMenuDto dto,
        CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            var parents = await LoadParentMenusAsync(ct);
            ViewBag.ParentMenus = parents.items;
            return View(dto);
        }

        var response = await Api.PutAsJsonAsync(
            $"/api/menus/{dto.Id}", dto, ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Menü güncellenemedi.");

        if (failure != null)
            return failure;

        return SuccessAndRedirect(
            "Menü güncellendi.");
    }

    // =======================
    // HELPERS
    // =======================
    private async Task<(List<MenuDto> items, IActionResult? failure)>
        LoadParentMenusAsync(CancellationToken ct)
    {
        var response = await Api.GetAsync("/api/menus", ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Menüler alınamadı.");

        if (failure != null)
            return (new List<MenuDto>(), failure);

        var items = await response
            .ReadContentAsync<List<MenuDto>>() ?? new();

        return (
            items
                .Where(x => x.ParentId == null)
                .OrderBy(x => x.Order)
                .ToList(),
            null
        );
    }
}
