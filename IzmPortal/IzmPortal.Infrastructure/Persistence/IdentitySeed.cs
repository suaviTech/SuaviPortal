using IzmPortal.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

public static class IdentitySeed
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var roleManager =
            scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        var userManager =
            scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        // 1️⃣ Roller
        var roles = new[] { "SuperAdmin", "Manager" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // 2️⃣ Admin kullanıcı
        var adminEmail = "admin@izmportal.local";

        var admin = await userManager.FindByEmailAsync(adminEmail);

        if (admin == null)
        {
            admin = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                TcNumber = "11111111111",
                ForcePasswordChange = true
            };

            // 🔐 İlk şifre = TC son 4
            var result = await userManager.CreateAsync(admin, "1111");

            if (!result.Succeeded)
                throw new Exception("Admin oluşturulamadı");

            await userManager.AddToRoleAsync(admin, "SuperAdmin");
        }
    }
}
