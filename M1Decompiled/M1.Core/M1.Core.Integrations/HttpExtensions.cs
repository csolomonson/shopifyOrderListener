using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace M1.Core.Integrations;

public static class HttpExtensions
{
	public static async Task<string> GetExceptionDetailsAsync(this HttpResponseMessage response)
	{
		StringBuilder detail = new StringBuilder();
		string value = await response.Content.ReadAsStringAsync();
		HttpContent content = response.Content;
		if (content != null)
		{
			HttpContentHeaders headers = content.Headers;
			bool? obj;
			if (headers == null)
			{
				obj = null;
			}
			else
			{
				MediaTypeHeaderValue contentType = headers.ContentType;
				obj = ((contentType == null) ? ((bool?)null) : contentType.MediaType?.StartsWith("application/json"));
			}
			bool? flag = obj;
			_ = flag == true;
		}
		detail.AppendLine($"HTTP Status {(int)response.StatusCode} ({response.ReasonPhrase})");
		detail.AppendLine(value);
		if (response.Headers != null)
		{
			response.CopyResponseHeaders(detail);
		}
		return detail.ToString();
	}

	public static IReadOnlyDictionary<string, IEnumerable<string>> GetAllResponseHeaders(this HttpResponseMessage responseMessage)
	{
		Dictionary<string, IEnumerable<string>> dictionary = ((IEnumerable<KeyValuePair<string, IEnumerable<string>>>)responseMessage.Headers).ToDictionary((KeyValuePair<string, IEnumerable<string>> h_) => h_.Key, (KeyValuePair<string, IEnumerable<string>> h_) => h_.Value);
		if (responseMessage.Content != null && responseMessage.Content.Headers != null)
		{
			foreach (KeyValuePair<string, IEnumerable<string>> item in (HttpHeaders)responseMessage.Content.Headers)
			{
				dictionary[item.Key] = item.Value;
			}
		}
		return dictionary;
	}

	public static void CopyResponseHeaders(this HttpResponseMessage responseMessage, StringBuilder detail)
	{
		if (responseMessage.Headers == null)
		{
			return;
		}
		foreach (KeyValuePair<string, IEnumerable<string>> allResponseHeader in responseMessage.GetAllResponseHeaders())
		{
			detail.AppendLine(allResponseHeader.Key + ": " + allResponseHeader.Value.First());
		}
	}

	public static string GetTraceId(this HttpResponseMessage message)
	{
		if (message == null)
		{
			return null;
		}
		IEnumerable<string> source = default(IEnumerable<string>);
		if (((HttpHeaders)message.Headers).TryGetValues("traceId", ref source))
		{
			return source.First();
		}
		IEnumerable<string> source2 = default(IEnumerable<string>);
		if (((HttpHeaders)message.Headers).TryGetValues("traceparent", ref source2))
		{
			return source2.First();
		}
		return null;
	}
}
