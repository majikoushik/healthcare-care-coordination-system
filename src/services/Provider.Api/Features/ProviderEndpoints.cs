using FluentValidation;
using HealthcareCareCoordination.Provider.Api.Domain;
using HealthcareCareCoordination.Provider.Api.DTOs;
using HealthcareCareCoordination.Provider.Api.Infrastructure;
using HealthcareCareCoordination.SharedKernel;
using HealthcareCareCoordination.Observability;
using HealthcareCareCoordination.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthcareCareCoordination.Provider.Api.Features;

public static class ProviderEndpoints
{
    public static void MapProviderEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/v1/providers");

        group.MapPost("/", RegisterProvider)
             .RequireAuthorization(HealthcarePermissions.ProviderProfileWrite);
             
        group.MapGet("/", GetProviders)
             .RequireAuthorization(HealthcarePermissions.ProviderProfileRead);
             
        group.MapGet("/{id:guid}", GetProviderById)
             .RequireAuthorization(HealthcarePermissions.ProviderProfileRead);
             
        group.MapGet("/specialty/{specialty}", GetProvidersBySpecialty)
             .RequireAuthorization(HealthcarePermissions.ProviderProfileRead);
             
        group.MapPut("/{id:guid}", UpdateProvider)
             .RequireAuthorization(HealthcarePermissions.ProviderProfileWrite);
             
        group.MapPatch("/{id:guid}/availability-status", UpdateAvailabilityStatus)
             .RequireAuthorization(HealthcarePermissions.ProviderProfileWrite);
    }

    private static async Task<IResult> RegisterProvider(
        RegisterProviderRequest request,
        IValidator<RegisterProviderRequest> validator,
        ProviderDbContext dbContext,
        HttpContext context,
        ILogger<Program> logger)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var provider = new Domain.Provider
        {
            FullName = request.FullName,
            Specialty = request.Specialty,
            Email = request.Email,
            MobileNumber = request.MobileNumber,
            Department = request.Department,
            AvailabilityStatus = request.AvailabilityStatus
        };

        dbContext.Providers.Add(provider);
        await dbContext.SaveChangesAsync();

        logger.LogInformation("Provider created with ID: {ProviderId}", provider.Id);

        var response = MapToResponse(provider);
        return Results.Created($"/api/v1/providers/{provider.Id}", new ApiResponse<ProviderResponse>(
            response,
            context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier,
            DateTimeOffset.UtcNow));
    }

    private static async Task<IResult> GetProviders(ProviderDbContext dbContext, HttpContext context)
    {
        var providers = await dbContext.Providers
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

        var response = providers.Select(MapToResponse).ToList();
        return Results.Ok(new ApiResponse<IEnumerable<ProviderResponse>>(
            response,
            context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier,
            DateTimeOffset.UtcNow));
    }

    private static async Task<IResult> GetProviderById(Guid id, ProviderDbContext dbContext, HttpContext context, ILogger<Program> logger)
    {
        var provider = await dbContext.Providers.FindAsync(id);
        if (provider == null)
        {
            logger.LogWarning("Provider not found with ID: {ProviderId}", id);
            return Results.NotFound(new ProblemDetails { Title = "Provider not found", Status = 404 });
        }

        logger.LogInformation("Provider accessed with ID: {ProviderId}", id);

        return Results.Ok(new ApiResponse<ProviderResponse>(
            MapToResponse(provider),
            context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier,
            DateTimeOffset.UtcNow));
    }

    private static async Task<IResult> GetProvidersBySpecialty(Specialty specialty, ProviderDbContext dbContext, HttpContext context)
    {
        var providers = await dbContext.Providers
            .Where(p => p.Specialty == specialty)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

        var response = providers.Select(MapToResponse).ToList();
        return Results.Ok(new ApiResponse<IEnumerable<ProviderResponse>>(
            response,
            context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier,
            DateTimeOffset.UtcNow));
    }

    private static async Task<IResult> UpdateProvider(
        Guid id,
        UpdateProviderRequest request,
        IValidator<UpdateProviderRequest> validator,
        ProviderDbContext dbContext,
        HttpContext context,
        ILogger<Program> logger)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var provider = await dbContext.Providers.FindAsync(id);
        if (provider == null)
        {
            return Results.NotFound(new ProblemDetails { Title = "Provider not found", Status = 404 });
        }

        provider.FullName = request.FullName;
        provider.Specialty = request.Specialty;
        provider.Email = request.Email;
        provider.MobileNumber = request.MobileNumber;
        provider.Department = request.Department;
        provider.UpdatedAt = DateTimeOffset.UtcNow;

        await dbContext.SaveChangesAsync();

        logger.LogInformation("Provider updated with ID: {ProviderId}", provider.Id);

        return Results.Ok(new ApiResponse<ProviderResponse>(
            MapToResponse(provider),
            context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier,
            DateTimeOffset.UtcNow));
    }

    private static async Task<IResult> UpdateAvailabilityStatus(
        Guid id,
        UpdateAvailabilityStatusRequest request,
        IValidator<UpdateAvailabilityStatusRequest> validator,
        ProviderDbContext dbContext,
        HttpContext context,
        ILogger<Program> logger)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var provider = await dbContext.Providers.FindAsync(id);
        if (provider == null)
        {
            return Results.NotFound(new ProblemDetails { Title = "Provider not found", Status = 404 });
        }

        provider.AvailabilityStatus = request.AvailabilityStatus;
        provider.UpdatedAt = DateTimeOffset.UtcNow;

        await dbContext.SaveChangesAsync();

        logger.LogInformation("Provider availability status updated for ID: {ProviderId}", provider.Id);

        return Results.Ok(new ApiResponse<ProviderResponse>(
            MapToResponse(provider),
            context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier,
            DateTimeOffset.UtcNow));
    }

    private static ProviderResponse MapToResponse(Domain.Provider provider) => new(
        provider.Id,
        provider.FullName,
        provider.Specialty,
        provider.Email,
        provider.MobileNumber,
        provider.Department,
        provider.AvailabilityStatus,
        provider.CreatedAt,
        provider.UpdatedAt);
}
