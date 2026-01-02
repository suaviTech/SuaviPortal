using IzmPortal.Domain.Entities;
using IzmPortal.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


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
    public DbSet<SubMenu> SubMenus => Set<SubMenu>();
    public DbSet<MenuDocument> MenuDocuments => Set<MenuDocument>();
    public DbSet<Slider> Sliders => Set<Slider>();
    public DbSet<ApplicationShortcut> ApplicationShortcuts => Set<ApplicationShortcut>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PortalDbContext).Assembly);
        
    }
}
