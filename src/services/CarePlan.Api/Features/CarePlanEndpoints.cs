using FluentValidation;
using HealthcareCareCoordination.CarePlan.Api.Domain;
using HealthcareCareCoordination.CarePlan.Api.DTOs;
using HealthcareCareCoordination.CarePlan.Api.Infrastructure;
using HealthcareCareCoordination.SharedKernel;
using HealthcareCareCoordination.Observability;
using HealthcareCareCoordination.Security;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareCareCoordination.CarePlan.Api.Features;

public static class CarePlanEndpoints
{
    public static void MapCarePlanEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/v1/care-plans");

        group.MapPost("/", CreateCarePlan)
             .RequireAuthorization(HealthcarePermissions.CarePlanWrite);
             
        group.MapGet("/", GetCarePlans)
             .RequireAuthorization(HealthcarePermissions.CarePlanRead);
             
        group.MapGet("/{id:guid}", GetCarePlanById)
             .RequireAuthorization(HealthcarePermissions.CarePlanRead);
             
        group.MapPatch("/{id:guid}/status", UpdateCarePlanStatus)
             .RequireAuthorization(HealthcarePermissions.CarePlanStatusUpdate);
        
        group.MapPost("/{id:guid}/goals", AddGoal)
             .RequireAuthorization(HealthcarePermissions.CarePlanWrite);
             
        group.MapPatch("/{id:guid}/goals/{goalId:guid}/status", UpdateGoalStatus)
             .RequireAuthorization(HealthcarePermissions.CarePlanWrite);
        
        group.MapPost("/{id:guid}/tasks", AddTask)
             .RequireAuthorization(HealthcarePermissions.FollowUpTaskWrite);
             
        group.MapPatch("/{id:guid}/tasks/{taskId:guid}/status", UpdateTaskStatus)
             .RequireAuthorization(HealthcarePermissions.FollowUpTaskStatusUpdate);

        // Sub-resources mapping to Patient/Provider domains
        routes.MapGet("/api/v1/patients/{patientId:guid}/care-plans", GetCarePlansByPatientId)
              .RequireAuthorization(HealthcarePermissions.CarePlanRead);
              
        routes.MapGet("/api/v1/providers/{providerId:guid}/care-plans", GetCarePlansByProviderId)
              .RequireAuthorization(HealthcarePermissions.CarePlanRead);
    }

    private static async Task<IResult> CreateCarePlan(
        CreateCarePlanRequest request,
        IValidator<CreateCarePlanRequest> validator,
        ICarePlanRepository repository,
        HttpContext context,
        ILogger<Program> logger)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var carePlan = new CarePlanDocument
        {
            PatientId = request.PatientId,
            ProviderId = request.ProviderId,
            RelatedAppointmentId = request.RelatedAppointmentId,
            Title = request.Title,
            ClinicalSummary = request.ClinicalSummary,
            Instructions = request.Instructions,
            FollowUpDate = request.FollowUpDate,
            Status = CarePlanStatus.Draft,
            Goals = request.Goals.Select(g => new CarePlanGoal 
            { 
                Description = g.Description, 
                TargetDate = g.TargetDate, 
                Priority = g.Priority 
            }).ToList(),
            Tasks = request.Tasks.Select(t => new CarePlanTask 
            { 
                TaskTitle = t.TaskTitle, 
                TaskDescription = t.TaskDescription, 
                DueDate = t.DueDate, 
                Priority = t.Priority, 
                AssignedTo = t.AssignedTo 
            }).ToList()
        };

        var created = await repository.CreateAsync(carePlan);

        // Privacy rule: Do not log full clinical summaries
        logger.LogInformation("Care Plan created with ID: {CarePlanId} for Patient: {PatientId}", created.Id, created.PatientId);

        return Results.Created($"/api/v1/care-plans/{created.Id}", new ApiResponse<CarePlanDocument>(
            created,
            context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier,
            DateTimeOffset.UtcNow));
    }

    private static async Task<IResult> GetCarePlans(ICarePlanRepository repository, HttpContext context)
    {
        var plans = await repository.GetAllAsync();
        return Results.Ok(new ApiResponse<IEnumerable<CarePlanDocument>>(
            plans,
            context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier,
            DateTimeOffset.UtcNow));
    }

    private static async Task<IResult> GetCarePlanById(Guid id, ICarePlanRepository repository, HttpContext context, ILogger<Program> logger)
    {
        var plan = await repository.GetByIdAsync(id);
        if (plan == null)
        {
            return Results.NotFound(new ProblemDetails { Title = "Care Plan not found", Status = 404 });
        }

        return Results.Ok(new ApiResponse<CarePlanDocument>(
            plan,
            context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier,
            DateTimeOffset.UtcNow));
    }

    private static async Task<IResult> GetCarePlansByPatientId(Guid patientId, ICarePlanRepository repository, HttpContext context)
    {
        var plans = await repository.GetByPatientIdAsync(patientId);
        return Results.Ok(new ApiResponse<IEnumerable<CarePlanDocument>>(
            plans,
            context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier,
            DateTimeOffset.UtcNow));
    }

    private static async Task<IResult> GetCarePlansByProviderId(Guid providerId, ICarePlanRepository repository, HttpContext context)
    {
        var plans = await repository.GetByProviderIdAsync(providerId);
        return Results.Ok(new ApiResponse<IEnumerable<CarePlanDocument>>(
            plans,
            context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier,
            DateTimeOffset.UtcNow));
    }

    private static async Task<IResult> UpdateCarePlanStatus(
        Guid id,
        UpdateCarePlanStatusRequest request,
        IValidator<UpdateCarePlanStatusRequest> validator,
        ICarePlanRepository repository,
        HttpContext context,
        ILogger<Program> logger)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid) return Results.ValidationProblem(validationResult.ToDictionary());

        var plan = await repository.GetByIdAsync(id);
        if (plan == null) return Results.NotFound(new ProblemDetails { Title = "Care Plan not found", Status = 404 });

        if (!CarePlanStatusMachine.CanTransition(plan.Status, request.Status))
        {
            logger.LogWarning("Invalid transition from {OldStatus} to {NewStatus} on CarePlan {CarePlanId}", plan.Status, request.Status, plan.Id);
            return Results.BadRequest(new ProblemDetails { Title = "Invalid status transition", Status = 400 });
        }

        plan.Status = request.Status;
        var updated = await repository.UpdateAsync(plan);

        logger.LogInformation("Care Plan {CarePlanId} status updated to {Status}", updated.Id, updated.Status);

        return Results.Ok(new ApiResponse<CarePlanDocument>(
            updated,
            context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier,
            DateTimeOffset.UtcNow));
    }

    private static async Task<IResult> AddGoal(
        Guid id,
        AddCarePlanGoalRequest request,
        IValidator<AddCarePlanGoalRequest> validator,
        ICarePlanRepository repository,
        HttpContext context)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid) return Results.ValidationProblem(validationResult.ToDictionary());

        var plan = await repository.GetByIdAsync(id);
        if (plan == null) return Results.NotFound(new ProblemDetails { Title = "Care Plan not found", Status = 404 });

        plan.Goals.Add(new CarePlanGoal
        {
            Description = request.Description,
            TargetDate = request.TargetDate,
            Priority = request.Priority
        });

        var updated = await repository.UpdateAsync(plan);

        return Results.Ok(new ApiResponse<CarePlanDocument>(
            updated,
            context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier,
            DateTimeOffset.UtcNow));
    }

    private static async Task<IResult> UpdateGoalStatus(
        Guid id,
        Guid goalId,
        UpdateCarePlanGoalStatusRequest request,
        IValidator<UpdateCarePlanGoalStatusRequest> validator,
        ICarePlanRepository repository,
        HttpContext context)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid) return Results.ValidationProblem(validationResult.ToDictionary());

        var plan = await repository.GetByIdAsync(id);
        if (plan == null) return Results.NotFound(new ProblemDetails { Title = "Care Plan not found", Status = 404 });

        var goal = plan.Goals.FirstOrDefault(g => g.GoalId == goalId);
        if (goal == null) return Results.NotFound(new ProblemDetails { Title = "Goal not found", Status = 404 });

        goal.Status = request.Status;
        var updated = await repository.UpdateAsync(plan);

        return Results.Ok(new ApiResponse<CarePlanDocument>(
            updated,
            context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier,
            DateTimeOffset.UtcNow));
    }

    private static async Task<IResult> AddTask(
        Guid id,
        AddCarePlanTaskRequest request,
        IValidator<AddCarePlanTaskRequest> validator,
        ICarePlanRepository repository,
        HttpContext context)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid) return Results.ValidationProblem(validationResult.ToDictionary());

        var plan = await repository.GetByIdAsync(id);
        if (plan == null) return Results.NotFound(new ProblemDetails { Title = "Care Plan not found", Status = 404 });

        plan.Tasks.Add(new CarePlanTask
        {
            TaskTitle = request.TaskTitle,
            TaskDescription = request.TaskDescription,
            DueDate = request.DueDate,
            Priority = request.Priority,
            AssignedTo = request.AssignedTo
        });

        var updated = await repository.UpdateAsync(plan);

        return Results.Ok(new ApiResponse<CarePlanDocument>(
            updated,
            context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier,
            DateTimeOffset.UtcNow));
    }

    private static async Task<IResult> UpdateTaskStatus(
        Guid id,
        Guid taskId,
        UpdateCarePlanTaskStatusRequest request,
        IValidator<UpdateCarePlanTaskStatusRequest> validator,
        ICarePlanRepository repository,
        HttpContext context)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid) return Results.ValidationProblem(validationResult.ToDictionary());

        var plan = await repository.GetByIdAsync(id);
        if (plan == null) return Results.NotFound(new ProblemDetails { Title = "Care Plan not found", Status = 404 });

        var task = plan.Tasks.FirstOrDefault(t => t.TaskId == taskId);
        if (task == null) return Results.NotFound(new ProblemDetails { Title = "Task not found", Status = 404 });

        task.Status = request.Status;
        if (request.Status == Domain.TaskStatus.Completed)
        {
            task.CompletedTimestamp = DateTimeOffset.UtcNow;
        }

        var updated = await repository.UpdateAsync(plan);

        return Results.Ok(new ApiResponse<CarePlanDocument>(
            updated,
            context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier,
            DateTimeOffset.UtcNow));
    }
}
