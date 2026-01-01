using IzmPortal.Application.Abstractions.Repositories;
using IzmPortal.Application.Abstractions.Services;
using IzmPortal.Application.Services;
using IzmPortal.Infrastructure.Identity;
using IzmPortal.Infrastructure.Persistence;
using IzmPortal.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IzmPortal.Infrastructure.Services;


namespace IzmPortal.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // DbContexts
        services.AddDbContext<PortalDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("PortalConnection")));

        services.AddDbContext<PersonalDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("PersonalConnection")));

        // 🔐 IDENTITY (EKSİK OLAN KISIM BUYDU)
        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            options.Password.RequiredLength = 4;
            options.Password.RequireDigit = true;

            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;

            options.Password.RequiredUniqueChars = 1;
        })
.AddEntityFrameworkStores<PortalDbContext>()
.AddDefaultTokenProviders();



        // Repositories
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();

        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IAnnouncementService, AnnouncementService>();

        services.AddScoped<IFileStorageService, LocalFileStorageService>();

        services.AddScoped<ISliderRepository, SliderRepository>();
        services.AddScoped<ISliderService, SliderService>();


        return services;
    }
}

