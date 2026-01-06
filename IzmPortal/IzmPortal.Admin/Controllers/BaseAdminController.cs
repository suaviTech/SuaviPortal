using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace IzmPortal.Admin.Controllers;

[Authorize]
public abstract class BaseAdminController : Controller
{
    protected readonly HttpClient Api;
    protected string ApiBaseUrl { get; }

    protected BaseAdminController(IHttpClientFactory factory)
    {
        Api = factory.CreateClient("ApiClient");

        ApiBaseUrl = Api.BaseAddress?.ToString()
            ?? throw new InvalidOperationException("ApiClient BaseAddress tanımlı değil.");

        ViewData["ApiBaseUrl"] = ApiBaseUrl;
    }

    protected async Task<IActionResult?> HandleApiFailureAsync(
        HttpResponseMessage response,
        string defaultMessage,
        string redirectAction = "Index",
        string? redirectController = null,
        object? routeValues = null)
    {
        if (response.IsSuccessStatusCode)
            return null;

        // 🔐 Token süresi dolmuş
        if (response.StatusCode == HttpStatusCode.Unauthorized)
            return RedirectToAction("Logout", "Account");

        // ⛔ Yetki yok
        if (response.StatusCode == HttpStatusCode.Forbidden)
            return RedirectToAction("AccessDenied", "Account");

        var message = defaultMessage;

        try
        {
            var content = await response.Content.ReadAsStringAsync();
            if (!string.IsNullOrWhiteSpace(content))
                message = content;
        }
        catch
        {
            // sessiz geç
        }

        TempData["Error"] = message;

        return RedirectToAction(
            redirectAction,
            redirectController,
            routeValues);
    }

    protected IActionResult SuccessAndRedirect(
        string message,
        string action = "Index",
        string? controller = null,
        object? routeValues = null)
    {
        TempData["Success"] = message;

        return RedirectToAction(
            action,
            controller,
            routeValues);
    }
}
