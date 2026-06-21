using HealthcareCareCoordination.Patient.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace HealthcareCareCoordination.Patient.Api.Infrastructure;

public class PatientDbContext : DbContext
{
    public PatientDbContext(DbContextOptions<PatientDbContext> options) : base(options)
    {
    }

    public DbSet<Domain.Patient> Patients { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Domain.Patient>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FullName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
            entity.Property(e => e.MobileNumber).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Address).IsRequired().HasMaxLength(500);
            entity.Property(e => e.EmergencyContactName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.EmergencyContactNumber).IsRequired().HasMaxLength(50);
        });
    }
}
