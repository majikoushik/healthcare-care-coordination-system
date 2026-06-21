using FluentValidation;
using HealthcareCareCoordination.CarePlan.Api.Domain;
using HealthcareCareCoordination.CarePlan.Api.DTOs;
using HealthcareCareCoordination.CarePlan.Api.Infrastructure;
using HealthcareCareCoordination.SharedKernel;
using HealthcareCareCoordination.Observability;
using Microsoft.AspNetCore.Mvc;
using TaskStatus = HealthcareCareCoordination.CarePlan.Api.Domain.FollowUpTaskStatus;

namespace HealthcareCareCoordination.CarePlan.Api.Features;

public static class FollowUpTaskEndpoints
{
    public static void MapFollowUpTaskEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/v1/follow-up-tasks");

        group.MapPost("/", CreateTask);
        group.MapGet("/", GetTasks);
        group.MapGet("/{id:guid}", GetTaskById);
        group.MapPatch("/{id:guid}/status", UpdateTaskStatus);
        
        group.MapGet("/due-today", GetDueToday);
        group.MapGet("/overdue", GetOverdue);

        // Sub-resources
        routes.MapGet("/api/v1/patients/{patientId:guid}/follow-up-tasks", GetByPatientId);
        routes.MapGet("/api/v1/care-plans/{carePlanId:guid}/follow-up-tasks", GetByCarePlanId);
        routes.MapGet("/api/v1/clinical-insights/{clinicalInsightId:guid}/follow-up-tasks", GetByClinicalInsightId);
    }

    private static async Task<IResult> CreateTask(
        CreateFollowUpTaskRequest request,
        IValidator<CreateFollowUpTaskRequest> validator,
        IFollowUpTaskRepository repository,
        HttpContext context,
        ILogger<Program> logger)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Results.ValidationProblem(validationResult.ToDictionary());

        var document = new FollowUpTaskDocument
        {
            PatientId = request.PatientId,
            ProviderId = request.ProviderId,
            CarePlanId = request.CarePlanId,
            ClinicalInsightId = request.ClinicalInsightId,
            AppointmentId = request.AppointmentId,
            Title = request.Title,
            Description = request.Description,
            TaskType = request.TaskType,
            Priority = request.Priority,
            DueDate = request.DueDate,
            AssignedTo = request.AssignedTo,
            Status = TaskStatus.Pending
        };

        var created = await repository.CreateAsync(document);

        // Privacy rule: Do not log full description
        logger.LogInformation("Follow-up Task created with ID: {TaskId} for Patient: {PatientId}", created.Id, created.PatientId);

        return Results.Created($"/api/v1/follow-up-tasks/{created.Id}", new ApiResponse<FollowUpTaskDocument>(
            created,
            context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier,
            DateTimeOffset.UtcNow));
    }

    private static async Task<IResult> GetTasks(IFollowUpTaskRepository repository, HttpContext context)
    {
        var tasks = await repository.GetAllAsync();
        return Results.Ok(new ApiResponse<IEnumerable<FollowUpTaskDocument>>(
            tasks,
            context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier,
            DateTimeOffset.UtcNow));
    }

    private static async Task<IResult> GetTaskById(Guid id, IFollowUpTaskRepository repository, HttpContext context)
    {
        var task = await repository.GetByIdAsync(id);
        if (task == null) return Results.NotFound(new ProblemDetails { Title = "Follow-up task not found", Status = 404 });

        return Results.Ok(new ApiResponse<FollowUpTaskDocument>(
            task,
            context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier,
            DateTimeOffset.UtcNow));
    }

    private static async Task<IResult> GetDueToday(IFollowUpTaskRepository repository, HttpContext context)
    {
        var tasks = await repository.GetDueTodayAsync();
        return Results.Ok(new ApiResponse<IEnumerable<FollowUpTaskDocument>>(
            tasks,
            context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier,
            DateTimeOffset.UtcNow));
    }

    private static async Task<IResult> GetOverdue(IFollowUpTaskRepository repository, HttpContext context)
    {
        var tasks = await repository.GetOverdueAsync();
        return Results.Ok(new ApiResponse<IEnumerable<FollowUpTaskDocument>>(
            tasks,
            context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier,
            DateTimeOffset.UtcNow));
    }

    private static async Task<IResult> GetByPatientId(Guid patientId, IFollowUpTaskRepository repository, HttpContext context)
    {
        var tasks = await repository.GetByPatientIdAsync(patientId);
        return Results.Ok(new ApiResponse<IEnumerable<FollowUpTaskDocument>>(
            tasks,
            context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier,
            DateTimeOffset.UtcNow));
    }

    private static async Task<IResult> GetByCarePlanId(Guid carePlanId, IFollowUpTaskRepository repository, HttpContext context)
    {
        var tasks = await repository.GetByCarePlanIdAsync(carePlanId);
        return Results.Ok(new ApiResponse<IEnumerable<FollowUpTaskDocument>>(
            tasks,
            context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier,
            DateTimeOffset.UtcNow));
    }

    private static async Task<IResult> GetByClinicalInsightId(Guid clinicalInsightId, IFollowUpTaskRepository repository, HttpContext context)
    {
        var tasks = await repository.GetByClinicalInsightIdAsync(clinicalInsightId);
        return Results.Ok(new ApiResponse<IEnumerable<FollowUpTaskDocument>>(
            tasks,
            context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier,
            DateTimeOffset.UtcNow));
    }

    private static async Task<IResult> UpdateTaskStatus(
        Guid id,
        UpdateFollowUpTaskStatusRequest request,
        IValidator<UpdateFollowUpTaskStatusRequest> validator,
        IFollowUpTaskRepository repository,
        HttpContext context,
        ILogger<Program> logger)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid) return Results.ValidationProblem(validationResult.ToDictionary());

        var task = await repository.GetByIdAsync(id);
        if (task == null) return Results.NotFound(new ProblemDetails { Title = "Follow-up task not found", Status = 404 });

        if (!FollowUpTaskStatusMachine.CanTransition(task.Status, request.Status))
        {
            logger.LogWarning("Invalid transition from {OldStatus} to {NewStatus} on FollowUpTask {TaskId}", task.Status, request.Status, task.Id);
            return Results.BadRequest(new ProblemDetails { Title = "Invalid status transition", Status = 400 });
        }

        task.Status = request.Status;
        if (request.Status == TaskStatus.Completed)
        {
            task.CompletedTimestamp = DateTimeOffset.UtcNow;
            task.CompletionNotes = request.CompletionNotes;
        }

        var updated = await repository.UpdateAsync(task);

        logger.LogInformation("Follow-up Task {TaskId} status updated to {Status}", updated.Id, updated.Status);

        return Results.Ok(new ApiResponse<FollowUpTaskDocument>(
            updated,
            context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier,
            DateTimeOffset.UtcNow));
    }
}
