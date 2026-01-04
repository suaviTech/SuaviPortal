using IzmPortal.Application.Abstractions.Repositories;
using IzmPortal.Application.Abstractions.Services;
using IzmPortal.Application.Services;
using IzmPortal.Infrastructure.Identity;
using IzmPortal.Infrastructure.Persistence;
using IzmPortal.Infrastructure.Repositories;
using IzmPortal.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IzmPortal.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // ============================
        // DbContexts
        // ============================
        services.AddDbContext<PortalDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("PortalConnection")));

        services.AddDbContext<PersonalDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("PersonalConnection")));

        // ============================
        // Repositories
        // ============================
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IMenuRepository, MenuRepository>();
        services.AddScoped<ISubMenuRepository, SubMenuRepository>();
        services.AddScoped<IMenuDocumentRepository, MenuDocumentRepository>();
        services.AddScoped<IApplicationShortcutRepository, ApplicationShortcutRepository>();
        services.AddScoped<ISliderRepository, SliderRepository>();

        // ============================
        // Services (Domain / Application)
        // ============================
        services.AddScoped<IAnnouncementService, AnnouncementService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IMenuService, MenuService>();
        services.AddScoped<ISubMenuService, SubMenuService>();
        services.AddScoped<IMenuDocumentService, MenuDocumentService>();
        services.AddScoped<IApplicationShortcutService, ApplicationShortcutService>();
        services.AddScoped<ISliderService, SliderService>();

        services.AddScoped<IAuditService, AuditService>();
        services.AddScoped<IFileStorageService, LocalFileStorageService>();

        return services;
    }
}
