using FluentValidation.TestHelper;
using HealthcareCareCoordination.Patient.Api.Domain;
using HealthcareCareCoordination.Patient.Api.DTOs;
using HealthcareCareCoordination.Patient.Api.Features.Validation;
using Xunit;

namespace Patient.Api.Tests;

/// <summary>
/// Validates that patient registration and update rules reject invalid input.
/// All test data is synthetic — no real patient information is used.
/// </summary>
public class PatientValidationTests
{
    private readonly RegisterPatientRequestValidator _registerValidator = new();
    private readonly UpdatePatientRequestValidator _updateValidator = new();

    // ---------------------------------------------------------------------------
    // RegisterPatientRequest — required field validation
    // ---------------------------------------------------------------------------

    [Fact]
    public void Register_WhenFullNameIsEmpty_ShouldFail()
    {
        var request = ValidRegisterRequest() with { FullName = "" };
        var result = _registerValidator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.FullName);
    }

    [Fact]
    public void Register_WhenFullNameExceedsMaxLength_ShouldFail()
    {
        var request = ValidRegisterRequest() with { FullName = new string('A', 201) };
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
    public void Register_WhenEmailIsEmpty_ShouldFail()
    {
        var request = ValidRegisterRequest() with { Email = "" };
        var result = _registerValidator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Register_WhenDateOfBirthIsInTheFuture_ShouldFail()
    {
        var request = ValidRegisterRequest() with { DateOfBirth = DateTime.UtcNow.AddDays(1) };
        var result = _registerValidator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.DateOfBirth);
    }

    [Fact]
    public void Register_WhenMobileNumberIsEmpty_ShouldFail()
    {
        var request = ValidRegisterRequest() with { MobileNumber = "" };
        var result = _registerValidator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.MobileNumber);
    }

    [Fact]
    public void Register_WhenAddressIsEmpty_ShouldFail()
    {
        var request = ValidRegisterRequest() with { Address = "" };
        var result = _registerValidator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Address);
    }

    [Fact]
    public void Register_WhenEmergencyContactNameIsEmpty_ShouldFail()
    {
        var request = ValidRegisterRequest() with { EmergencyContactName = "" };
        var result = _registerValidator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.EmergencyContactName);
    }

    [Fact]
    public void Register_WhenAllFieldsAreValid_ShouldPass()
    {
        var result = _registerValidator.TestValidate(ValidRegisterRequest());
        result.ShouldNotHaveAnyValidationErrors();
    }

    // ---------------------------------------------------------------------------
    // UpdatePatientRequest — required field validation
    // ---------------------------------------------------------------------------

    [Fact]
    public void Update_WhenEmailIsInvalid_ShouldFail()
    {
        var request = ValidUpdateRequest() with { Email = "bad@" };
        var result = _updateValidator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Email);
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

    private static RegisterPatientRequest ValidRegisterRequest() =>
        new(
            FullName: "Synthetic Demo Patient",
            DateOfBirth: new DateTime(1985, 6, 15),
            Gender: Gender.Male,
            Email: "synthetic.patient@demo.local",
            MobileNumber: "+44-000-000-0000",
            Address: "1 Demo Street, Synthetic City, SC1 0AA",
            EmergencyContactName: "Demo Emergency Contact",
            EmergencyContactNumber: "+44-000-000-0001",
            ConsentStatus: ConsentStatus.Provided);

    private static UpdatePatientRequest ValidUpdateRequest() =>
        new(
            FullName: "Synthetic Demo Patient Updated",
            Email: "updated.patient@demo.local",
            MobileNumber: "+44-000-000-0002",
            Address: "2 Demo Street, Synthetic City, SC1 0BB",
            EmergencyContactName: "Demo Emergency Contact",
            EmergencyContactNumber: "+44-000-000-0001");
}
