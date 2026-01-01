using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IzmPortal.Application.Abstractions.Repositories;
using IzmPortal.Application.Abstractions.Services;
using IzmPortal.Infrastructure.Persistence;
using IzmPortal.Infrastructure.Repositories;
using IzmPortal.Application.Services;

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

        // Repositories
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();

        // Services
        services.AddScoped<ICategoryService, CategoryService>();

        return services;
    }
}
