using IzmPortal.Application.DTOs.Menu;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IzmPortal.Admin.Controllers;

[Authorize]
public class MenusController : Controller
{
    private readonly HttpClient _api;

    public MenusController(IHttpClientFactory factory)
    {
        _api = factory.CreateClient("ApiClient");
    }

    // LIST
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var items = await _api.GetFromJsonAsync<List<MenuDto>>(
            "/api/menus", ct);

        return View(items);
    }

    // CREATE (GET)
    public IActionResult Create()
    {
        return View();
    }

    // CREATE (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        CreateMenuDto dto,
        CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var response = await _api.PostAsJsonAsync(
            "/api/menus", dto, ct);

        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError("", "Menü oluşturulamadı.");
            return View(dto);
        }

        return RedirectToAction(nameof(Index));
    }

    // EDIT (GET)
    public async Task<IActionResult> Edit(Guid id, CancellationToken ct)
    {
        var item = await _api.GetFromJsonAsync<MenuDto>(
            $"/api/menus/{id}", ct);

        if (item == null)
            return NotFound();

        return View(new UpdateMenuDto
        {
            Id = item.Id,
            Title = item.Title,
            Order = item.Order
        });
    }

    // EDIT (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        UpdateMenuDto dto,
        CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var response = await _api.PutAsJsonAsync(
            $"/api/menus/{dto.Id}", dto, ct);

        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError("", "Menü güncellenemedi.");
            return View(dto);
        }

        return RedirectToAction(nameof(Index));
    }

    // ACTIVATE
    [HttpPost]
    public async Task<IActionResult> Activate(Guid id, CancellationToken ct)
    {
        await _api.PutAsync($"/api/menus/{id}/activate", null, ct);
        return RedirectToAction(nameof(Index));
    }

    // DEACTIVATE
    [HttpPost]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken ct)
    {
        await _api.PutAsync($"/api/menus/{id}/deactivate", null, ct);
        return RedirectToAction(nameof(Index));
    }
}

