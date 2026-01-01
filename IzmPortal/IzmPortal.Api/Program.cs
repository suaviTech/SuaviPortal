using IzmPortal.Api.Security;
using IzmPortal.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using System.Text;
namespace IzmPortal.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddInfrastructure(builder.Configuration);

       
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        builder.Services
    .AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            )
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

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }
        
        app.UseHttpsRedirection();

      
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
