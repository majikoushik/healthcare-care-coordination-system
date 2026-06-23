using System.Net;
using System.Net.Http.Json;
using HealthcareCareCoordination.Patient.Api.Domain;
using HealthcareCareCoordination.Patient.Api.DTOs;
using HealthcareCareCoordination.Patient.Api.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Patient.Api.Tests;

/// <summary>
/// Verifies the full patient registration flow through the API endpoints.
/// Uses WebApplicationFactory to run the Patient.Api in-memory.
/// Demonstrates end-to-end integration from HTTP request -> FluentValidation -> EF Core DB.
/// </summary>
public class PatientRegistrationIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public PatientRegistrationIntegrationTests(WebApplicationFactory<Program> factory)
    {
        // Override the SQL Server DbContext with an InMemory one for CI safety
        var appFactory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<PatientDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<PatientDbContext>(options =>
                {
                    options.UseInMemoryDatabase("IntegrationTestsDb");
                });
            });
        });

        _client = appFactory.CreateClient();
    }

    [Fact]
    public async Task RegisterPatient_WhenValidRequest_ReturnsCreated()
    {
        // Arrange
        var request = new RegisterPatientRequest(
            FullName: "Integration Test Patient",
            DateOfBirth: new DateTime(1980, 1, 1),
            Gender: Gender.Male,
            Email: "integration.patient@demo.local",
            MobileNumber: "+44-000-000-1111",
            Address: "123 Integration Street",
            EmergencyContactName: "Emergency Demo",
            EmergencyContactNumber: "+44-000-000-2222",
            ConsentStatus: ConsentStatus.Provided
        );

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/patients", request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        
        var location = response.Headers.Location;
        Assert.NotNull(location);
        
        // Verify we can fetch the newly created patient
        var getResponse = await _client.GetAsync(location);
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
    }

    [Fact]
    public async Task RegisterPatient_WhenValidationFails_ReturnsBadRequestWithProblemDetails()
    {
        // Arrange: Missing FullName and Invalid Email
        var invalidRequest = new RegisterPatientRequest(
            FullName: "", 
            DateOfBirth: new DateTime(1980, 1, 1),
            Gender: Gender.Male,
            Email: "not-an-email",
            MobileNumber: "+44-000-000-1111",
            Address: "123 Integration Street",
            EmergencyContactName: "Emergency Demo",
            EmergencyContactNumber: "+44-000-000-2222",
            ConsentStatus: ConsentStatus.Provided
        );

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/patients", invalidRequest);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        // Standard ASP.NET Core Problem Details response shape
        var problemDetails = await response.Content.ReadFromJsonAsync<System.Text.Json.JsonElement>();
        Assert.Equal("https://tools.ietf.org/html/rfc9110#section-15.5.1", problemDetails.GetProperty("type").GetString());
        Assert.Equal(400, problemDetails.GetProperty("status").GetInt32());
        
        // Verify errors collection contains our specific validation failures
        var errors = problemDetails.GetProperty("errors");
        Assert.True(errors.TryGetProperty("FullName", out _));
        Assert.True(errors.TryGetProperty("Email", out _));
    }
}
