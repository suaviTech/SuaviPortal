using IzmPortal.Admin.Extensions;
using IzmPortal.Application.DTOs.MenuDocument;
using Microsoft.AspNetCore.Mvc;

namespace IzmPortal.Admin.Controllers;

public class MenuDocumentsController : BaseAdminController
{
    public MenuDocumentsController(IHttpClientFactory factory)
        : base(factory)
    {
    }

    // =======================
    // LIST
    // =======================
    public async Task<IActionResult> Index(Guid menuId, CancellationToken ct)
    {
        var response = await Api.GetAsync(
            $"/api/menudocuments/by-menu/{menuId}", ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Dokümanlar alınamadı.");

        if (failure != null)
            return failure;

        var items = await response
            .ReadContentAsync<List<MenuDocumentDto>>() ?? new();

        ViewBag.MenuId = menuId;
        return View(items);
    }

    // =======================
    // UPLOAD (GET)
    // =======================
    public IActionResult Upload(Guid menuId)
    {
        ViewBag.MenuId = menuId;
        return View();
    }

    // =======================
    // UPLOAD (POST)
    // =======================
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Upload(
        CreateMenuDocumentDto dto,
        IFormFile file,
        CancellationToken ct)
    {
        if (!ModelState.IsValid || file == null || file.Length == 0)
        {
            ModelState.AddModelError(
                nameof(file),
                "Dosya seçilmedi.");

            ViewBag.MenuId = dto.MenuId;
            return View(dto);
        }

        using var content = new MultipartFormDataContent();

        content.Add(
            new StringContent(dto.MenuId.ToString()),
            nameof(CreateMenuDocumentDto.MenuId));

        content.Add(
            new StringContent(dto.Title ?? string.Empty),
            nameof(CreateMenuDocumentDto.Title));

        content.Add(
            new StreamContent(file.OpenReadStream()),
            nameof(file),
            file.FileName);

        var response = await Api.PostAsync(
            "/api/menudocuments",
            content,
            ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Doküman yüklenemedi.");

        if (failure != null)
            return failure;

        TempData["Success"] = "Doküman başarıyla yüklendi.";

        return RedirectToAction(
            nameof(Index),
            new { menuId = dto.MenuId });
    }

    // =======================
    // DELETE
    // =======================
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(
        Guid id,
        Guid menuId,
        CancellationToken ct)
    {
        var response = await Api.DeleteAsync(
            $"/api/menudocuments/{id}",
            ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Doküman silinemedi.");

        if (failure != null)
            return failure;

        TempData["Success"] = "Doküman silindi.";

        return RedirectToAction(
            nameof(Index),
            new { menuId });
    }
}
