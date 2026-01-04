using IzmPortal.Admin.Extensions;
using IzmPortal.Application.DTOs.Announcement;
using IzmPortal.Application.DTOs.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IzmPortal.Admin.Controllers;

[Authorize]
public class AnnouncementsController : BaseAdminController
{
    public AnnouncementsController(IHttpClientFactory factory)
        : base(factory)
    {
    }

    // --------------------------------------------------
    // LIST
    // GET: /Announcements
    // --------------------------------------------------
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

    // --------------------------------------------------
    // CREATE (GET)
    // --------------------------------------------------
    public async Task<IActionResult> Create(CancellationToken ct)
    {
        var categoryResponse = await Api.GetAsync("/api/categories", ct);

        var failure = await HandleApiFailureAsync(
            categoryResponse,
            "Kategoriler alınamadı.");

        if (failure != null)
            return failure;

        ViewBag.Categories = await categoryResponse
            .ReadContentAsync<List<CategoryDto>>() ?? new();

        return View();
    }

    // --------------------------------------------------
    // CREATE (POST)
    // --------------------------------------------------
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        CreateAnnouncementDto dto,
        CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var response = await Api.PostAsJsonAsync(
            "/api/announcements", dto, ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Duyuru oluşturulamadı.");

        if (failure != null)
            return failure;

        return SuccessAndRedirect(
            "Duyuru başarıyla oluşturuldu.");
    }

    // --------------------------------------------------
    // EDIT (GET)
    // --------------------------------------------------
    public async Task<IActionResult> Edit(Guid id, CancellationToken ct)
    {
        var response = await Api.GetAsync(
            $"/api/announcements/{id}", ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Duyuru bulunamadı.");

        if (failure != null)
            return failure;

        var item = await response
            .ReadContentAsync<AnnouncementDto>();

        if (item == null)
            return NotFound();

        var categoryResponse = await Api.GetAsync("/api/categories", ct);

        failure = await HandleApiFailureAsync(
            categoryResponse,
            "Kategoriler alınamadı.");

        if (failure != null)
            return failure;

        ViewBag.Categories = await categoryResponse
            .ReadContentAsync<List<CategoryDto>>() ?? new();

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
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        UpdateAnnouncementDto dto,
        CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var response = await Api.PutAsJsonAsync(
            $"/api/announcements/{dto.Id}", dto, ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Duyuru güncellenemedi.");

        if (failure != null)
            return failure;

        return SuccessAndRedirect(
            "Duyuru başarıyla güncellendi.");
    }

    // --------------------------------------------------
    // ACTIVATE
    // --------------------------------------------------
    [HttpPost]
    public async Task<IActionResult> Activate(Guid id, CancellationToken ct)
    {
        var response = await Api.PutAsync(
            $"/api/announcements/{id}/activate", null, ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Duyuru aktifleştirilemedi.");

        if (failure != null)
            return failure;

        return SuccessAndRedirect(
            "Duyuru aktifleştirildi.");
    }

    // --------------------------------------------------
    // DEACTIVATE
    // --------------------------------------------------
    [HttpPost]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken ct)
    {
        var response = await Api.PutAsync(
            $"/api/announcements/{id}/deactivate", null, ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Duyuru pasifleştirilemedi.");

        if (failure != null)
            return failure;

        return SuccessAndRedirect(
            "Duyuru pasifleştirildi.");
    }

    // --------------------------------------------------
    // DETAILS (READ-ONLY PREVIEW)
    // --------------------------------------------------
    public async Task<IActionResult> Details(Guid id, CancellationToken ct)
    {
        var response = await Api.GetAsync(
            $"/api/announcements/{id}", ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Duyuru bulunamadı.");

        if (failure != null)
            return failure;

        var item = await response
            .ReadContentAsync<AnnouncementDto>();

        if (item == null)
            return NotFound();

        return View(item);
    }

}
