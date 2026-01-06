using IzmPortal.Admin.Infrastructure; // ApiAuthHandler
using IzmPortal.Admin.Middleware;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;


var builder = WebApplication.CreateBuilder(args);

// --------------------
// MVC
// --------------------
builder.Services.AddControllersWithViews();






// --------------------
// API Base URL (appsettings.json)
// --------------------
var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"]
    ?? throw new InvalidOperationException("ApiSettings:BaseUrl is not configured.");

// --------------------
// HttpContext (JWT için şart)
// --------------------
builder.Services.AddHttpContextAccessor();

// --------------------
// API Auth Handler (Bearer ekler)
// --------------------
builder.Services.AddTransient<ApiAuthHandler>();

// --------------------
// HttpClient → API
// --------------------
builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
})
.AddHttpMessageHandler<ApiAuthHandler>();

// --------------------
// Cookie Authentication
// --------------------
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";

        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.Redirect(context.RedirectUri);
            return Task.CompletedTask;
        };

        options.Cookie.Name = "IzmPortal.Admin.Auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;

        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });


builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});



var app = builder.Build();

// --------------------
// Middleware
// --------------------
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseMiddleware<ForcePasswordChangeMiddleware>();
app.UseAuthorization();

// --------------------
// Routes
// --------------------
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
