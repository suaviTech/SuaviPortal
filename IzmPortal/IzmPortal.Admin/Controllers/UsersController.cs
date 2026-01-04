using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using IzmPortal.Admin.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IzmPortal.Admin.Controllers;

[Authorize(Roles = "SuperAdmin")]
public class UsersController : Controller
{
   
    private readonly HttpClient _apiClient;
    public UsersController(IHttpClientFactory httpClientFactory)
    {
        _apiClient = httpClientFactory.CreateClient("ApiClient");
    }

    // --------------------
    // INDEX
    // --------------------
    public async Task<IActionResult> Index()
    {
        var client = _apiClient;
        var response = await client.GetAsync("/api/users");

        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "Kullanıcılar alınamadı.";
            return View(new UsersIndexVm());
        }

        var users =
            JsonSerializer.Deserialize<List<UserListVm>>(
                await response.Content.ReadAsStringAsync(),
                JsonOptions());

        return View(new UsersIndexVm
        {
            Users = users ?? new()
        });
    }

    // --------------------
    // LOOKUP (API)
    // --------------------
    [HttpPost]
    public async Task<IActionResult> Lookup(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            TempData["Error"] = "Email boş olamaz.";
            return RedirectToAction(nameof(Index));
        }

        var client = _apiClient;
        var response = await client.GetAsync(
            $"/api/users/lookup?email={email}");

        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "Kullanıcı sorgulanamadı.";
            return RedirectToAction(nameof(Index));
        }

        var lookup =
            JsonSerializer.Deserialize<UserLookupResultVm>(
                await response.Content.ReadAsStringAsync(),
                JsonOptions());

        TempData["Lookup"] = JsonSerializer.Serialize(lookup);
        TempData["LookupEmail"] = email;

        return RedirectToAction(nameof(Index));
    }

    // --------------------
    // AUTHORIZE FROM PERSONAL
    // --------------------
    [HttpPost]
    public async Task<IActionResult> AuthorizeFromPersonal(
        string email,
        string role)
    {
        var client = _apiClient;

        var body = new
        {
            email,
            role
        };

        var response = await client.PostAsync(
            "/api/users/authorize-from-personal",
            new StringContent(
                JsonSerializer.Serialize(body),
                Encoding.UTF8,
                "application/json"));

        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] =
                await response.Content.ReadAsStringAsync();
            return RedirectToAction(nameof(Index));
        }

        TempData["Success"] = "Kullanıcı yetkilendirildi.";
        return RedirectToAction(nameof(Index));
    }
    [HttpPost]
    public async Task<IActionResult> ChangeRole(string userId, string role)
    {
        var response = await _apiClient.PostAsJsonAsync(
            "/api/users/change-role",
            new
            {
                UserId = userId,
                Role = role
            });

        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "Rol değiştirilemedi.";
            return RedirectToAction(nameof(Index));
        }

        TempData["Success"] = "Rol başarıyla güncellendi.";
        return RedirectToAction(nameof(Index));
    }

    // --------------------
    // HELPERS
    // --------------------

    private static JsonSerializerOptions JsonOptions()
        => new() { PropertyNameCaseInsensitive = true };

    //ResetPassword action EKLE

    [HttpPost]
    public async Task<IActionResult> ResetPassword(string userId)
    {
        // Varsayılan geçici şifre (kurum standardı)
        var tempPassword = "Temp123*"; // istersen konfigürasyona alırız

        var response = await _apiClient.PostAsJsonAsync(
            "/api/users/reset-password",
            new
            {
                UserId = userId,
                NewPassword = tempPassword
            });

        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "Şifre sıfırlanamadı.";
            return RedirectToAction(nameof(Index));
        }

        TempData["Success"] = "Şifre sıfırlandı. Kullanıcı ilk girişte şifre değiştirecek.";
        return RedirectToAction(nameof(Index));
    }

}
