using Microsoft.AspNetCore.Authentication.Cookies;
using IzmPortal.Admin.Infrastructure; // ApiAuthHandler burada olacak
using IzmPortal.Admin.Middleware;


var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

// 🔗 API Base URL
var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"]
    ?? throw new InvalidOperationException("ApiSettings:BaseUrl is not configured.");

// 🔐 HttpContext erişimi (JWT almak için ŞART)
builder.Services.AddHttpContextAccessor();

// 🔐 API Auth Handler (Bearer token ekler)
builder.Services.AddTransient<ApiAuthHandler>();

// 🌐 API Client (JWT otomatik gider)
builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
})
.AddHttpMessageHandler<ApiAuthHandler>();

// 🍪 Cookie Authentication
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Middleware
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseMiddleware<ForcePasswordChangeMiddleware>();
app.UseAuthorization();


// Routes
app.MapDefaultControllerRoute();

app.Run();
