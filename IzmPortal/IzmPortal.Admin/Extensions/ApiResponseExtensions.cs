using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace IzmPortal.Admin.Extensions;

public static class ApiResponseExtensions
{
    /// <summary>
    /// API response başarısızsa merkezi şekilde yönlendirir
    /// </summary>
    public static async Task<IActionResult?> HandleFailureAsync(
        this HttpResponseMessage response,
        Controller controller,
        string? errorMessage = null)
    {
        // 🔐 Oturum süresi dolmuş
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            return controller.RedirectToAction(
                "Login",
                "Account",
                new { expired = true });
        }

        // ⛔ Yetki yok
        if (response.StatusCode == HttpStatusCode.Forbidden)
        {
            return controller.RedirectToAction(
                "AccessDenied",
                "Account");
        }

        // 💥 Diğer hatalar
        if (!response.IsSuccessStatusCode)
        {
            controller.TempData["Error"] =
                errorMessage ?? "İşlem sırasında bir hata oluştu.";

            return controller.RedirectToAction("Index");
        }

        return null; // başarılı
    }

    /// <summary>
    /// JSON içeriği güvenli şekilde okur
    /// </summary>
    public static async Task<T?> ReadContentAsync<T>(
        this HttpResponseMessage response)
    {
        var json = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<T>(json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
    }
}
