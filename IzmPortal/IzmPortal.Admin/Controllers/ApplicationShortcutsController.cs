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

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateUpdateApplicationShortcutDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var response = await Api.PostAsJsonAsync(
            "/api/application-shortcuts", dto);

        var failure = await HandleApiFailureAsync(
            response,
            "Uygulama kısayolu oluşturulamadı.");

        if (failure != null)
            return failure;

        return SuccessAndRedirect(
            "Uygulama kısayolu başarıyla oluşturuldu.");
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var response = await Api.GetAsync(
            $"/api/application-shortcuts/admin/{id}");

        var failure = await HandleApiFailureAsync(
            response,
            "Kayıt bulunamadı.");

        if (failure != null)
            return failure;

        var item = await response
            .ReadContentAsync<ApplicationShortcutAdminDto>();

        if (item == null)
            return NotFound();

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
        if (!ModelState.IsValid)
            return View(dto);

        var response = await Api.PutAsJsonAsync(
            $"/api/application-shortcuts/{id}", dto);

        var failure = await HandleApiFailureAsync(
            response,
            "Uygulama kısayolu güncellenemedi.");

        if (failure != null)
            return failure;

        return SuccessAndRedirect(
            "Uygulama kısayolu başarıyla güncellendi.");
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
