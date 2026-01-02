using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace IzmPortal.Admin.Controllers;

public class AccountController : Controller
{
    private readonly HttpClient _apiClient;

    public AccountController(IHttpClientFactory factory)
    {
        _apiClient = factory.CreateClient("ApiClient");
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        var response = await _apiClient.PostAsJsonAsync(
            "/api/auth/login",
            new { email, password });

        if (!response.IsSuccessStatusCode)
        {
            ViewBag.Error = "Giriş başarısız";
            return View();
        }

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);

        var token = doc.RootElement.GetProperty("accessToken").GetString();
        var role = doc.RootElement.GetProperty("role").GetString();

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, email),
            new Claim(ClaimTypes.Role, role!),
            new Claim("access_token", token!)
        };

        var identity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity));

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction("Login");
    }

    public IActionResult AccessDenied()
    {
        return View();
    }
}
