using HealthcareCareCoordination.ClinicalAI;
using Xunit;

public sealed class FoundationTests
{
    [Fact]
    public async Task MockAnalyzerIncludesHumanReviewNotice()
    {
        var analyzer = new MockClinicalTextAnalyzer();
        var result = await analyzer.AnalyzeAsync(new ClinicalNoteRequest("patient-1", "Synthetic note only.", "corr-1"));
        Assert.Contains("reviewed by a qualified healthcare professional", result.HumanReviewNotice);
    }
}
