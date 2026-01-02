using IzmPortal.Application.DTOs.Menu;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IzmPortal.Admin.Controllers;

[Authorize]
public class SubMenusController : Controller
{
    private readonly HttpClient _api;

    public SubMenusController(IHttpClientFactory factory)
    {
        _api = factory.CreateClient("ApiClient");
    }

    // LIST
    // /SubMenus?menuId=xxx
    public async Task<IActionResult> Index(Guid menuId, CancellationToken ct)
    {
        var items = await _api.GetFromJsonAsync<List<SubMenuDto>>(
            $"/api/submenus/by-menu/{menuId}", ct);

        ViewBag.MenuId = menuId;
        return View(items);
    }

    // CREATE (GET)
    public IActionResult Create(Guid menuId)
    {
        ViewBag.MenuId = menuId;
        return View();
    }

    // CREATE (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        CreateSubMenuDto dto,
        CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.MenuId = dto.MenuId;
            return View(dto);
        }

        var response = await _api.PostAsJsonAsync(
            "/api/submenus", dto, ct);

        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError("", "Alt menü oluşturulamadı.");
            ViewBag.MenuId = dto.MenuId;
            return View(dto);
        }

        return RedirectToAction(nameof(Index), new { menuId = dto.MenuId });
    }

    // EDIT (GET)
    public async Task<IActionResult> Edit(Guid id, Guid menuId, CancellationToken ct)
    {
        var item = await _api.GetFromJsonAsync<SubMenuDto>(
            $"/api/submenus/{id}", ct);

        if (item == null)
            return NotFound();

        ViewBag.MenuId = menuId;

        return View(new UpdateSubMenuDto
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
        UpdateSubMenuDto dto,
        Guid menuId,
        CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.MenuId = menuId;
            return View(dto);
        }

        var response = await _api.PutAsJsonAsync(
            $"/api/submenus/{dto.Id}", dto, ct);

        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError("", "Alt menü güncellenemedi.");
            ViewBag.MenuId = menuId;
            return View(dto);
        }

        return RedirectToAction(nameof(Index), new { menuId });
    }

    // ACTIVATE
    [HttpPost]
    public async Task<IActionResult> Activate(Guid id, Guid menuId, CancellationToken ct)
    {
        await _api.PutAsync($"/api/submenus/{id}/activate", null, ct);
        return RedirectToAction(nameof(Index), new { menuId });
    }

    // DEACTIVATE
    [HttpPost]
    public async Task<IActionResult> Deactivate(Guid id, Guid menuId, CancellationToken ct)
    {
        await _api.PutAsync($"/api/submenus/{id}/deactivate", null, ct);
        return RedirectToAction(nameof(Index), new { menuId });
    }
}

