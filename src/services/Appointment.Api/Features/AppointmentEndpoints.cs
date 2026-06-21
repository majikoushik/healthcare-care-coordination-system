using FluentValidation;
using HealthcareCareCoordination.Appointment.Api.Domain;
using HealthcareCareCoordination.Appointment.Api.DTOs;
using HealthcareCareCoordination.Appointment.Api.Infrastructure;
using HealthcareCareCoordination.SharedKernel;
using HealthcareCareCoordination.SharedKernel.Audit;
using HealthcareCareCoordination.Observability;
using HealthcareCareCoordination.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthcareCareCoordination.Appointment.Api.Features;

public static class AppointmentEndpoints
{
    public static void MapAppointmentEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/v1/appointments");

        group.MapPost("/", ScheduleAppointment)
             .RequireAuthorization(HealthcarePermissions.AppointmentWrite);
             
        group.MapGet("/", GetAppointments)
             .RequireAuthorization(HealthcarePermissions.AppointmentRead);
             
        group.MapGet("/{id:guid}", GetAppointmentById)
             .RequireAuthorization(HealthcarePermissions.AppointmentRead);
             
        group.MapGet("/date/{date:datetime}", GetAppointmentsByDate)
             .RequireAuthorization(HealthcarePermissions.AppointmentRead);
             
        group.MapPut("/{id:guid}", UpdateAppointment)
             .RequireAuthorization(HealthcarePermissions.AppointmentWrite);
             
        group.MapPatch("/{id:guid}/status", UpdateAppointmentStatus)
             .RequireAuthorization(HealthcarePermissions.AppointmentStatusUpdate);

        // Also map patient and provider specific endpoints at root level to adhere to REST
        routes.MapGet("/api/v1/patients/{patientId:guid}/appointments", GetAppointmentsByPatientId)
              .RequireAuthorization(HealthcarePermissions.AppointmentRead);
              
        routes.MapGet("/api/v1/providers/{providerId:guid}/appointments", GetAppointmentsByProviderId)
              .RequireAuthorization(HealthcarePermissions.AppointmentRead);
    }

    private static async Task<IResult> ScheduleAppointment(
        ScheduleAppointmentRequest request,
        IValidator<ScheduleAppointmentRequest> validator,
        AppointmentDbContext dbContext,
        HttpContext context,
        IAuditLogger auditLogger,
        ILogger<Program> logger,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var appointment = new Domain.Appointment
        {
            PatientId = request.PatientId,
            ProviderId = request.ProviderId,
            AppointmentDateTime = request.AppointmentDateTime,
            VisitReason = request.VisitReason,
            Type = request.Type,
            Notes = request.Notes ?? string.Empty,
            Status = AppointmentStatus.Requested
        };

        dbContext.Appointments.Add(appointment);
        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Appointment scheduled with ID: {AppointmentId} for Patient ID: {PatientId} and Provider ID: {ProviderId}", appointment.Id, appointment.PatientId, appointment.ProviderId);

        await auditLogger.LogEventAsync(
            eventType: "AppointmentScheduled",
            entityType: "Appointment",
            entityId: appointment.Id.ToString(),
            action: "ScheduleAppointment",
            outcome: "Success",
            summary: "Appointment scheduling request was created.",
            metadata: new
            {
                appointment.AppointmentDateTime,
                appointment.Type,
                appointment.Status
            },
            patientId: appointment.PatientId.ToString(),
            providerId: appointment.ProviderId.ToString(),
            actorType: "User",
            actorId: context.User.Identity?.Name ?? "DemoUser",
            cancellationToken: cancellationToken);

        var response = MapToResponse(appointment);
        return Results.Created($"/api/v1/appointments/{appointment.Id}", new ApiResponse<AppointmentResponse>(
            response,
            context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier,
            DateTimeOffset.UtcNow));
    }

    private static async Task<IResult> GetAppointments(AppointmentDbContext dbContext, HttpContext context, CancellationToken cancellationToken)
    {
        var appointments = await dbContext.Appointments
            .OrderByDescending(p => p.AppointmentDateTime)
            .ToListAsync(cancellationToken);

        var response = appointments.Select(MapToResponse).ToList();
        return Results.Ok(new ApiResponse<IEnumerable<AppointmentResponse>>(
            response,
            context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier,
            DateTimeOffset.UtcNow));
    }

    private static async Task<IResult> GetAppointmentById(Guid id, AppointmentDbContext dbContext, HttpContext context, ILogger<Program> logger, CancellationToken cancellationToken)
    {
        var appointment = await dbContext.Appointments.FindAsync([id], cancellationToken);
        if (appointment == null)
        {
            logger.LogWarning("Appointment not found with ID: {AppointmentId}", id);
            return Results.NotFound(new ProblemDetails { Title = "Appointment not found", Status = 404 });
        }

        logger.LogInformation("Appointment accessed with ID: {AppointmentId}", id);

        return Results.Ok(new ApiResponse<AppointmentResponse>(
            MapToResponse(appointment),
            context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier,
            DateTimeOffset.UtcNow));
    }

    private static async Task<IResult> GetAppointmentsByPatientId(Guid patientId, AppointmentDbContext dbContext, HttpContext context, CancellationToken cancellationToken)
    {
        var appointments = await dbContext.Appointments
            .Where(a => a.PatientId == patientId)
            .OrderByDescending(a => a.AppointmentDateTime)
            .ToListAsync(cancellationToken);

        var response = appointments.Select(MapToResponse).ToList();
        return Results.Ok(new ApiResponse<IEnumerable<AppointmentResponse>>(
            response,
            context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier,
            DateTimeOffset.UtcNow));
    }

    private static async Task<IResult> GetAppointmentsByProviderId(Guid providerId, AppointmentDbContext dbContext, HttpContext context, CancellationToken cancellationToken)
    {
        var appointments = await dbContext.Appointments
            .Where(a => a.ProviderId == providerId)
            .OrderByDescending(a => a.AppointmentDateTime)
            .ToListAsync(cancellationToken);

        var response = appointments.Select(MapToResponse).ToList();
        return Results.Ok(new ApiResponse<IEnumerable<AppointmentResponse>>(
            response,
            context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier,
            DateTimeOffset.UtcNow));
    }

    private static async Task<IResult> GetAppointmentsByDate(DateTime date, AppointmentDbContext dbContext, HttpContext context, CancellationToken cancellationToken)
    {
        var startOfDay = new DateTimeOffset(date.Date, TimeSpan.Zero);
        var endOfDay = startOfDay.AddDays(1);

        var appointments = await dbContext.Appointments
            .Where(a => a.AppointmentDateTime >= startOfDay && a.AppointmentDateTime < endOfDay)
            .OrderBy(a => a.AppointmentDateTime)
            .ToListAsync(cancellationToken);

        var response = appointments.Select(MapToResponse).ToList();
        return Results.Ok(new ApiResponse<IEnumerable<AppointmentResponse>>(
            response,
            context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier,
            DateTimeOffset.UtcNow));
    }

    private static async Task<IResult> UpdateAppointment(
        Guid id,
        UpdateAppointmentRequest request,
        IValidator<UpdateAppointmentRequest> validator,
        AppointmentDbContext dbContext,
        HttpContext context,
        ILogger<Program> logger,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var appointment = await dbContext.Appointments.FindAsync([id], cancellationToken);
        if (appointment == null)
        {
            return Results.NotFound(new ProblemDetails { Title = "Appointment not found", Status = 404 });
        }

        appointment.AppointmentDateTime = request.AppointmentDateTime;
        appointment.VisitReason = request.VisitReason;
        appointment.Type = request.Type;
        appointment.Notes = request.Notes ?? string.Empty;
        appointment.UpdatedAt = DateTimeOffset.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Appointment updated with ID: {AppointmentId}", appointment.Id);

        return Results.Ok(new ApiResponse<AppointmentResponse>(
            MapToResponse(appointment),
            context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier,
            DateTimeOffset.UtcNow));
    }

    private static async Task<IResult> UpdateAppointmentStatus(
        Guid id,
        UpdateAppointmentStatusRequest request,
        IValidator<UpdateAppointmentStatusRequest> validator,
        AppointmentDbContext dbContext,
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

        var appointment = await dbContext.Appointments.FindAsync([id], cancellationToken);
        if (appointment == null)
        {
            return Results.NotFound(new ProblemDetails { Title = "Appointment not found", Status = 404 });
        }

        if (!AppointmentStatusMachine.CanTransition(appointment.Status, request.Status))
        {
            logger.LogWarning("Invalid status transition attempted for Appointment ID: {AppointmentId} from {CurrentStatus} to {NextStatus}", appointment.Id, appointment.Status, request.Status);
            return Results.BadRequest(new ProblemDetails 
            { 
                Title = "Invalid status transition", 
                Status = 400,
                Detail = $"Cannot transition appointment from '{appointment.Status}' to '{request.Status}'." 
            });
        }

        var oldStatus = appointment.Status;
        appointment.Status = request.Status;
        appointment.UpdatedAt = DateTimeOffset.UtcNow;

        // In a real system, the `request.Reason` and `request.UpdatedBy` would be logged in an audit or status history table. 
        // We will log it cleanly via structured logging.
        logger.LogInformation("Appointment status updated for ID: {AppointmentId} from {OldStatus} to {NewStatus}. Reason: {Reason}, UpdatedBy: {UpdatedBy}", 
            appointment.Id, oldStatus, appointment.Status, request.Reason, request.UpdatedBy);

        await dbContext.SaveChangesAsync(cancellationToken);

        await auditLogger.LogEventAsync(
            eventType: "AppointmentStatusChanged",
            entityType: "Appointment",
            entityId: appointment.Id.ToString(),
            action: "UpdateAppointmentStatus",
            outcome: "Success",
            summary: $"Appointment status was transitioned from {oldStatus} to {appointment.Status}.",
            metadata: new { OldStatus = oldStatus.ToString(), NewStatus = appointment.Status.ToString(), Reason = request.Reason },
            patientId: appointment.PatientId.ToString(),
            providerId: appointment.ProviderId.ToString(),
            actorType: "User",
            actorId: request.UpdatedBy ?? "Unknown",
            cancellationToken: cancellationToken);

        return Results.Ok(new ApiResponse<AppointmentResponse>(
            MapToResponse(appointment),
            context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier,
            DateTimeOffset.UtcNow));
    }

    private static AppointmentResponse MapToResponse(Domain.Appointment appointment) => new(
        appointment.Id,
        appointment.PatientId,
        appointment.ProviderId,
        appointment.AppointmentDateTime,
        appointment.VisitReason,
        appointment.Type,
        appointment.Status,
        appointment.Notes,
        appointment.CreatedAt,
        appointment.UpdatedAt);
}
