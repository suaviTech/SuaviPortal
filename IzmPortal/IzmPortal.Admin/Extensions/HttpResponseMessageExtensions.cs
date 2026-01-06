using System.Text.Json;

namespace IzmPortal.Admin.Extensions;

public static class HttpResponseMessageExtensions
{
    /// <summary>
    /// JSON içeriği güvenli şekilde deserialize eder
    /// </summary>
    public static async Task<T?> ReadContentAsync<T>(
        this HttpResponseMessage response)
    {
        var json = await response.Content.ReadAsStringAsync();

        if (string.IsNullOrWhiteSpace(json))
            return default;

        return JsonSerializer.Deserialize<T>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
    }
}
