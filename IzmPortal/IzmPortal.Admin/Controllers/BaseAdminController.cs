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
        string defaultMessage)
    {
        if (response.IsSuccessStatusCode)
            return null;

        if (response.StatusCode == HttpStatusCode.Unauthorized)
            return RedirectToAction("Logout", "Account");

        TempData["Error"] = defaultMessage;
        return RedirectToAction("Index");
    }

    protected IActionResult SuccessAndRedirect(string message)
    {
        TempData["Success"] = message;
        return RedirectToAction("Index");
    }
}
