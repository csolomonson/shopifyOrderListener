using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Hosting;
using M1.API.Utilities;

namespace M1.API.Controllers;

public class CustomHttpActionResult<T> : IHttpActionResult
{
	private readonly HttpStatusCode StatusCode;

	private readonly T data;

	private readonly string MediaType = "application/xml";

	private readonly string TraceId;

	public CustomHttpActionResult(HttpStatusCode statusCode, T data, string mediaType, string traceId = "")
	{
		StatusCode = statusCode;
		this.data = data;
		MediaType = mediaType;
		TraceId = traceId;
	}

	public HttpResponseMessage CreateResponse(HttpStatusCode statusCode, T data, string mediaType, string traceId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		HttpResponseMessage val = HttpRequestMessageExtensions.CreateResponse(new HttpRequestMessage
		{
			Properties = { 
			{
				HttpPropertyKeys.HttpConfigurationKey,
				(object)new HttpConfiguration()
			} }
		}, statusCode, data, mediaType);
		if (!string.IsNullOrEmpty(traceId))
		{
			((HttpHeaders)val.Headers).Add("traceid", traceId);
		}
		return val;
	}

	public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
	{
		try
		{
			return Task.FromResult<HttpResponseMessage>(CreateResponse(StatusCode, data, MediaType, TraceId));
		}
		catch (Exception ex)
		{
			APILogger.LogError(ex.Message);
			throw;
		}
	}
}
