using IzmPortal.Admin.Extensions;
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
        string errorMessage)
    {
        return await response.HandleFailureAsync(this, errorMessage);
    }

    protected IActionResult SuccessAndRedirect(
        string message,
        string action = "Index")
    {
        TempData["Success"] = message;
        return RedirectToAction(action);
    }

    protected IActionResult ErrorAndRedirect(
        string message,
        string action = "Index")
    {
        TempData["Error"] = message;
        return RedirectToAction(action);
    }
}

