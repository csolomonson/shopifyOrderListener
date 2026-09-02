using System;
using System.ComponentModel.Design;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using M1.Extensions;

namespace M1.Core;

public class AppContext : ApplicationContext, IServiceProvider, IServiceContainer
{
	public M1DatabaseCollection Databases;

	public M1DataDictionaryCollection DataDictionaries;

	public Reports Reports;

	public bool SuspendTimerEvents;

	public ServerManager DBServerManager;

	public ServerManager DDServerManager;

	public InstallationInfo Server = new InstallationInfo();

	public InstallationInfo Client = new InstallationInfo();

	public IsInstalled IsInstalled = new IsInstalled();

	public DatabaseInfoCollection InstalledDatabases;

	public DataDictionaryInfoCollection InstalledDataDictionaries;

	public RegistryInfo Registry = new RegistryInfo();

	public MetadataInfo Metadata = new MetadataInfo();

	public bool IsHosted;

	private string _Version = string.Empty;

	protected ServiceContainer serviceContainer;

	private const string DEFAULT_API_LOG_PATH = "M1APILogs";

	private const string DEFAULT_MOBILE_SERVICE_LOG_PATH = "M1MobileServiceLogs";

	public static bool InQuickExit { get; set; }

	public bool AllowSecurityRiskFiles => !IsHosted;

	public bool DisableOpenFileHelp => IsHosted;

	public string Version
	{
		get
		{
			if (_Version.Length == 0)
			{
				Version version = Assembly.GetExecutingAssembly().GetName().Version;
				_Version = version.Major + "." + version.Minor.ToString().PadLeft(1, '0') + "." + version.Build.ToString().PadLeft(3, '0');
			}
			return _Version;
		}
	}

	public AppContext(bool designMode)
	{
		serviceContainer = new ServiceContainer();
		DBServerManager = new ServerManager(this);
		DDServerManager = new ServerManager(this);
		if (Registry != null && Registry.MetadataServer.Length > 0)
		{
			IsHosted = true;
			Metadata.LoadMetadata(this);
		}
		InitializePathAndIni(Client, Server, designMode);
	}

	public AppContext(bool designMode, bool loadMetadata)
	{
		serviceContainer = new ServiceContainer();
		serviceContainer.AddService(typeof(AppContext), this);
		DBServerManager = new ServerManager(this);
		DDServerManager = new ServerManager(this);
		if (Registry != null && Registry.MetadataServer.Length > 0)
		{
			IsHosted = true;
			if (loadMetadata)
			{
				Metadata.LoadMetadata(this);
			}
		}
		Databases = new M1DatabaseCollection(this);
		DataDictionaries = new M1DataDictionaryCollection(this);
		Reports = new Reports(this);
		InstalledDatabases = new DatabaseInfoCollection(this);
		InstalledDataDictionaries = new DataDictionaryInfoCollection(this);
		InitializePathAndIni(Client, Server, designMode);
	}

	public AppContext()
	{
		serviceContainer = new ServiceContainer();
		serviceContainer.AddService(typeof(AppContext), this);
		DBServerManager = new ServerManager(this);
		DDServerManager = new ServerManager(this);
		if (Registry != null && Registry.MetadataServer.Length > 0)
		{
			IsHosted = true;
			Metadata.LoadMetadata(this);
		}
		Databases = new M1DatabaseCollection(this);
		DataDictionaries = new M1DataDictionaryCollection(this);
		Reports = new Reports(this);
		InstalledDatabases = new DatabaseInfoCollection(this);
		InstalledDataDictionaries = new DataDictionaryInfoCollection(this);
		InitializePathAndIni(Client, Server, designMode: false);
	}

	public void LoadConnectionInformation()
	{
		DBServerManager.LoadFromSettings(Server.IniSettings, useDataDictionarySettings: false);
		DDServerManager.LoadFromSettings(Server.IniSettings, useDataDictionarySettings: true);
	}

	public void LoadConnectionInformation(string host, string port, string sqluser, string sqlpass, bool isTrusted, string netLib)
	{
		DBServerManager.LoadSuppliedSettings(useDataDictionarySettings: false, host, port, sqluser, sqlpass, isTrusted, netLib);
		DDServerManager.LoadSuppliedSettings(useDataDictionarySettings: true, host, port, sqluser, sqlpass, isTrusted, netLib);
	}

	public void LoadConnectionInformationForMobile()
	{
		DBServerManager.LoadMobileConnectionSettings(Server);
		DDServerManager.LoadMobileConnectionSettings(Server);
	}

	public void LoadConnectionInformationForProductConfigurator()
	{
		DBServerManager.LoadProductConfiguratorConnectionSettings();
		DDServerManager.LoadProductConfiguratorConnectionSettings();
	}

	public void LoadThirdPartyInformation()
	{
		AppContext context = this;
		Metadata.LoadThirdPartyMetadata(ref context);
	}

	private static string getPath(bool designMode)
	{
		if (designMode)
		{
			int num = Environment.CommandLine.IndexOf("M1.sln", StringComparison.CurrentCultureIgnoreCase);
			if (num != -1)
			{
				string text = Environment.CommandLine.Substring(0, num);
				num = text.LastIndexOf(' ');
				if (num != -1)
				{
					text = text.Substring(num + 1).TrimEnd();
				}
				if (text.StartsWith("\""))
				{
					text = text.Substring(1);
				}
				return text.AddBackslash();
			}
			return Environment.CurrentDirectory.AddBackslash();
		}
		return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).AddBackslash();
	}

	private static void InitializePathAndIni(InstallationInfo clientInfo, InstallationInfo serverInfo, bool designMode)
	{
		string text = getPath(designMode);
		if (!File.Exists(Path.Combine(text, "m1.ini")))
		{
			do
			{
				DirectoryInfo parent = Directory.GetParent(text);
				if (parent == null)
				{
					text = string.Empty;
					break;
				}
				text = parent.Parent.FullName.AddBackslash();
			}
			while (!File.Exists(Path.Combine(text, "m1.ini")));
		}
		if (text.Length == 0)
		{
			text = getPath(designMode);
			if (!File.Exists(Path.Combine(text, "m1.ini")))
			{
				File.AppendAllText(Path.Combine(text, "m1.ini"), "[System Info]");
			}
		}
		clientInfo.Location = text;
		clientInfo.IniSettings = new IniSettings();
		clientInfo.IniSettings.LoadM1IniSettings(text);
		if (Directory.Exists(Path.Combine(text, "help")))
		{
			serverInfo.Location = clientInfo.Location;
			serverInfo.IniSettings = clientInfo.IniSettings;
		}
		else
		{
			serverInfo.Location = clientInfo.IniSettings.Get("ServerLocation", text).AddBackslash();
			serverInfo.IniSettings = new IniSettings();
			serverInfo.IniSettings.LoadM1IniSettings(serverInfo.Location);
		}
	}

	public string GetWindowsRegion()
	{
		string text = string.Empty;
		switch (CultureInfo.CurrentCulture.DisplayName.Trim().ToUpper())
		{
		case "ENGLISH (AUSTRALIA)":
			text = "AUS";
			break;
		case "ENGLISH (NEW ZEALAND)":
			text = "NZ";
			break;
		case "ENGLISH (CANADA)":
			text = "CAN";
			break;
		case "ENGLISH (UNITED STATES)":
			text = "US";
			break;
		case "ENGLISH (UNITED KINGDOM)":
			text = "UK";
			break;
		}
		if (text.Length == 0)
		{
			text = "AUS";
		}
		return text;
	}

	public string GetAPILogsPath()
	{
		string text = GetRootDrive() + "\\M1APILogs";
		if (IsHosted)
		{
			return string.IsNullOrWhiteSpace(Registry.API_LOG_PATH) ? text : Registry.API_LOG_PATH;
		}
		Server.IniSettings.LoadM1IniSettings(Server.Location + "m1.ini");
		return Server.IniSettings.Get("API_LOG_PATH", text);
	}

	public string GetMobileServiceLogsPath()
	{
		string text = GetRootDrive() + "\\M1MobileServiceLogs";
		if (IsHosted)
		{
			return string.IsNullOrWhiteSpace(Registry.MOBILE_SERVICE_LOG_PATH) ? text : Registry.MOBILE_SERVICE_LOG_PATH;
		}
		Server.IniSettings.LoadM1IniSettings(Server.Location + "m1.ini");
		return Server.IniSettings.Get("MOBILE_SERVICE_LOG_PATH", text);
	}

	public string GetRootDrive()
	{
		return Path.GetPathRoot(Assembly.GetEntryAssembly().Location);
	}

	public object GetService(Type serviceType)
	{
		return serviceContainer.GetService(serviceType);
	}

	public void AddService(Type serviceType, ServiceCreatorCallback callback, bool promote)
	{
		serviceContainer.AddService(serviceType, callback, promote);
	}

	public void AddService(Type serviceType, ServiceCreatorCallback callback)
	{
		serviceContainer.AddService(serviceType, callback);
	}

	public void AddService(Type serviceType, object serviceInstance, bool promote)
	{
		serviceContainer.AddService(serviceType, serviceInstance, promote);
	}

	public void AddService(Type serviceType, object serviceInstance)
	{
		serviceContainer.AddService(serviceType, serviceInstance);
	}

	public void RemoveService(Type serviceType, bool promote)
	{
		serviceContainer.RemoveService(serviceType, promote);
	}

	public void RemoveService(Type serviceType)
	{
		serviceContainer.RemoveService(serviceType);
	}
}
