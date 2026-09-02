using System.Collections.Generic;
using Newtonsoft.Json;

namespace M1.Core.Integrations;

public class IntegrationServiceConfiguration
{
	[JsonProperty("default")]
	public IntegrationConfigurationRegion Default { get; set; }

	[JsonProperty("defaultRegion")]
	public string DefaultRegion { get; set; }

	[JsonProperty("regions")]
	public IDictionary<string, IntegrationConfigurationRegion> Regions { get; set; }
}
