using FluentValidation;
using HealthcareCareCoordination.Patient.Api.DTOs;
using HealthcareCareCoordination.Patient.Api.Infrastructure;
using HealthcareCareCoordination.SharedKernel;
using HealthcareCareCoordination.SharedKernel.Audit;
using HealthcareCareCoordination.Observability;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthcareCareCoordination.Patient.Api.Features;

public static class PatientEndpoints
{
    public static void MapPatientEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/v1/patients");

        group.MapPost("/", RegisterPatient);
        group.MapGet("/", GetPatients);
        group.MapGet("/{id:guid}", GetPatientById);
        group.MapPut("/{id:guid}", UpdatePatient);
        group.MapPatch("/{id:guid}/consent-status", UpdateConsentStatus);
    }

    private static async Task<IResult> RegisterPatient(
        RegisterPatientRequest request,
        IValidator<RegisterPatientRequest> validator,
        PatientDbContext dbContext,
        IAuditLogger auditLogger,
        HttpContext context,
        ILogger<Program> logger,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var patient = new Domain.Patient
        {
            FullName = request.FullName,
            DateOfBirth = request.DateOfBirth.Date,
            Gender = request.Gender,
            Email = request.Email,
            MobileNumber = request.MobileNumber,
            Address = request.Address,
            EmergencyContactName = request.EmergencyContactName,
            EmergencyContactNumber = request.EmergencyContactNumber,
            ConsentStatus = request.ConsentStatus
        };

        dbContext.Patients.Add(patient);
        await dbContext.SaveChangesAsync(cancellationToken);

        await auditLogger.LogEventAsync(
            eventType: "PatientRegistered",
            entityType: "Patient",
            entityId: patient.Id.ToString(),
            action: "RegisterPatient",
            outcome: "Success",
            summary: "Synthetic patient profile was registered successfully.",
            metadata: new { patient.ConsentStatus },
            patientId: patient.Id.ToString(),
            cancellationToken: cancellationToken);

        logger.LogInformation("Patient registered with ID: {PatientId}", patient.Id);

        var response = MapToResponse(patient);
        return Results.Created($"/api/v1/patients/{patient.Id}", new ApiResponse<PatientResponse>(
            response,
            context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier,
            DateTimeOffset.UtcNow));
    }

    private static async Task<IResult> GetPatients(PatientDbContext dbContext, HttpContext context, CancellationToken cancellationToken)
    {
        var patients = await dbContext.Patients
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(cancellationToken);

        var response = patients.Select(MapToResponse).ToList();
        return Results.Ok(new ApiResponse<IEnumerable<PatientResponse>>(
            response,
            context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier,
            DateTimeOffset.UtcNow));
    }

    private static async Task<IResult> GetPatientById(Guid id, PatientDbContext dbContext, HttpContext context, ILogger<Program> logger, CancellationToken cancellationToken)
    {
        var patient = await dbContext.Patients.FindAsync(new object[] { id }, cancellationToken);
        if (patient == null)
        {
            logger.LogWarning("Patient not found with ID: {PatientId}", id);
            return Results.NotFound(new ProblemDetails { Title = "Patient not found", Status = 404 });
        }

        logger.LogInformation("Patient accessed with ID: {PatientId}", id);

        return Results.Ok(new ApiResponse<PatientResponse>(
            MapToResponse(patient),
            context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier,
            DateTimeOffset.UtcNow));
    }

    private static async Task<IResult> UpdatePatient(
        Guid id,
        UpdatePatientRequest request,
        IValidator<UpdatePatientRequest> validator,
        PatientDbContext dbContext,
        HttpContext context,
        ILogger<Program> logger,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var patient = await dbContext.Patients.FindAsync(new object[] { id }, cancellationToken);
        if (patient == null)
        {
            return Results.NotFound(new ProblemDetails { Title = "Patient not found", Status = 404 });
        }

        patient.FullName = request.FullName;
        patient.Email = request.Email;
        patient.MobileNumber = request.MobileNumber;
        patient.Address = request.Address;
        patient.EmergencyContactName = request.EmergencyContactName;
        patient.EmergencyContactNumber = request.EmergencyContactNumber;
        patient.UpdatedAt = DateTimeOffset.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Patient updated with ID: {PatientId}", patient.Id);

        return Results.Ok(new ApiResponse<PatientResponse>(
            MapToResponse(patient),
            context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier,
            DateTimeOffset.UtcNow));
    }

    private static async Task<IResult> UpdateConsentStatus(
        Guid id,
        UpdateConsentStatusRequest request,
        IValidator<UpdateConsentStatusRequest> validator,
        PatientDbContext dbContext,
        IAuditLogger auditLogger,
        HttpContext context,
        ILogger<Program> logger,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var patient = await dbContext.Patients.FindAsync(new object[] { id }, cancellationToken);
        if (patient == null)
        {
            return Results.NotFound(new ProblemDetails { Title = "Patient not found", Status = 404 });
        }

        patient.ConsentStatus = request.ConsentStatus;
        patient.UpdatedAt = DateTimeOffset.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);

        await auditLogger.LogEventAsync(
            eventType: "ConsentStatusUpdated",
            entityType: "Patient",
            entityId: patient.Id.ToString(),
            action: "UpdateConsentStatus",
            outcome: "Success",
            summary: "Patient consent status was updated.",
            metadata: new { patient.ConsentStatus },
            patientId: patient.Id.ToString(),
            cancellationToken: cancellationToken);

        logger.LogInformation("Patient consent status updated for ID: {PatientId}", patient.Id);

        return Results.Ok(new ApiResponse<PatientResponse>(
            MapToResponse(patient),
            context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier,
            DateTimeOffset.UtcNow));
    }

    private static PatientResponse MapToResponse(Domain.Patient patient) => new(
        patient.Id,
        patient.FullName,
        patient.DateOfBirth,
        patient.Gender,
        patient.Email,
        patient.MobileNumber,
        patient.Address,
        patient.EmergencyContactName,
        patient.EmergencyContactNumber,
        patient.ConsentStatus,
        patient.CreatedAt,
        patient.UpdatedAt);
}
