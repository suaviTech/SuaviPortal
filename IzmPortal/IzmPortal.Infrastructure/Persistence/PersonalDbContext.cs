using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace IzmPortal.Infrastructure.Persistence;

public class PersonalDbContext : DbContext
{
    public PersonalDbContext(DbContextOptions<PersonalDbContext> options)
        : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    public DbSet<TblPersonal> Tbl_Personal => Set<TblPersonal>();

    public override EntityEntry Add(object entity)
    => throw new InvalidOperationException("PersonalDbContext is read-only.");

    public override int SaveChanges()
        => throw new InvalidOperationException("PersonalDbContext is read-only.");

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => throw new InvalidOperationException("PersonalDbContext is read-only.");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblPersonal>().HasKey(x => x.Id);
        base.OnModelCreating(modelBuilder);
    }
}