using HealthcareCareCoordination.Patient.Api.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Patient.Api.Tests;

public class PatientApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // 1. Set environment to Testing. This allows Program.cs to skip production-only startup logic like migrations.
        builder.UseEnvironment("Testing");

        // 2. Provide a dummy connection string so the configuration check in Program.cs doesn't throw an InvalidOperationException before ConfigureServices can run.
        builder.UseSetting("ConnectionStrings:DefaultConnection", "Server=dummy;Database=test;User Id=sa;Password=dummy;");

        // 3. Override the real database with an InMemory one for CI safety.
        builder.ConfigureTestServices(services =>
        {
            // Fully remove any existing EF Core SQL Server registrations and lambda configurations
            var descriptors = services.Where(d => 
                d.ServiceType == typeof(DbContextOptions<PatientDbContext>) ||
                d.ServiceType == typeof(DbContextOptions) ||
                d.ServiceType == typeof(PatientDbContext) ||
                d.ServiceType.Name.Contains("DbContextOptionsConfiguration")).ToList();

            foreach (var d in descriptors)
            {
                services.Remove(d);
            }
            
            services.RemoveAll(typeof(System.Data.Common.DbConnection));

            services.AddDbContext<PatientDbContext>(options =>
            {
                options.UseInMemoryDatabase("IntegrationTestsDb");
            });

            // Ensure the in-memory database is created for the tests
            using var scope = services.BuildServiceProvider().CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<PatientDbContext>();
            db.Database.EnsureCreated();
        });
    }
}
