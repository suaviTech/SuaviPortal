using IzmPortal.Admin.Extensions;
using IzmPortal.Application.DTOs.MenuDocument;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IzmPortal.Admin.Controllers;

[Authorize]
public class MenuDocumentsController : BaseAdminController
{
    public MenuDocumentsController(IHttpClientFactory factory)
        : base(factory)
    {
    }

    // --------------------------------------------------
    // LIST
    // /MenuDocuments?subMenuId=xxx
    // --------------------------------------------------
    public async Task<IActionResult> Index(Guid subMenuId, CancellationToken ct)
    {
        var response = await Api.GetAsync(
            $"/api/menudocuments/by-submenu/{subMenuId}", ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Dokümanlar alınamadı.");

        if (failure != null)
            return failure;

        var items = await response
            .ReadContentAsync<List<MenuDocumentDto>>() ?? new();

        ViewBag.SubMenuId = subMenuId;

        return View(items);
    }

    // --------------------------------------------------
    // UPLOAD (GET)
    // --------------------------------------------------
    public IActionResult Upload(Guid subMenuId)
    {
        ViewBag.SubMenuId = subMenuId;
        return View();
    }

    // --------------------------------------------------
    // UPLOAD (POST)
    // --------------------------------------------------
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
        content.Add(new StringContent(title ?? string.Empty), "Title");
        content.Add(new StreamContent(file.OpenReadStream()), "File", file.FileName);

        var response = await Api.PostAsync(
            "/api/menudocuments", content, ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Dosya yüklenemedi.");

        if (failure != null)
            return failure;

        TempData["Success"] = "Doküman başarıyla yüklendi.";

        return RedirectToAction(nameof(Index), new { subMenuId });
    }

    // --------------------------------------------------
    // DELETE
    // --------------------------------------------------
    [HttpPost]
    public async Task<IActionResult> Delete(
        Guid id,
        Guid subMenuId,
        CancellationToken ct)
    {
        var response = await Api.DeleteAsync(
            $"/api/menudocuments/{id}", ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Doküman silinemedi.");

        if (failure != null)
            return failure;
        return SuccessAndRedirect("Doküman silindi.");

    }
    public async Task<IActionResult> Edit(Guid id, Guid subMenuId, CancellationToken ct)
    {
        var response = await Api.GetAsync(
            $"/api/menudocuments/{id}", ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Doküman bulunamadı.");

        if (failure != null)
            return failure;

        var item = await response
            .ReadContentAsync<MenuDocumentDto>();

        if (item == null)
            return NotFound();

        ViewBag.SubMenuId = subMenuId;

        return View(new UpdateMenuDocumentDto
        {
            Id = item.Id,
            Title = item.Title
        });
    }


    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
    UpdateMenuDocumentDto dto,
    Guid subMenuId,
    CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.SubMenuId = subMenuId;
            return View(dto);
        }

        var response = await Api.PutAsJsonAsync(
            $"/api/menudocuments/{dto.Id}", dto, ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Doküman güncellenemedi.");

        if (failure != null)
            return failure;

        return RedirectToAction(nameof(Index), new { subMenuId });
    }

    [HttpPost]
    public async Task<IActionResult> Activate(
    Guid id,
    Guid subMenuId,
    CancellationToken ct)
    {
        var response = await Api.PutAsync(
            $"/api/menudocuments/{id}/activate", null, ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Doküman aktifleştirilemedi.");

        if (failure != null)
            return failure;

        return RedirectToAction(nameof(Index), new { subMenuId });
    }

    [HttpPost]
    public async Task<IActionResult> Deactivate(
        Guid id,
        Guid subMenuId,
        CancellationToken ct)
    {
        var response = await Api.PutAsync(
            $"/api/menudocuments/{id}/deactivate", null, ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Doküman pasifleştirilemedi.");

        if (failure != null)
            return failure;

        return RedirectToAction(nameof(Index), new { subMenuId });
    }




}
