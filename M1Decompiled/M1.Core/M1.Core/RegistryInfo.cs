using Microsoft.Win32;

namespace M1.Core;

public class RegistryInfo
{
	private const string M1_KEY = "HKEY_LOCAL_MACHINE\\SOFTWARE\\ECi\\M1";

	private const string INTEGRATION_SERVICE_KEY = "HKEY_LOCAL_MACHINE\\SOFTWARE\\ECi Software Solutions\\IntegrationService";

	public string FileServer { get; private set; }

	public string MetadataDB { get; private set; }

	public string MetadataServer { get; private set; }

	public string NetworkLibrary { get; private set; }

	public string API_SSL_CONFIGURED { get; private set; }

	public string API_TCP_PORT { get; private set; } = "80";

	public string API_LOG_PATH { get; set; }

	public string MOBILE_SERVICE_LOG_PATH { get; set; }

	public string INTEGRATION_SERVICE_PROGRAM_LOCATION { get; set; }

	public string ENVIRONMENT_NAME { get; }

	public string ENVIRONMENT_REGION { get; }

	public string DISCOVERY_URL { get; }

	public RegistryInfo()
	{
		FileServer = getValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\ECi\\M1", "FileServer");
		MetadataDB = getValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\ECi\\M1", "MetadataDB");
		MetadataServer = getValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\ECi\\M1", "MetadataServer");
		NetworkLibrary = getValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\ECi\\M1", "NetworkLibrary");
		API_SSL_CONFIGURED = getValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\ECi\\M1", "API_SSL_CONFIGURED");
		API_TCP_PORT = getValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\ECi\\M1", "API_TCP_PORT");
		API_LOG_PATH = getValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\ECi\\M1", "API_LOG_PATH");
		MOBILE_SERVICE_LOG_PATH = getValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\ECi\\M1", "MOBILE_SERVICE_LOG_PATH");
		INTEGRATION_SERVICE_PROGRAM_LOCATION = getValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\ECi Software Solutions\\IntegrationService", "ProgramLocation");
		ENVIRONMENT_NAME = getValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\ECi\\M1", "EnvironmentName", "Production");
		ENVIRONMENT_REGION = getValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\ECi\\M1", "Region", "US");
		DISCOVERY_URL = getValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\ECi\\M1", "DiscoveryUrl", "https://m1.ecimanufacturing.com/");
	}

	private static string getValue(string keyName, string valueName)
	{
		object value = Registry.GetValue(keyName, valueName, string.Empty);
		if (value != null)
		{
			return value.ToString();
		}
		return string.Empty;
	}

	private static string getValue(string keyName, string valueName, string defaultValue)
	{
		object value = Registry.GetValue(keyName, valueName, defaultValue);
		if (value != null)
		{
			return value.ToString();
		}
		return defaultValue;
	}
}
