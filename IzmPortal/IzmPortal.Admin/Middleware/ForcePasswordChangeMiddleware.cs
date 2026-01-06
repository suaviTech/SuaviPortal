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
        var user = context.User;

        // 🔹 Static dosyaları atla
        var path = context.Request.Path;
        if (path.StartsWithSegments("/css") ||
            path.StartsWithSegments("/js") ||
            path.StartsWithSegments("/lib") ||
            path.StartsWithSegments("/favicon"))
        {
            await _next(context);
            return;
        }

        if (user.Identity?.IsAuthenticated == true)
        {
            var force = user.FindFirst("force_password_change")?.Value;

            if (force == "true")
            {
                // 🔹 Bu sayfalara izin ver
                if (!path.StartsWithSegments("/Account/ChangePassword") &&
                    !path.StartsWithSegments("/Account/Logout") &&
                    !path.StartsWithSegments("/Account/Login") &&
                    !path.StartsWithSegments("/Account/AccessDenied"))
                {
                    context.Response.Redirect("/Account/ChangePassword");
                    return;
                }
            }
        }

        await _next(context);
    }
}
