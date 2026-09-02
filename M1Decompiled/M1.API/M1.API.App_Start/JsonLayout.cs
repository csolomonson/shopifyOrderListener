using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Newtonsoft.Json;
using log4net.Core;
using log4net.Layout;

namespace M1.API.App_Start;

public class JsonLayout : LayoutSkeleton
{
	[ExcludeFromCodeCoverage]
	public override void ActivateOptions()
	{
	}

	public override void Format(TextWriter writer, LoggingEvent e)
	{
		object requestProperties = null;
		if (e.LookupProperty("DatabaseID") != null)
		{
			requestProperties = new
			{
				database = e.LookupProperty("DatabaseID"),
				apiModule = e.LookupProperty("APIModule"),
				requestMethod = e.LookupProperty("RequestMethod"),
				requestPath = e.LookupProperty("RequestPath"),
				traceId = e.LookupProperty("TraceId"),
				parentId = e.LookupProperty("ParentId")
			};
		}
		var value = new
		{
			message = e.MessageObject,
			identity = e.Identity,
			pid = Process.GetCurrentProcess().Id,
			timestamp = e.TimeStamp.ToUniversalTime().ToString("O"),
			level = e.Level.DisplayName,
			thread = e.ThreadName,
			requestProperties = requestProperties
		};
		writer.WriteLine(JsonConvert.SerializeObject(value));
	}
}
