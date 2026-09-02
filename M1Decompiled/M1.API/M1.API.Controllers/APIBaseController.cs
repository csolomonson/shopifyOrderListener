using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using M1.API.DTOs.Core;
using M1.API.Repositories;
using M1.API.Utilities;

namespace M1.API.Controllers;

public abstract class APIBaseController : ApiController
{
	/// <summary>
	/// Variable to store APIClientContext 
	/// </summary>
	public APIClientContext ApiClientContext;

	/// <summary>
	/// Variable to store APISessionDto
	/// </summary>
	public APISessionDto CurrentSession;

	public string MediaType { get; set; } = "application/xml";

	public APIBaseController()
	{
		CurrentSession = new APISessionDto();
	}

	public void SetMediaTypeFromRequest(HttpRequestMessage request)
	{
		string empty = string.Empty;
		if (request.Method == HttpMethod.Get || request.Method == HttpMethod.Delete)
		{
			empty = ((object)request.Headers.Accept)?.ToString();
		}
		else
		{
			MediaTypeHeaderValue contentType = request.Content.Headers.ContentType;
			empty = ((contentType != null) ? contentType.MediaType : null) ?? string.Empty;
		}
		MediaType = (string.IsNullOrWhiteSpace(empty) ? MediaType : empty);
	}

	/// <summary>
	/// Creates new API client context.
	/// </summary>
	/// <param name="request">The request as HTTP Request Message</param>
	/// <param name="apiClientModel">The apiClientModel as IAPIClientModel</param>
	/// <returns>The APIClientContext object</returns>
	public virtual Task<APIClientContext> GetApiClientContextAsync(HttpRequestMessage request)
	{
		return Task.FromResult(ApiClientContext);
	}

	public List<string> GetErrorListFromModelState(ModelStateDictionary modelState)
	{
		List<string> list = modelState.Keys.SelectMany((string key) => modelState[key]?.Errors.Select((ModelError x) => x.Exception?.Message ?? string.Empty)).ToList();
		list.AddRange(modelState.Keys.SelectMany((string key) => modelState[key]?.Errors.Select((ModelError x) => x?.ErrorMessage ?? string.Empty)).ToList());
		list.AddRange(modelState.Keys.SelectMany((string key) => modelState[key]?.Errors.Select((ModelError x) => x?.Exception?.InnerException?.Message ?? string.Empty)).ToList());
		return list.Where((string s) => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
	}

	public virtual Task<bool> DisposeApiDataClientAsync(APIClientContext clientContextDto)
	{
		return new APIClientRepository()?.DoLogOutAsync(clientContextDto);
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);
	}
}
