using Newtonsoft.Json;

namespace M1.Core.Integrations;

public class IntegrationConfigurationRegion
{
	[JsonProperty("webAppsApiUrl")]
	public string WebAppsApiUrl { get; set; }

	[JsonProperty("financeIntegrationUrl")]
	public string FinanceIntegrationUrl { get; set; }

	[JsonProperty("auth0Domain")]
	public string Auth0Domain { get; set; }

	[JsonProperty("wmsClientId")]
	public string WmsClientId { get; set; }

	[JsonProperty("clientId")]
	public string ClientId { get; set; } = "n7WSVl7Fdi0H8VE2cM6658NU0Dp6oB2J";

	[JsonProperty("displayName")]
	public string DisplayName { get; set; }
}
