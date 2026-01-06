using IzmPortal.Admin.Extensions;
using IzmPortal.Application.DTOs.Announcement;
using IzmPortal.Application.DTOs.Category;
using Microsoft.AspNetCore.Mvc;

namespace IzmPortal.Admin.Controllers;

public class AnnouncementsController : BaseAdminController
{
    public AnnouncementsController(IHttpClientFactory factory)
        : base(factory)
    {
    }

    // =======================
    // LIST
    // =======================
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var response = await Api.GetAsync("/api/announcements", ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Duyurular alınamadı.");

        if (failure != null)
            return failure;

        var items = await response
            .ReadContentAsync<List<AnnouncementDto>>() ?? new();

        return View(items);
    }

    // =======================
    // CREATE (GET)
    // =======================
    public async Task<IActionResult> Create(CancellationToken ct)
    {
        var categories = await LoadCategoriesAsync(ct);
        if (categories.failure != null)
            return categories.failure;

        ViewBag.Categories = categories.items;
        return View();
    }

    // =======================
    // CREATE (POST)
    // =======================
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        CreateAnnouncementDto dto,
        IFormFile? pdfFile,
        CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            var categories = await LoadCategoriesAsync(ct);
            ViewBag.Categories = categories.items;
            return View(dto);
        }

        using var content = BuildMultipart(dto, pdfFile);

        var response = await Api.PostAsync(
            "/api/announcements",
            content,
            ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Duyuru oluşturulamadı.");

        if (failure != null)
            return failure;

        return SuccessAndRedirect(
            "Duyuru başarıyla oluşturuldu.");
    }

    // =======================
    // EDIT (GET)
    // =======================
    public async Task<IActionResult> Edit(Guid id, CancellationToken ct)
    {
        var response = await Api.GetAsync(
            $"/api/announcements/{id}", ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Duyuru bilgileri alınamadı.");

        if (failure != null)
            return failure;

        var item = await response
            .ReadContentAsync<AnnouncementDto>();

        if (item == null)
            return SuccessAndRedirect(
                "Duyuru bulunamadı.",
                controller: "Announcements");

        var categories = await LoadCategoriesAsync(ct);
        if (categories.failure != null)
            return categories.failure;

        ViewBag.Categories = categories.items;

        return View(new UpdateAnnouncementDto
        {
            Id = item.Id,
            Title = item.Title,
            Content = item.Content,
            CategoryId = item.CategoryId,
            IsActive = item.IsActive,
            ExistingPdfUrl = item.PdfUrl
        });
    }

    // =======================
    // EDIT (POST)
    // =======================
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        UpdateAnnouncementDto dto,
        IFormFile? pdfFile,
        CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            var categories = await LoadCategoriesAsync(ct);
            ViewBag.Categories = categories.items;
            return View(dto);
        }

        using var content = BuildMultipart(dto, pdfFile);

        var response = await Api.PutAsync(
            $"/api/announcements/{dto.Id}",
            content,
            ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Duyuru güncellenemedi.");

        if (failure != null)
            return failure;

        return SuccessAndRedirect(
            "Duyuru başarıyla güncellendi.");
    }

    // =======================
    // ACTIVATE
    // =======================
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Activate(Guid id, CancellationToken ct)
    {
        var response = await Api.PutAsync(
            $"/api/announcements/{id}/activate",
            null,
            ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Duyuru aktifleştirilemedi.");

        if (failure != null)
            return failure;

        return SuccessAndRedirect(
            "Duyuru aktifleştirildi.");
    }

    // =======================
    // DEACTIVATE
    // =======================
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken ct)
    {
        var response = await Api.PutAsync(
            $"/api/announcements/{id}/deactivate",
            null,
            ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Duyuru pasifleştirilemedi.");

        if (failure != null)
            return failure;

        return SuccessAndRedirect(
            "Duyuru pasifleştirildi.");
    }

    // =======================
    // DELETE
    // =======================
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var response = await Api.DeleteAsync(
            $"/api/announcements/{id}",
            ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Duyuru silinemedi.");

        if (failure != null)
            return failure;

        return SuccessAndRedirect(
            "Duyuru silindi.");
    }

    // =======================
    // HELPERS
    // =======================
    private async Task<(List<CategoryDto> items, IActionResult? failure)>
        LoadCategoriesAsync(CancellationToken ct)
    {
        var response = await Api.GetAsync("/api/categories", ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Kategoriler alınamadı.");

        if (failure != null)
            return (new List<CategoryDto>(), failure);

        var items = await response
            .ReadContentAsync<List<CategoryDto>>() ?? new();

        return (items, null);
    }

    private static MultipartFormDataContent BuildMultipart(
        CreateAnnouncementDto dto,
        IFormFile? pdfFile)
    {
        var content = new MultipartFormDataContent();

        content.Add(new StringContent(dto.Title), nameof(dto.Title));
        content.Add(new StringContent(dto.Content), nameof(dto.Content));
        content.Add(new StringContent(dto.CategoryId.ToString()), nameof(dto.CategoryId));

        if (pdfFile != null && pdfFile.Length > 0)
        {
            content.Add(
                new StreamContent(pdfFile.OpenReadStream()),
                "pdfFile",
                pdfFile.FileName);
        }

        return content;
    }

    private static MultipartFormDataContent BuildMultipart(
        UpdateAnnouncementDto dto,
        IFormFile? pdfFile)
    {
        var content = new MultipartFormDataContent();

        content.Add(new StringContent(dto.Id.ToString()), nameof(dto.Id));
        content.Add(new StringContent(dto.Title), nameof(dto.Title));
        content.Add(new StringContent(dto.Content), nameof(dto.Content));
        content.Add(new StringContent(dto.CategoryId.ToString()), nameof(dto.CategoryId));
        content.Add(new StringContent(dto.IsActive.ToString()), nameof(dto.IsActive));
        content.Add(new StringContent(dto.ExistingPdfUrl ?? ""), nameof(dto.ExistingPdfUrl));

        if (pdfFile != null && pdfFile.Length > 0)
        {
            content.Add(
                new StreamContent(pdfFile.OpenReadStream()),
                "pdfFile",
                pdfFile.FileName);
        }

        return content;
    }
}
