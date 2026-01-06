using IzmPortal.Admin.Extensions;
using IzmPortal.Admin.Models.Users;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace IzmPortal.Admin.Controllers;

public class UsersController : BaseAdminController
{
    public UsersController(IHttpClientFactory factory)
        : base(factory)
    {
    }

    // =======================
    // INDEX
    // =======================
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var response = await Api.GetAsync(
            "/api/users",
            ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Kullanıcılar alınamadı.");

        if (failure != null)
            return failure;

        var users = await response
            .ReadContentAsync<List<UserListVm>>() ?? new();

        return View(new UsersIndexVm
        {
            Users = users
        });
    }

    // =======================
    // LOOKUP
    // =======================
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Lookup(
        string email,
        CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            TempData["Error"] = "Email boş olamaz.";
            return RedirectToAction(nameof(Index));
        }

        var response = await Api.GetAsync(
            $"/api/users/lookup?email={email}",
            ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Kullanıcı sorgulanamadı.");

        if (failure != null)
            return failure;

        var lookup = await response
            .ReadContentAsync<UserLookupResultVm>();

        TempData["Lookup"] =
            JsonSerializer.Serialize(lookup);

        TempData["LookupEmail"] = email;

        return RedirectToAction(nameof(Index));
    }

    // =======================
    // AUTHORIZE FROM PERSONAL
    // =======================
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> AuthorizeFromPersonal(
        string email,
        string role,
        CancellationToken ct)
    {
        var response = await Api.PostAsJsonAsync(
            "/api/users/authorize-from-personal",
            new { email, role },
            ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Kullanıcı yetkilendirilemedi.");

        if (failure != null)
            return failure;

        return SuccessAndRedirect(
            "Kullanıcı yetkilendirildi.");
    }

    // =======================
    // CHANGE ROLE
    // =======================
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeRole(
        string userId,
        string role,
        CancellationToken ct)
    {
        var response = await Api.PostAsJsonAsync(
            "/api/users/change-role",
            new { userId, role },
            ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Rol değiştirilemedi.");

        if (failure != null)
            return failure;

        return SuccessAndRedirect(
            "Rol başarıyla güncellendi.");
    }

    // =======================
    // RESET PASSWORD
    // =======================
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(
        string userId,
        CancellationToken ct)
    {
        var response = await Api.PostAsJsonAsync(
            "/api/users/reset-password",
            new
            {
                userId,
                newPassword = "Temp123*"
            },
            ct);

        var failure = await HandleApiFailureAsync(
            response,
            "Şifre sıfırlanamadı.");

        if (failure != null)
            return failure;

        return SuccessAndRedirect(
            "Şifre sıfırlandı. Kullanıcı ilk girişte şifre değiştirecek.");
    }
}
