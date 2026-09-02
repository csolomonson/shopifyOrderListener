using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace M1.Core.Integrations;

public static class IntegrationConfigurationDiscovery
{
	public static async Task<IntegrationConfigurationRegion> GetIntegrationConfigurationRegionAsync(string discoveryUrl, string environmentName = "Production", string regionName = "US")
	{
		IntegrationServiceConfiguration integrationServiceConfiguration = await GetIntegrationServiceConfigurationAsync(discoveryUrl, environmentName).ConfigureAwait(continueOnCapturedContext: false);
		if (integrationServiceConfiguration.Regions.TryGetValue(regionName, out var value))
		{
			return value;
		}
		if (integrationServiceConfiguration.DefaultRegion != null && integrationServiceConfiguration.Regions.TryGetValue(integrationServiceConfiguration.DefaultRegion, out var value2))
		{
			return value2;
		}
		return integrationServiceConfiguration.Default;
	}

	public static async Task<IntegrationServiceConfiguration> GetIntegrationServiceConfigurationAsync(string discoveryUrl, string environmentName = "Production")
	{
		string settingsUrl = GetSettingsUrl(environmentName, discoveryUrl);
		HttpClient remoteSettingsClient = new HttpClient();
		try
		{
			return JsonConvert.DeserializeObject<IntegrationServiceConfiguration>(await remoteSettingsClient.GetStringAsync(settingsUrl).ConfigureAwait(continueOnCapturedContext: false));
		}
		catch (Exception innerException)
		{
			throw new Exception("Could not get settings from " + settingsUrl + " for Environment: " + environmentName, innerException);
		}
		finally
		{
			((IDisposable)remoteSettingsClient)?.Dispose();
		}
	}

	private static string GetSettingsUrl(string environmentName, string discoveryUrl)
	{
		string text = "integration.json";
		if (environmentName == "Production")
		{
			text = "integration.json";
		}
		else if (!string.IsNullOrWhiteSpace(environmentName))
		{
			text = "integration-" + environmentName + ".json";
		}
		return discoveryUrl + ".well-known/config/v1.0/" + text;
	}
}
