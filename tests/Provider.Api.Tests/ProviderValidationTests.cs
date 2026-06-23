using FluentValidation.TestHelper;
using HealthcareCareCoordination.Provider.Api.DTOs;
using HealthcareCareCoordination.Provider.Api.Domain;
using HealthcareCareCoordination.Provider.Api.Features.Validation;
using Xunit;

namespace Provider.Api.Tests;

/// <summary>
/// Validates that provider registration and update rules reject invalid input.
/// All test data is synthetic — no real provider information is used.
/// </summary>
public class ProviderValidationTests
{
    private readonly RegisterProviderRequestValidator _registerValidator = new();
    private readonly UpdateProviderRequestValidator _updateValidator = new();

    [Fact]
    public void Register_WhenFullNameIsEmpty_ShouldFail()
    {
        var request = ValidRegisterRequest() with { FullName = "" };
        var result = _registerValidator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.FullName);
    }

    [Fact]
    public void Register_WhenEmailIsInvalid_ShouldFail()
    {
        var request = ValidRegisterRequest() with { Email = "not-an-email" };
        var result = _registerValidator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Register_WhenDepartmentIsEmpty_ShouldFail()
    {
        var request = ValidRegisterRequest() with { Department = "" };
        var result = _registerValidator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Department);
    }

    [Fact]
    public void Register_WhenMobileNumberIsEmpty_ShouldFail()
    {
        var request = ValidRegisterRequest() with { MobileNumber = "" };
        var result = _registerValidator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.MobileNumber);
    }

    [Fact]
    public void Register_WhenAllFieldsAreValid_ShouldPass()
    {
        var result = _registerValidator.TestValidate(ValidRegisterRequest());
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Update_WhenEmailIsInvalid_ShouldFail()
    {
        var request = ValidUpdateRequest() with { Email = "bad@" };
        var result = _updateValidator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Update_WhenFullNameExceedsMaxLength_ShouldFail()
    {
        var request = ValidUpdateRequest() with { FullName = new string('X', 201) };
        var result = _updateValidator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.FullName);
    }

    [Fact]
    public void Update_WhenAllFieldsAreValid_ShouldPass()
    {
        var result = _updateValidator.TestValidate(ValidUpdateRequest());
        result.ShouldNotHaveAnyValidationErrors();
    }

    // ---------------------------------------------------------------------------
    // Synthetic test data builders
    // ---------------------------------------------------------------------------

    private static RegisterProviderRequest ValidRegisterRequest() =>
        new(
            FullName: "Synthetic Demo Provider",
            Specialty: Specialty.GeneralMedicine,
            Email: "synthetic.provider@demo.local",
            MobileNumber: "+44-000-000-0010",
            Department: "Demo General Medicine",
            AvailabilityStatus: AvailabilityStatus.Available);

    private static UpdateProviderRequest ValidUpdateRequest() =>
        new(
            FullName: "Synthetic Demo Provider Updated",
            Specialty: Specialty.Cardiology,
            Email: "updated.provider@demo.local",
            MobileNumber: "+44-000-000-0011",
            Department: "Demo Cardiology");
}
