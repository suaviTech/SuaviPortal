using Microsoft.EntityFrameworkCore;

namespace IzmPortal.Infrastructure.Persistence;

public class PersonalDbContext : DbContext
{
    public PersonalDbContext(DbContextOptions<PersonalDbContext> options)
        : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    public DbSet<TblPersonal> Tbl_Personal => Set<TblPersonal>();

    public override int SaveChanges()
        => throw new InvalidOperationException("PersonalDbContext is read-only.");

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => throw new InvalidOperationException("PersonalDbContext is read-only.");
}
