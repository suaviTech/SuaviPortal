using IzmPortal.Admin.Extensions;
using IzmPortal.Application.DTOs.ApplicationShortcut;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IzmPortal.Admin.Controllers;

[Authorize]
public class ApplicationShortcutsController : BaseAdminController
{
    public ApplicationShortcutsController(IHttpClientFactory factory)
        : base(factory)
    {
    }

    public async Task<IActionResult> Index()
    {
        var response = await Api.GetAsync("/api/application-shortcuts/admin");

        var failure = await HandleApiFailureAsync(
            response,
            "Uygulama kısayolları alınamadı.");

        if (failure != null)
            return failure;

        var items = await response
            .ReadContentAsync<List<ApplicationShortcutAdminDto>>() ?? new();

        return View(items);
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        CreateApplicationShortcutDto dto,
        CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var response = await Api.PostAsJsonAsync(
            "/api/applicationshortcuts", dto, ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Kısayol oluşturulamadı.");

        if (failure != null)
            return failure;

        return SuccessAndRedirect("Kısayol oluşturuldu.");
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
       UpdateApplicationShortcutDto dto,
       CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var response = await Api.PutAsJsonAsync(
            $"/api/applicationshortcuts/{dto.Id}", dto, ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Kısayol güncellenemedi.");

        if (failure != null)
            return failure;

        return SuccessAndRedirect("Kısayol güncellendi.");
    }


    [HttpPost]
    public async Task<IActionResult> Activate(Guid id)
    {
        var response = await Api.PutAsync(
            $"/api/application-shortcuts/{id}/activate", null);

        var failure = await HandleApiFailureAsync(
            response,
            "Uygulama kısayolu aktifleştirilemedi.");

        if (failure != null)
            return failure;

        return SuccessAndRedirect(
            "Uygulama kısayolu aktifleştirildi.");
    }

    [HttpPost]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        var response = await Api.PutAsync(
            $"/api/application-shortcuts/{id}/deactivate", null);

        var failure = await HandleApiFailureAsync(
            response,
            "Uygulama kısayolu pasifleştirilemedi.");

        if (failure != null)
            return failure;

        return SuccessAndRedirect(
            "Uygulama kısayolu pasifleştirildi.");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await Api.DeleteAsync(
            $"/api/application-shortcuts/{id}");

        var failure = await HandleApiFailureAsync(
            response,
            "Uygulama kısayolu silinemedi.");

        if (failure != null)
            return failure;

        return SuccessAndRedirect(
            "Uygulama kısayolu silindi.");
    }
}
