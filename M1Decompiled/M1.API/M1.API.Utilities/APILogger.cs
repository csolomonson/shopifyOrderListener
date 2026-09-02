using System;
using System.Collections.Generic;
using M1.API.App_Start;

namespace M1.API.Utilities;

public static class APILogger
{
	public static void LogInfo(string message)
	{
		APIStartup.Logger.Info(message ?? "");
	}

	public static void LogInfo(string apiId, string message)
	{
		APIStartup.Logger.Info(apiId + " - " + message);
	}

	public static void LogError(string message)
	{
		APIStartup.Logger.Error(message ?? "");
	}

	public static void LogError(string apiId, string message)
	{
		APIStartup.Logger.Error(apiId + " - " + message);
	}

	public static void LogError(string apiId, IList<string> lstErrors, IList<string> lstWarnings)
	{
		string text = string.Join(",", lstErrors);
		string text2 = string.Join(",", lstWarnings);
		if (lstErrors != null && lstErrors.Count > 0)
		{
			APIStartup.Logger.Error(apiId + " - [" + text + "]");
		}
		if (lstWarnings != null && lstWarnings.Count > 0)
		{
			APIStartup.Logger.Warn(apiId + " - [" + text2 + "]");
		}
	}

	public static void LogError(string apiId, string message, Exception exception)
	{
		APIStartup.Logger.Error(apiId + " - " + message, exception);
	}

	public static void LogWarnning(string apiId, string message)
	{
		APIStartup.Logger.Warn(apiId + " - " + message);
	}
}
