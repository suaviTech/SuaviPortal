using IzmPortal.Application.DTOs.Announcement;
using IzmPortal.Application.DTOs.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IzmPortal.Admin.Controllers;

[Authorize]
public class AnnouncementsController : Controller
{
    private readonly HttpClient _api;

    public AnnouncementsController(IHttpClientFactory factory)
    {
        _api = factory.CreateClient("ApiClient");
    }

    // --------------------------------------------------
    // LIST
    // GET: /Announcements
    // --------------------------------------------------
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var items = await _api.GetFromJsonAsync<List<AnnouncementDto>>(
            "/api/announcements", ct);

        return View(items);
    }

    // --------------------------------------------------
    // CREATE
    // GET: /Announcements/Create
    // --------------------------------------------------
    public async Task<IActionResult> Create(CancellationToken ct)
    {
        ViewBag.Categories = await _api.GetFromJsonAsync<List<CategoryDto>>(
            "/api/categories", ct);

        return View();
    }

    // --------------------------------------------------
    // CREATE (POST)
    // --------------------------------------------------
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        CreateAnnouncementDto dto,
        CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = await _api.GetFromJsonAsync<List<CategoryDto>>(
                "/api/categories", ct);
            return View(dto);
        }

        var response = await _api.PostAsJsonAsync(
            "/api/announcements", dto, ct);

        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError("", "Duyuru oluşturulamadı.");
            ViewBag.Categories = await _api.GetFromJsonAsync<List<CategoryDto>>(
                "/api/categories", ct);
            return View(dto);
        }

        return RedirectToAction(nameof(Index));
    }

    // --------------------------------------------------
    // EDIT
    // GET: /Announcements/Edit/{id}
    // --------------------------------------------------
    public async Task<IActionResult> Edit(Guid id, CancellationToken ct)
    {
        var item = await _api.GetFromJsonAsync<AnnouncementDto>(
            $"/api/announcements/{id}", ct);

        if (item == null)
            return NotFound();

        ViewBag.Categories = await _api.GetFromJsonAsync<List<CategoryDto>>(
            "/api/categories", ct);

        return View(new UpdateAnnouncementDto
        {
            Id = item.Id,
            Title = item.Title,
            Content = item.Content,
            CategoryId = item.CategoryId,
            IsActive = item.IsActive
        });
    }

    // --------------------------------------------------
    // EDIT (POST)
    // --------------------------------------------------
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        UpdateAnnouncementDto dto,
        CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = await _api.GetFromJsonAsync<List<CategoryDto>>(
                "/api/categories", ct);
            return View(dto);
        }

        var response = await _api.PutAsJsonAsync(
            $"/api/announcements/{dto.Id}", dto, ct);

        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError("", "Duyuru güncellenemedi.");
            ViewBag.Categories = await _api.GetFromJsonAsync<List<CategoryDto>>(
                "/api/categories", ct);
            return View(dto);
        }

        return RedirectToAction(nameof(Index));
    }

    // --------------------------------------------------
    // ACTIVATE
    // POST: /Announcements/Activate/{id}
    // --------------------------------------------------
    [HttpPost]
    public async Task<IActionResult> Activate(Guid id, CancellationToken ct)
    {
        await _api.PutAsync(
            $"/api/announcements/{id}/activate", null, ct);

        return RedirectToAction(nameof(Index));
    }

    // --------------------------------------------------
    // DEACTIVATE
    // POST: /Announcements/Deactivate/{id}
    // --------------------------------------------------
    [HttpPost]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken ct)
    {
        await _api.PutAsync(
            $"/api/announcements/{id}/deactivate", null, ct);

        return RedirectToAction(nameof(Index));
    }
}
