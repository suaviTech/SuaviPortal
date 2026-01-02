using IzmPortal.Admin.Extensions;
using IzmPortal.Admin.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IzmPortal.Admin.Controllers;

[Authorize(Roles = "SuperAdmin")]
public class UsersController : BaseAdminController
{
    public UsersController(IHttpClientFactory factory)
        : base(factory)
    {
    }

    public async Task<IActionResult> Index()
    {
        var response = await Api.GetAsync("/api/users");

        var failure = await HandleApiFailureAsync(
            response,
            "Kullanıcılar alınamadı.");

        if (failure != null)
            return failure;

        var users = await response
            .ReadContentAsync<List<UserAdminVm>>() ?? new();

        return View(users);
    }

    [HttpPost]
    public async Task<IActionResult> Activate(string id)
    {
        var response = await Api.PostAsync(
            $"/api/users/{id}/activate", null);

        var failure = await HandleApiFailureAsync(
            response,
            "Kullanıcı aktifleştirilemedi.");

        if (failure != null)
            return failure;

        return SuccessAndRedirect("Kullanıcı aktifleştirildi.");
    }

    [HttpPost]
    public async Task<IActionResult> Deactivate(string id)
    {
        var response = await Api.PostAsync(
            $"/api/users/{id}/deactivate", null);

        var failure = await HandleApiFailureAsync(
            response,
            "Kullanıcı pasifleştirilemedi.");

        if (failure != null)
            return failure;

        return SuccessAndRedirect("Kullanıcı pasifleştirildi.");
    }

    [HttpPost]
    public async Task<IActionResult> ChangeRole(string id, string role)
    {
        var response = await Api.PostAsJsonAsync(
            "/api/users/change-role",
            new { UserId = id, Role = role });

        var failure = await HandleApiFailureAsync(
            response,
            "Rol değiştirilemedi.");

        if (failure != null)
            return failure;

        return SuccessAndRedirect("Rol güncellendi.");
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword(string id)
    {
        var response = await Api.PostAsJsonAsync(
            "/api/users/reset-password",
            new { UserId = id, NewPassword = "1234" });

        var failure = await HandleApiFailureAsync(
            response,
            "Şifre resetlenemedi.");

        if (failure != null)
            return failure;

        return SuccessAndRedirect("Şifre resetlendi (1234).");
    }
}
