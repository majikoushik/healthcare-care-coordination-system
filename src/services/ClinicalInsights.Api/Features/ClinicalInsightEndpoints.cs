using FluentValidation;
using HealthcareCareCoordination.ClinicalInsights.Api.Domain;
using HealthcareCareCoordination.ClinicalInsights.Api.DTOs;
using HealthcareCareCoordination.ClinicalInsights.Api.Infrastructure;
using HealthcareCareCoordination.ClinicalAI;
using HealthcareCareCoordination.Observability;
using HealthcareCareCoordination.SharedKernel;
using HealthcareCareCoordination.Security;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareCareCoordination.ClinicalInsights.Api.Features;

public static class ClinicalInsightEndpoints
{
    public static void MapClinicalInsightEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1").WithTags("ClinicalInsights");

        group.MapPost("/clinical-insights/analyze", async (
            [FromBody] AnalyzeNoteRequest request,
            [FromServices] IValidator<AnalyzeNoteRequest> validator,
            [FromServices] IClinicalTextAnalyzer analyzer,
            [FromServices] IClinicalInsightRepository repository,
            ILogger<Program> logger,
            HttpContext context,
            CancellationToken cancellationToken) =>
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
                return Results.Problem(new ProblemDetails
                {
                    Title = "Validation failed",
                    Status = 400,
                    Detail = "One or more validation errors occurred.",
                    Extensions = { ["errors"] = errors }
                });
            }

            var correlationId = context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier;

            logger.LogInformation("Analyzing clinical note for PatientId {PatientId}. CorrelationId: {CorrelationId}", 
                request.PatientId, correlationId);
            
            // Invoke AI Provider (Mock)
            var aiRequest = new ClinicalNoteRequest(request.PatientId, request.ClinicalNoteText, correlationId);
            var aiResult = await analyzer.AnalyzeAsync(aiRequest, cancellationToken);

            var insight = new ClinicalNoteInsight
            {
                PatientId = request.PatientId,
                ProviderId = request.ProviderId,
                RelatedCarePlanId = request.RelatedCarePlanId,
                RelatedAppointmentId = request.RelatedAppointmentId,
                ClinicalNoteText = request.ClinicalNoteText, // Synthetic data stored in DB
                
                // Map the generic string terms to our API's rich entity structure for the UI
                ExtractedEntities = aiResult.ExtractedTerms.Select(t => new ExtractedEntity { Text = t, Category = EntityCategory.Unknown, ConfidenceScore = 0.9 }).ToList(),
                RiskIndicators = new List<string>(), // Handled by follow-up suggestions in the AI provider
                SuggestedFollowUpTopics = aiResult.FollowUpSuggestionsForReview.ToList(),
                
                AiProviderName = aiResult.ProviderName,
                AiProviderMode = AiProviderMode.Mock,
                HumanReviewStatus = HumanReviewStatus.PendingReview
            };

            var savedInsight = await repository.CreateAsync(insight, cancellationToken);

            return Results.Created($"/api/v1/clinical-insights/{savedInsight.Id}", 
                new ApiResponse<ClinicalNoteInsight>(savedInsight, correlationId, DateTimeOffset.UtcNow));
        })
        .RequireAuthorization(HealthcarePermissions.ClinicalInsightAnalyze);

        group.MapGet("/clinical-insights", async (
            [FromServices] IClinicalInsightRepository repository,
            HttpContext context,
            CancellationToken cancellationToken) =>
        {
            var insights = await repository.GetAllAsync(cancellationToken);
            var correlationId = context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier;
            return Results.Ok(new ApiResponse<IEnumerable<ClinicalNoteInsight>>(insights, correlationId, DateTimeOffset.UtcNow));
        })
        .RequireAuthorization(HealthcarePermissions.ClinicalInsightRead);

        group.MapGet("/clinical-insights/{id}", async (
            string id,
            [FromServices] IClinicalInsightRepository repository,
            HttpContext context,
            CancellationToken cancellationToken) =>
        {
            var insight = await repository.GetByIdAsync(id, cancellationToken);
            if (insight == null) return Results.NotFound(new ProblemDetails { Title = "Insight not found", Status = 404 });

            var correlationId = context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier;
            return Results.Ok(new ApiResponse<ClinicalNoteInsight>(insight, correlationId, DateTimeOffset.UtcNow));
        })
        .RequireAuthorization(HealthcarePermissions.ClinicalInsightRead);

        group.MapGet("/patients/{patientId}/clinical-insights", async (
            string patientId,
            [FromServices] IClinicalInsightRepository repository,
            HttpContext context,
            CancellationToken cancellationToken) =>
        {
            var insights = await repository.GetByPatientIdAsync(patientId, cancellationToken);
            var correlationId = context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier;
            return Results.Ok(new ApiResponse<IEnumerable<ClinicalNoteInsight>>(insights, correlationId, DateTimeOffset.UtcNow));
        })
        .RequireAuthorization(HealthcarePermissions.ClinicalInsightRead);

        group.MapGet("/care-plans/{carePlanId}/clinical-insights", async (
            string carePlanId,
            [FromServices] IClinicalInsightRepository repository,
            HttpContext context,
            CancellationToken cancellationToken) =>
        {
            var insights = await repository.GetByCarePlanIdAsync(carePlanId, cancellationToken);
            var correlationId = context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier;
            return Results.Ok(new ApiResponse<IEnumerable<ClinicalNoteInsight>>(insights, correlationId, DateTimeOffset.UtcNow));
        })
        .RequireAuthorization(HealthcarePermissions.ClinicalInsightRead);

        group.MapPatch("/clinical-insights/{id}/review-status", async (
            string id,
            [FromBody] UpdateReviewStatusRequest request,
            [FromServices] IValidator<UpdateReviewStatusRequest> validator,
            [FromServices] IClinicalInsightRepository repository,
            ILogger<Program> logger,
            HttpContext context,
            CancellationToken cancellationToken) =>
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
                return Results.Problem(new ProblemDetails
                {
                    Title = "Validation failed",
                    Status = 400,
                    Detail = "One or more validation errors occurred.",
                    Extensions = { ["errors"] = errors }
                });
            }

            var insight = await repository.GetByIdAsync(id, cancellationToken);
            if (insight == null) return Results.NotFound(new ProblemDetails { Title = "Insight not found", Status = 404 });

            var correlationId = context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier;

            logger.LogInformation("Updating ClinicalInsight {InsightId} review status to {Status} by {ReviewedBy}. CorrelationId: {CorrelationId}", 
                id, request.ReviewStatus, request.ReviewedBy, correlationId);

            insight.HumanReviewStatus = request.ReviewStatus;
            insight.ReviewedBy = request.ReviewedBy;
            insight.ReviewedTimestamp = DateTimeOffset.UtcNow;
            
            await repository.UpdateAsync(insight, cancellationToken);

            return Results.Ok(new ApiResponse<ClinicalNoteInsight>(insight, correlationId, DateTimeOffset.UtcNow));
        })
        .RequireAuthorization(HealthcarePermissions.ClinicalInsightReview);

        group.MapGet("/clinical-insights/ai-provider/status", (
            [FromServices] Microsoft.Extensions.Options.IOptions<HealthcareCareCoordination.ClinicalAI.ClinicalAIConfiguration> options,
            [FromServices] HealthcareCareCoordination.ClinicalAI.IClinicalTextAnalyzer activeAnalyzer) =>
        {
            var config = options.Value;
            var isConfigured = !string.IsNullOrEmpty(config.AzureLanguage.Endpoint);
            
            var status = new
            {
                providerMode = config.ProviderMode,
                azureLanguageConfigured = isConfigured,
                activeProvider = activeAnalyzer.GetType().Name,
                message = isConfigured && config.ProviderMode == "AzureAIConfigured" 
                    ? "System is configured for Azure AI Language."
                    : "Local development is using the mock clinical text analyzer."
            };

            return Results.Ok(status);
        })
        .RequireAuthorization(HealthcarePermissions.ClinicalInsightRead);
    }
}
