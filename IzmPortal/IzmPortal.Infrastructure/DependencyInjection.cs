using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IzmPortal.Infrastructure.Persistence;

namespace IzmPortal.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<PortalDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("PortalConnection")));

        services.AddDbContext<PersonalDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("PersonalConnection")));

        return services;
    }
}

