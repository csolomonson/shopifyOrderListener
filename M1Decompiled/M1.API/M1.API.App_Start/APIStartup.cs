using System;
using System.Collections.Concurrent;
using System.Reflection;
using M1.API.DTOs.Core;
using M1.Core;
using Microsoft.Owin.Hosting;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;

namespace M1.API.App_Start;

public static class APIStartup
{
	public static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

	public static ConcurrentDictionary<string, APIMetadataDto> APIKeyStore = new ConcurrentDictionary<string, APIMetadataDto>();

	public static string APILogFilePath { get; set; } = "C:\\M1Logs";

	public static bool IsHosted { get; set; }

	private static void ConfigureLog4net()
	{
		new PatternLayout("%date %level [%thread] %logger{1} - %message%newline");
		RollingFileAppender rollingFileAppender = new RollingFileAppender();
		rollingFileAppender.Layout = new JsonLayout();
		rollingFileAppender.File = APILogFilePath + "//";
		rollingFileAppender.DatePattern = "'M1ApiLog-'yyyyMMdd'.txt'";
		rollingFileAppender.RollingStyle = RollingFileAppender.RollingMode.Composite;
		rollingFileAppender.StaticLogFileName = false;
		rollingFileAppender.AppendToFile = true;
		rollingFileAppender.MaximumFileSize = "10MB";
		rollingFileAppender.MaxSizeRollBackups = 0;
		rollingFileAppender.Threshold = Level.Verbose;
		rollingFileAppender.ActivateOptions();
		BasicConfigurator.Configure(rollingFileAppender);
	}

	private static StartOptions LoadStartUpOptions()
	{
		StartOptions startOptions = new StartOptions();
		using M1.Core.AppContext appContext = new M1.Core.AppContext(designMode: false, loadMetadata: false);
		string text = "http";
		string text2 = "80";
		bool flag = false;
		IsHosted = appContext.IsHosted;
		APILogFilePath = appContext.GetAPILogsPath();
		if (appContext.IsHosted)
		{
			flag = appContext.Registry.API_SSL_CONFIGURED == "1";
			text2 = (string.IsNullOrWhiteSpace(appContext.Registry.API_TCP_PORT) ? "55555" : appContext.Registry.API_TCP_PORT);
		}
		else
		{
			appContext.Server.IniSettings.LoadM1IniSettings(appContext.Server.Location + "m1.ini");
			flag = appContext.Server.IniSettings.GetAsBool("API_SSL_CONFIGURED", defaultValue: false);
			text2 = appContext.Server.IniSettings.Get("API_TCP_PORT", "80");
		}
		if (flag)
		{
			text = "https";
		}
		startOptions.Urls.Add(text + "://+:" + text2);
		return startOptions;
	}

	/// <summary>
	/// Starting point of the API.
	/// </summary>
	/// <returns>Returns the API server object.</returns>
	public static IDisposable APIStart()
	{
		try
		{
			new StartOptions();
			IDisposable result = WebApp.Start<APIBuilder>(LoadStartUpOptions());
			ConfigureLog4net();
			Logger.Info("[M1API Service] - Starting service");
			return result;
		}
		catch (Exception ex)
		{
			Logger.Error("[M1API Service] - Failed to start due to the following error: [" + ex.Message + "]");
			throw;
		}
	}
}
