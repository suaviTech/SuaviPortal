using IzmPortal.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace IzmPortal.Infrastructure.Persistence;

public static class IdentitySeed
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var roleManager = scope.ServiceProvider
            .GetRequiredService<RoleManager<IdentityRole>>();

        var userManager = scope.ServiceProvider
            .GetRequiredService<UserManager<ApplicationUser>>();

        // 1️⃣ Roller
        var roles = new[] { "SuperAdmin", "Manager" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // 2️⃣ İlk Admin Kullanıcı
        var adminEmail = "admin@izmportal.local";
        var tcNumber = "44444444444";               // 🔐 TC
        var pin = tcNumber.Substring(tcNumber.Length - 4); // 👉 4444

        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                TcNumber = tcNumber
            };

            var result = await userManager.CreateAsync(adminUser, pin);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "SuperAdmin");
            }
        }
    }
}
