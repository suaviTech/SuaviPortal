using IzmPortal.Api.Security;
using IzmPortal.Infrastructure;
using IzmPortal.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --------------------
// Services
// --------------------
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddOpenApi();

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





builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SuperAdminOnly", policy =>
        policy.RequireRole("SuperAdmin"));

    options.AddPolicy("AdminAccess", policy =>
        policy.RequireRole("SuperAdmin", "Manager"));
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ForcePasswordChangeFilter>();
});

builder.Services.AddScoped<JwtTokenGenerator>();

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

// 🔥 IDENTITY SEED (DOĞRU YER)
using (var scope = app.Services.CreateScope())
{
    await IdentitySeed.SeedAsync(scope.ServiceProvider);
}

app.MapControllers();

app.Run();
