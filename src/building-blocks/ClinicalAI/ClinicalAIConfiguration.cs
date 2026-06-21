namespace HealthcareCareCoordination.ClinicalAI;

public class ClinicalAIConfiguration
{
    public const string SectionName = "ClinicalAI";

    public string ProviderMode { get; set; } = "Mock"; // Mock, AzureAIReady, AzureAIConfigured
    public AzureLanguageConfig AzureLanguage { get; set; } = new();
}

public class AzureLanguageConfig
{
    public string Endpoint { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string ModelVersion { get; set; } = "latest";
}
