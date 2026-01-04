using IzmPortal.Admin.Extensions;
using IzmPortal.Application.DTOs.MenuDocument;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IzmPortal.Admin.Controllers;

[Authorize]
public class MenuDocumentsController : BaseAdminController
{
    public MenuDocumentsController(IHttpClientFactory factory)
        : base(factory) { }

    // LIST
    public async Task<IActionResult> Index(Guid subMenuId, CancellationToken ct)
    {
        var response = await Api.GetAsync(
            $"/api/menudocuments/by-submenu/{subMenuId}", ct);

        var failure = await HandleApiFailureAsync(
            response, "Dokümanlar alınamadı.");

        if (failure != null)
            return failure;

        var items = await response
            .ReadContentAsync<List<MenuDocumentDto>>() ?? new();

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
    [HttpPost, ValidateAntiForgeryToken]
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
        content.Add(new StringContent(title ?? ""), "Title");
        content.Add(new StreamContent(file.OpenReadStream()), "File", file.FileName);

        var response = await Api.PostAsync("/api/menudocuments", content, ct);

        var failure = await HandleApiFailureAsync(
            response, "Doküman yüklenemedi.");

        if (failure != null)
            return failure;

        return SuccessAndRedirect("Doküman yüklendi.");
    }

    // DELETE
    [HttpPost]
    public async Task<IActionResult> Delete(Guid id, Guid subMenuId, CancellationToken ct)
    {
        var response = await Api.DeleteAsync(
            $"/api/menudocuments/{id}", ct);

        var failure = await HandleApiFailureAsync(
            response, "Doküman silinemedi.");

        if (failure != null)
            return failure;

        return RedirectToAction(nameof(Index), new { subMenuId });
    }
}
