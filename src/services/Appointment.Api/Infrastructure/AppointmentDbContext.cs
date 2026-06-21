using HealthcareCareCoordination.Appointment.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace HealthcareCareCoordination.Appointment.Api.Infrastructure;

public class AppointmentDbContext : DbContext
{
    public AppointmentDbContext(DbContextOptions<AppointmentDbContext> options) : base(options)
    {
    }

    public DbSet<Domain.Appointment> Appointments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Domain.Appointment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.VisitReason).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Notes).HasMaxLength(2000);
        });
    }
}
