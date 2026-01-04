using IzmPortal.Admin.Extensions;
using IzmPortal.Application.DTOs.SubMenu;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IzmPortal.Admin.Controllers;

[Authorize]
public class SubMenusController : BaseAdminController
{
    public SubMenusController(IHttpClientFactory factory)
        : base(factory)
    {
    }

    // --------------------------------------------------
    // LIST
    // /SubMenus?menuId=xxx
    // --------------------------------------------------
    public async Task<IActionResult> Index(Guid menuId, CancellationToken ct)
    {
        var response = await Api.GetAsync(
            $"/api/submenus/by-menu/{menuId}", ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Alt menüler alınamadı.");

        if (failure != null)
            return failure;

        var items = await response
            .ReadContentAsync<List<SubMenuDto>>() ?? new();

        ViewBag.MenuId = menuId;

        return View(items);
    }

    // --------------------------------------------------
    // CREATE (GET)
    // --------------------------------------------------
    public IActionResult Create(Guid menuId)
    {
        ViewBag.MenuId = menuId;
        return View();
    }

    // --------------------------------------------------
    // CREATE (POST)
    // --------------------------------------------------
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        CreateSubMenuDto dto,
        CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.MenuId = dto.MenuId;
            return View(dto);
        }

        var response = await Api.PostAsJsonAsync(
            "/api/submenus", dto, ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Alt menü oluşturulamadı.");

        if (failure != null)
            return failure;

        return SuccessAndRedirect("Alt menü başarıyla oluşturuldu.");
    }

    // --------------------------------------------------
    // EDIT (GET)
    // --------------------------------------------------
    public async Task<IActionResult> Edit(
        Guid id,
        Guid menuId,
        CancellationToken ct)
    {
        var response = await Api.GetAsync(
            $"/api/submenus/{id}", ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Alt menü bulunamadı.");

        if (failure != null)
            return failure;

        var item = await response
            .ReadContentAsync<SubMenuDto>();

        if (item == null)
            return NotFound();

        ViewBag.MenuId = menuId;

        return View(new UpdateSubMenuDto
        {
            Id = item.Id,
            Title = item.Title,
            Order = item.Order
        });
    }

    // --------------------------------------------------
    // EDIT (POST)
    // --------------------------------------------------
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        UpdateSubMenuDto dto,
        Guid menuId,
        CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.MenuId = menuId;
            return View(dto);
        }

        var response = await Api.PutAsJsonAsync(
            $"/api/submenus/{dto.Id}", dto, ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Alt menü güncellenemedi.");

        if (failure != null)
            return failure;

        return SuccessAndRedirect("Alt menü başarıyla güncellendi.");
    }

    // --------------------------------------------------
    // ACTIVATE
    // --------------------------------------------------
    [HttpPost]
    public async Task<IActionResult> Activate(
        Guid id,
        Guid menuId,
        CancellationToken ct)
    {
        var response = await Api.PutAsync(
            $"/api/submenus/{id}/activate", null, ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Alt menü aktifleştirilemedi.");

        if (failure != null)
            return failure;

        return SuccessAndRedirect("Alt menü aktifleştirildi.");
    }

    // --------------------------------------------------
    // DEACTIVATE
    // --------------------------------------------------
    [HttpPost]
    public async Task<IActionResult> Deactivate(
        Guid id,
        Guid menuId,
        CancellationToken ct)
    {
        var response = await Api.PutAsync(
            $"/api/submenus/{id}/deactivate", null, ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Alt menü pasifleştirilemedi.");

        if (failure != null)
            return failure;

        return SuccessAndRedirect("Alt menü pasifleştirildi.");
    }
}
