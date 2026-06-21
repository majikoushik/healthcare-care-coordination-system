using HealthcareCareCoordination.Provider.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace HealthcareCareCoordination.Provider.Api.Infrastructure;

public class ProviderDbContext : DbContext
{
    public ProviderDbContext(DbContextOptions<ProviderDbContext> options) : base(options)
    {
    }

    public DbSet<Domain.Provider> Providers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Domain.Provider>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FullName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
            entity.Property(e => e.MobileNumber).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Department).IsRequired().HasMaxLength(200);
        });
    }
}
