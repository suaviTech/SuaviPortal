using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace IzmPortal.Admin.Controllers;

public abstract class BaseAdminController : Controller
{
    protected readonly HttpClient Api;

    protected BaseAdminController(IHttpClientFactory factory)
    {
        Api = factory.CreateClient("ApiClient");
    }

    protected async Task<IActionResult?> HandleApiFailureAsync(
        HttpResponseMessage response,
        string defaultMessage,
        string? redirectAction = "Index",
        string? redirectController = null,
        object? routeValues = null)
    {
        if (response.IsSuccessStatusCode)
            return null;

        // 🔐 Token düşmüş / yetki yok
        if (response.StatusCode == HttpStatusCode.Unauthorized)
            return RedirectToAction("Logout", "Account");

        // 🔎 API'den mesaj okumaya çalış
        var apiMessage = defaultMessage;

        try
        {
            var content = await response.Content.ReadAsStringAsync();
            if (!string.IsNullOrWhiteSpace(content))
                apiMessage = content;
        }
        catch
        {
            // sessiz geç
        }

        TempData["Error"] = apiMessage;

        return RedirectToAction(
            redirectAction!,
            redirectController,
            routeValues);
    }

    protected IActionResult SuccessAndRedirect(
        string message,
        string? action = "Index",
        string? controller = null,
        object? routeValues = null)
    {
        TempData["Success"] = message;

        return RedirectToAction(
            action!,
            controller,
            routeValues);
    }
}
