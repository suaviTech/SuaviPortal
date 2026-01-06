using IzmPortal.Admin.Extensions;
using IzmPortal.Application.DTOs.ApplicationShortcut;
using Microsoft.AspNetCore.Mvc;

namespace IzmPortal.Admin.Controllers;

public class ApplicationShortcutsController : BaseAdminController
{
    public ApplicationShortcutsController(IHttpClientFactory factory)
        : base(factory)
    {
    }

    // =======================
    // LIST
    // =======================
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var response = await Api.GetAsync(
            "/api/application-shortcuts/admin",
            ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Uygulama kısayolları alınamadı.");

        if (failure != null)
            return failure;

        var items = await response
            .ReadContentAsync<List<ApplicationShortcutAdminDto>>() ?? new();

        return View(items);
    }

    // =======================
    // CREATE (GET)
    // =======================
    public IActionResult Create()
    {
        return View();
    }

    // =======================
    // CREATE (POST)
    // =======================
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        CreateApplicationShortcutDto dto,
        CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var response = await Api.PostAsJsonAsync(
            "/api/application-shortcuts",
            dto,
            ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Kısayol oluşturulamadı.");

        if (failure != null)
            return failure;

        return SuccessAndRedirect(
            "Kısayol oluşturuldu.");
    }

    // =======================
    // EDIT (POST)
    // =======================
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        UpdateApplicationShortcutDto dto,
        CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var response = await Api.PutAsJsonAsync(
            $"/api/application-shortcuts/{dto.Id}",
            dto,
            ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Kısayol güncellenemedi.");

        if (failure != null)
            return failure;

        return SuccessAndRedirect(
            "Kısayol güncellendi.");
    }

    // =======================
    // ACTIVATE
    // =======================
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Activate(Guid id, CancellationToken ct)
    {
        var response = await Api.PutAsync(
            $"/api/application-shortcuts/{id}/activate",
            null,
            ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Uygulama kısayolu aktifleştirilemedi.");

        if (failure != null)
            return failure;

        return SuccessAndRedirect(
            "Uygulama kısayolu aktifleştirildi.");
    }

    // =======================
    // DEACTIVATE
    // =======================
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken ct)
    {
        var response = await Api.PutAsync(
            $"/api/application-shortcuts/{id}/deactivate",
            null,
            ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Uygulama kısayolu pasifleştirilemedi.");

        if (failure != null)
            return failure;

        return SuccessAndRedirect(
            "Uygulama kısayolu pasifleştirildi.");
    }

    // =======================
    // DELETE
    // =======================
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var response = await Api.DeleteAsync(
            $"/api/application-shortcuts/{id}",
            ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Uygulama kısayolu silinemedi.");

        if (failure != null)
            return failure;

        return SuccessAndRedirect(
            "Uygulama kısayolu silindi.");
    }
}
