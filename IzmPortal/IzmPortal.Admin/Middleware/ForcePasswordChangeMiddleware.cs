using Microsoft.AspNetCore.Http;

namespace IzmPortal.Admin.Middleware;

public class ForcePasswordChangeMiddleware
{
    private readonly RequestDelegate _next;

    public ForcePasswordChangeMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var forceChange = context.User
                .Claims
                .FirstOrDefault(c => c.Type == "force_password_change")
                ?.Value;

            var path = context.Request.Path.Value?.ToLower();

            if (forceChange == "true"
                && path != null
                && !path.StartsWith("/account/changepassword")
                && !path.StartsWith("/account/logout"))
            {
                context.Response.Redirect("/Account/ChangePassword");
                return;
            }
        }

        await _next(context);
    }
}

