using IzmPortal.Application.DTOs.Slider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IzmPortal.Admin.Controllers;

[Authorize]
public class SlidersController : Controller
{
    private readonly HttpClient _api;

    public SlidersController(IHttpClientFactory factory)
    {
        _api = factory.CreateClient("ApiClient");
    }

    // LIST
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var sliders = await _api.GetFromJsonAsync<List<SliderDto>>(
            "/api/sliders", ct);

        return View(sliders);
    }

    // UPLOAD (GET)
    public IActionResult Upload()
    {
        return View();
    }

    // UPLOAD (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upload(
        IFormFile file,
        CancellationToken ct)
    {
        if (file == null || file.Length == 0)
        {
            ModelState.AddModelError("", "Dosya seçilmedi.");
            return View();
        }

        using var content = new MultipartFormDataContent();
        content.Add(new StreamContent(file.OpenReadStream()), "file", file.FileName);

        var response = await _api.PostAsync(
            "/api/sliders", content, ct);

        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError("", "Slider yüklenemedi.");
            return View();
        }

        return RedirectToAction(nameof(Index));
    }

    // DELETE
    [HttpPost]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _api.DeleteAsync($"/api/sliders/{id}", ct);
        return RedirectToAction(nameof(Index));
    }
}

