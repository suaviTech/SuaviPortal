using IzmPortal.Api.Security;
using IzmPortal.Infrastructure;
using IzmPortal.Infrastructure.Identity;
using IzmPortal.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
// INFRASTRUCTURE (DbContext + Services)
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
// IDENTITY
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
// JWT TOKEN GENERATOR
// =====================================================
builder.Services.AddScoped<JwtTokenGenerator>();

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
