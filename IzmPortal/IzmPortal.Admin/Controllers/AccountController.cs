using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;

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
    [AllowAnonymous]
    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl;
        return View();
    }

    // -------------------------
    // LOGIN (POST)
    // -------------------------
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Login(
        string email,
        string password,
        string? returnUrl = null)
    {
        var response = await _apiClient.PostAsJsonAsync(
            "/api/auth/login",
            new { email, password });

        if (!response.IsSuccessStatusCode)
        {
            ViewBag.Error = "Giriş başarısız";
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);

        // -------------------------
        // TOKEN
        // -------------------------
        if (!doc.RootElement.TryGetProperty("token", out var tokenElement))
        {
            ViewBag.Error = "Token alınamadı";
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        var token = tokenElement.GetString();
        if (string.IsNullOrWhiteSpace(token))
        {
            ViewBag.Error = "Token boş";
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // -------------------------
        // FORCE PASSWORD CHANGE
        // -------------------------
        var forcePasswordChange = false;
        if (doc.RootElement.TryGetProperty("forcePasswordChange", out var fpc))
        {
            forcePasswordChange = fpc.GetBoolean();
        }

        // -------------------------
        // JWT → CLAIMS
        // -------------------------
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, email),
            new Claim(ClaimTypes.Name, email),
            new Claim("access_token", token)
        };

        // ROLE CLAIMS
        claims.AddRange(
            jwt.Claims
               .Where(c => c.Type == ClaimTypes.Role || c.Type == "role")
               .Select(c => new Claim(ClaimTypes.Role, c.Value))
        );

        // FORCE PASSWORD CLAIM
        if (forcePasswordChange)
        {
            claims.Add(new Claim("force_password_change", "true"));
        }

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
        // REDIRECT
        // -------------------------
        if (forcePasswordChange)
        {
            return RedirectToAction("ChangePassword");
        }

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction("Index", "Home");
    }

    // -------------------------
    // LOGOUT
    // -------------------------
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToAction("Login");
    }

    // -------------------------
    // ACCESS DENIED
    // -------------------------
    [AllowAnonymous]
    public IActionResult AccessDenied()
    {
        return View();
    }

    // -------------------------
    // CHANGE PASSWORD (GET)
    // -------------------------
    [Authorize]
    [HttpGet]
    public IActionResult ChangePassword()
    {
        return View();
    }

    // -------------------------
    // CHANGE PASSWORD (POST)
    // -------------------------
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> ChangePassword(
        string CurrentPassword,
        string NewPassword)
    {
        var response = await _apiClient.PostAsJsonAsync(
            "/api/auth/change-password",
            new { CurrentPassword, NewPassword });

        if (!response.IsSuccessStatusCode)
        {
            ViewBag.Error = "Şifre değiştirilemedi";
            return View();
        }

        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToAction("Login");
    }
}
