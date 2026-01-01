using Microsoft.EntityFrameworkCore;
using IzmPortal.Domain.Entities;
using IzmPortal.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace IzmPortal.Infrastructure.Persistence;

public class PortalDbContext : IdentityDbContext<ApplicationUser>
{
    public PortalDbContext(DbContextOptions<PortalDbContext> options)
        : base(options)
    {
    }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Announcement> Announcements => Set<Announcement>();
    public DbSet<Menu> Menus => Set<Menu>();
    public DbSet<Slider> Sliders => Set<Slider>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PortalDbContext).Assembly);
        
    }
}
