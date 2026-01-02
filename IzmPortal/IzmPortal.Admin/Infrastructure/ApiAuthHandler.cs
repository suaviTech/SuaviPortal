using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace IzmPortal.Admin.Infrastructure;

public class ApiAuthHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ApiAuthHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var context = _httpContextAccessor.HttpContext;

        // 🔐 JWT'yi cookie'den al
        var token = context?
            .User?
            .FindFirst("access_token")?
            .Value;

        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        var response = await base.SendAsync(request, cancellationToken);

        // 🚨 TOKEN GEÇERSİZ / SÜRESİ DOLMUŞ
        if (response.StatusCode == HttpStatusCode.Unauthorized && context != null)
        {
            // Cookie temizle
            await context.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            // Login'e yönlendir
            context.Response.Redirect("/Account/Login");
        }

        return response;
    }
}
