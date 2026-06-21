namespace HealthcareCareCoordination.Security;

public class SecurityConfiguration
{
    public string Mode { get; set; } = "Demo";
    public bool AllowDemoHeaders { get; set; } = true;
    public string DefaultDemoRole { get; set; } = "CareCoordinator";
    public bool RequireAuthenticationInProduction { get; set; } = true;
    
    public AzureEntraIdConfiguration AzureEntraId { get; set; } = new();
}

public class AzureEntraIdConfiguration
{
    public string TenantId { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string Authority { get; set; } = string.Empty;
}
