using IzmPortal.Api.Security;
using IzmPortal.Application.Abstractions.Repositories;
using IzmPortal.Application.Abstractions.Services;
using IzmPortal.Application.Services;
using IzmPortal.Infrastructure;
using IzmPortal.Infrastructure.Identity;
using IzmPortal.Infrastructure.Persistence;
using IzmPortal.Infrastructure.Repositories;
using IzmPortal.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// =====================================================
// CONTROLLERS + GLOBAL FILTERS
// =====================================================
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ForcePasswordChangeFilter>();
});

// =====================================================
// INFRASTRUCTURE (DbContext, Configurations, etc.)
// =====================================================
builder.Services.AddInfrastructure(builder.Configuration);

// =====================================================
// OPEN API (Swagger)
// =====================================================
builder.Services.AddOpenApi();

// =====================================================
// JWT AUTHENTICATION
// =====================================================
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],

        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
        ),

        ClockSkew = TimeSpan.Zero
    };
});

// =====================================================
// IDENTITY (ApplicationUser)
// =====================================================
builder.Services
    .AddIdentityCore<ApplicationUser>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 4;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredUniqueChars = 0;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<PortalDbContext>()
    .AddSignInManager();

// =====================================================
// 🔥 JWT TOKEN GENERATOR (KRİTİK EKSİK OLAN PARÇA)
// =====================================================
builder.Services.AddScoped<JwtTokenGenerator>();

// =====================================================
// REPOSITORIES
// =====================================================
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IMenuRepository, MenuRepository>();
builder.Services.AddScoped<ISubMenuRepository, SubMenuRepository>();
builder.Services.AddScoped<IMenuDocumentRepository, MenuDocumentRepository>();
builder.Services.AddScoped<IApplicationShortcutRepository, ApplicationShortcutRepository>();

// =====================================================
// SERVICES
// =====================================================
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddScoped<ISubMenuService, SubMenuService>();
builder.Services.AddScoped<IMenuDocumentService, MenuDocumentService>();
builder.Services.AddScoped<IApplicationShortcutService, ApplicationShortcutService>();
builder.Services.AddScoped<IAuditService, AuditService>();

builder.Services.AddHttpContextAccessor();

// =====================================================
// AUTHORIZATION POLICIES
// =====================================================
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SuperAdminOnly", policy =>
        policy.RequireRole("SuperAdmin"));

    options.AddPolicy("AdminAccess", policy =>
        policy.RequireRole("SuperAdmin", "Manager"));
});

// =====================================================
// BUILD
// =====================================================
var app = builder.Build();

// =====================================================
// PIPELINE
// =====================================================
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// =====================================================
// IDENTITY SEED
// =====================================================
using (var scope = app.Services.CreateScope())
{
    await IdentitySeed.SeedAsync(scope.ServiceProvider);
}

app.MapControllers();

app.Run();
