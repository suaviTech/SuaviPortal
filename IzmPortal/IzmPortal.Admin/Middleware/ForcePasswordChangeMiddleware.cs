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

        if (user.Identity?.IsAuthenticated == true)
        {
            var force = user.FindFirst("force_password_change")?.Value;

            var path = context.Request.Path.Value?.ToLower();

            if (force == "true" &&
                path != null &&
                !path.StartsWith("/account/changepassword") &&
                !path.StartsWith("/account/logout"))
            {
                context.Response.Redirect("/Account/ChangePassword");
                return;
            }
        }

        await _next(context);
    }
}
