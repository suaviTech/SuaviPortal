using System.IdentityModel.Tokens.Jwt;
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

    // -------------------------
    // LOGIN (GET)
    // -------------------------
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    // -------------------------
    // LOGIN (POST)
    // -------------------------
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

        // 🔐 TOKEN
        if (!doc.RootElement.TryGetProperty("token", out var tokenElement))
        {
            ViewBag.Error = "Token alınamadı";
            return View();
        }

        var token = tokenElement.GetString();
        if (string.IsNullOrEmpty(token))
        {
            ViewBag.Error = "Token boş";
            return View();
        }

        // 🔁 FORCE PASSWORD CHANGE
        var forcePasswordChange = false;
        if (doc.RootElement.TryGetProperty("forcePasswordChange", out var fpc))
        {
            forcePasswordChange = fpc.GetBoolean();
        }

        // -------------------------
        // JWT → CLAIMS
        // -------------------------
        var handler = new JwtSecurityTokenHandler();
        JwtSecurityToken jwt = handler.ReadJwtToken(token);

        var claims = new List<Claim>
{
    new Claim(ClaimTypes.Name, email),
    new Claim("access_token", token)
};

        // ROLE CLAIMLERİ
        var roleClaims = jwt.Claims
            .Where(c => c.Type == ClaimTypes.Role || c.Type == "role")
            .Select(c => new Claim(ClaimTypes.Role, c.Value));

        claims.AddRange(roleClaims);


        // -------------------------
        // COOKIE LOGIN
        // -------------------------
        var identity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity));

        // -------------------------
        // FORCE PASSWORD CHANGE
        // -------------------------
        if (forcePasswordChange)
        {
            return RedirectToAction("ChangePassword", "Account");
        }

        return RedirectToAction("Index", "Home");
    }

    // -------------------------
    // LOGOUT
    // -------------------------
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToAction("Login");
    }

    // -------------------------
    // ACCESS DENIED
    // -------------------------
    public IActionResult AccessDenied()
    {
        return View();
    }

    // -------------------------
    // CHANGE PASSWORD (GET)
    // -------------------------
    [HttpGet]
    public IActionResult ChangePassword()
    {
        return View();
    }

    // -------------------------
    // CHANGE PASSWORD (POST)
    // -------------------------
    [HttpPost]
    public async Task<IActionResult> ChangePassword(
        string CurrentPassword,
        string NewPassword)
    {
        var response = await _apiClient.PostAsJsonAsync(
            "/api/auth/change-password",
            new
            {
                CurrentPassword,
                NewPassword
            });

        if (!response.IsSuccessStatusCode)
        {
            ViewBag.Error = "Şifre değiştirilemedi";
            return View();
        }

        // 🔒 Şifre değişti → logout
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToAction("Login");
    }
}
