using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace M1.API.Utilities;

public static class TraceParent
{
	public static string GenerateTraceParent()
	{
		string arg = Guid.NewGuid().ToString("N");
		string arg2 = Guid.NewGuid().ToString("N").Substring(0, 16);
		return string.Format(CultureInfo.InvariantCulture, "00-{0}-{1}-01", arg, arg2);
	}

	public static string[] GetTraceIdFromTraceParent(string traceParent)
	{
		if (string.IsNullOrWhiteSpace(traceParent))
		{
			return Array.Empty<string>();
		}
		string[] array = traceParent.Split('-');
		if (array.Length != 4)
		{
			return Array.Empty<string>();
		}
		return new string[2]
		{
			array[1],
			array[2]
		};
	}

	public static bool IsValidTraceparent(string traceparent)
	{
		string pattern = "^[0-9a-fA-F]{2}-[0-9a-fA-F]{32}-[0-9a-fA-F]{16}-[0-9a-fA-F]{2}$";
		if (Regex.IsMatch(traceparent, pattern))
		{
			return true;
		}
		return false;
	}
}
