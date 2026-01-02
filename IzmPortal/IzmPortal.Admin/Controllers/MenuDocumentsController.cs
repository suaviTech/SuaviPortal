using IzmPortal.Application.DTOs.Menu;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IzmPortal.Admin.Controllers;

[Authorize]
public class MenuDocumentsController : Controller
{
    private readonly HttpClient _api;

    public MenuDocumentsController(IHttpClientFactory factory)
    {
        _api = factory.CreateClient("ApiClient");
    }

    // LIST
    // /MenuDocuments?subMenuId=xxx
    public async Task<IActionResult> Index(Guid subMenuId, CancellationToken ct)
    {
        var items = await _api.GetFromJsonAsync<List<MenuDocumentDto>>(
            $"/api/menudocuments/by-submenu/{subMenuId}", ct);

        ViewBag.SubMenuId = subMenuId;
        return View(items);
    }

    // UPLOAD (GET)
    public IActionResult Upload(Guid subMenuId)
    {
        ViewBag.SubMenuId = subMenuId;
        return View();
    }

    // UPLOAD (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upload(
        Guid subMenuId,
        string title,
        IFormFile file,
        CancellationToken ct)
    {
        if (file == null || file.Length == 0)
        {
            ModelState.AddModelError("", "Dosya seçilmedi.");
            ViewBag.SubMenuId = subMenuId;
            return View();
        }

        using var content = new MultipartFormDataContent();
        content.Add(new StringContent(subMenuId.ToString()), "SubMenuId");
        content.Add(new StringContent(title), "Title");
        content.Add(new StreamContent(file.OpenReadStream()), "File", file.FileName);

        var response = await _api.PostAsync(
            "/api/menudocuments", content, ct);

        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError("", "Dosya yüklenemedi.");
            ViewBag.SubMenuId = subMenuId;
            return View();
        }

        return RedirectToAction(nameof(Index), new { subMenuId });
    }

    // DELETE
    [HttpPost]
    public async Task<IActionResult> Delete(Guid id, Guid subMenuId, CancellationToken ct)
    {
        await _api.DeleteAsync($"/api/menudocuments/{id}", ct);
        return RedirectToAction(nameof(Index), new { subMenuId });
    }
}
