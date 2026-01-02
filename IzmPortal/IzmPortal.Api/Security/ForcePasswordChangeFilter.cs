using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IzmPortal.Api.Security;

public class ForcePasswordChangeFilter : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;

        // Auth yoksa dokunma (AllowAnonymous senaryosu)
        if (user?.Identity?.IsAuthenticated != true)
            return;

        var force = user.FindFirst("force_password_change")?.Value;

        if (force == "true")
        {
            var path = context.HttpContext.Request.Path.Value?.ToLower();

            // Sadece change-password serbest
            if (path is not null && path.Contains("/api/auth/change-password"))
                return;

            context.Result = new ForbidResult();
        }
    }
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var path = context.HttpContext.Request.Path.Value;

        if (path != null &&
            (path.StartsWith("/Account/Login")
            || path.StartsWith("/Account/AccessDenied")))
        {
            return; // 🔴 login ve access denied'e dokunma
        }

        // mevcut force password change logic
    }

}
