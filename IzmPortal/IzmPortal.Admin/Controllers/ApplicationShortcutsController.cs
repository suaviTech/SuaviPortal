using IzmPortal.Application.DTOs.ApplicationShortcut;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IzmPortal.Admin.Controllers;

[Authorize]
public class ApplicationShortcutsController : Controller
{
    private readonly HttpClient _api;

    public ApplicationShortcutsController(IHttpClientFactory factory)
    {
        _api = factory.CreateClient("ApiClient");
    }

    public async Task<IActionResult> Index()
    {
        var response = await _api.GetAsync("/api/application-shortcuts/admin");

        // 🔐 TOKEN SÜRESİ DOLMUŞ / YETKİ YOK
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            // ApiAuthHandler zaten logout + redirect yaptı
            return new EmptyResult(); // pipeline bozulmasın
        }

        if (!response.IsSuccessStatusCode)
        {
            ViewBag.Error = "Veriler alınamadı";
            return View(new List<ApplicationShortcutAdminDto>());
        }

        var items = await response.Content
            .ReadFromJsonAsync<List<ApplicationShortcutAdminDto>>();

        return View(items ?? new());
    }

    // CREATE
    public IActionResult Create() => View();

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateUpdateApplicationShortcutDto dto)
    {
        if (!ModelState.IsValid) return View(dto);

        var res = await _api.PostAsJsonAsync("/api/application-shortcuts", dto);
        if (!res.IsSuccessStatusCode)
        {
            ModelState.AddModelError("", "Kayıt oluşturulamadı.");
            return View(dto);
        }
        return RedirectToAction(nameof(Index));
    }

    // EDIT
    public async Task<IActionResult> Edit(Guid id)
    {
        var list = await _api.GetFromJsonAsync<List<ApplicationShortcutAdminDto>>(
            "/api/application-shortcuts/admin");
        var item = list?.FirstOrDefault(x => x.Id == id);
        if (item == null) return NotFound();

        ViewBag.Id = id;
        return View(new CreateUpdateApplicationShortcutDto
        {
            Title = item.Title,
            Icon = item.Icon,
            Url = item.Url,
            IsExternal = item.IsExternal,
            IsActive = item.IsActive
        });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, CreateUpdateApplicationShortcutDto dto)
    {
        if (!ModelState.IsValid) return View(dto);

        var res = await _api.PutAsJsonAsync($"/api/application-shortcuts/{id}", dto);
        if (!res.IsSuccessStatusCode)
        {
            ModelState.AddModelError("", "Güncelleme yapılamadı.");
            return View(dto);
        }
        return RedirectToAction(nameof(Index));
    }

    // ACTIVATE / DEACTIVATE
    [HttpPost]
    public async Task<IActionResult> Activate(Guid id)
    {
        await _api.PutAsync($"/api/application-shortcuts/{id}/activate", null);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        await _api.PutAsync($"/api/application-shortcuts/{id}/deactivate", null);
        return RedirectToAction(nameof(Index));
    }

    // DELETE
    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _api.DeleteAsync($"/api/application-shortcuts/{id}");
        return RedirectToAction(nameof(Index));
    }
}
