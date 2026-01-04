using IzmPortal.Admin.Extensions;
using IzmPortal.Application.DTOs.Menu;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IzmPortal.Admin.Controllers;

[Authorize]
public class MenusController : BaseAdminController
{
    public MenusController(IHttpClientFactory factory)
        : base(factory)
    {
    }

    // LIST
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

    // CREATE (GET)
    public IActionResult Create()
        => View();

    // CREATE (POST)
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateMenuDto dto,
        CancellationToken ct)
    {
        var response = await Api.PostAsJsonAsync(
            "/api/menus", dto, ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Menü oluşturulamadı.");

        if (failure != null)
            return failure;

        return SuccessAndRedirect("Menü oluşturuldu.");
    }

    // EDIT (GET)
    public async Task<IActionResult> Edit(Guid id, CancellationToken ct)
    {
        var response = await Api.GetAsync(
            $"/api/menus/{id}", ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Menü bulunamadı.");

        if (failure != null)
            return failure;

        var item = await response
            .ReadContentAsync<MenuDto>();

        return View(new UpdateMenuDto
        {
            Id = item!.Id,
            Title = item.Title
        });
    }

    // EDIT (POST)
    [HttpPost]
    public async Task<IActionResult> Edit(
        UpdateMenuDto dto,
        CancellationToken ct)
    {
        var response = await Api.PutAsJsonAsync(
            $"/api/menus/{dto.Id}", dto, ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Menü güncellenemedi.");

        if (failure != null)
            return failure;

        return SuccessAndRedirect("Menü güncellendi.");
    }
}
