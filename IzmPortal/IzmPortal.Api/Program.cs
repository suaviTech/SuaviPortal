using IzmPortal.Api.Security;
using IzmPortal.Application.Abstractions.Services;
using IzmPortal.Infrastructure;
using IzmPortal.Infrastructure.Identity;
using IzmPortal.Infrastructure.Persistence;
using IzmPortal.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using IzmPortal.Application.Services;


var builder = WebApplication.CreateBuilder(args);

// --------------------
// Controllers + Filters
// --------------------
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ForcePasswordChangeFilter>();
});

// --------------------
// Infrastructure
// --------------------
builder.Services.AddInfrastructure(builder.Configuration);

// --------------------
// OpenAPI
// --------------------
builder.Services.AddOpenApi();

// --------------------
// Authentication (JWT)
// --------------------
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

// --------------------
// Identity (ApplicationUser)
// --------------------
builder.Services
    .AddIdentityCore<ApplicationUser>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequiredLength = 4;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<PortalDbContext>()
    .AddSignInManager()
    .AddUserManager<UserManager<ApplicationUser>>();


// --------------------
// Application Services
// --------------------
builder.Services.AddScoped<IApplicationShortcutService, ApplicationShortcutService>();
builder.Services.AddScoped<IAuditService, AuditService>();
builder.Services.AddScoped<JwtTokenGenerator>();

builder.Services.AddHttpContextAccessor();

// --------------------
// Authorization Policies
// --------------------
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SuperAdminOnly", policy =>
        policy.RequireRole("SuperAdmin"));

    options.AddPolicy("AdminAccess", policy =>
        policy.RequireRole("SuperAdmin", "Manager"));
});

// --------------------
// Build
// --------------------
var app = builder.Build();

// --------------------
// Pipeline
// --------------------
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// --------------------
// Identity Seed
// --------------------
using (var scope = app.Services.CreateScope())
{
    await IdentitySeed.SeedAsync(scope.ServiceProvider);
}

app.MapControllers();

app.Run();
