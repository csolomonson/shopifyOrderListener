using System;
using System.Net.Http;
using System.Security.Principal;
using Microsoft.Owin;
using WebApiContrib.MessageHandlers;

namespace M1.API.Utilities;

public class UserAwareThrottlingHandler : ThrottlingHandler
{
	public UserAwareThrottlingHandler(WebAPIThrottleStore store, Func<string, long> maxRequestsForUserIdentifier, TimeSpan period, string message)
		: base(store, maxRequestsForUserIdentifier, period, message)
	{
	}

	private string GetClientIpAddress(HttpRequestMessage request)
	{
		if (request.Properties.ContainsKey("MS_OwinContext"))
		{
			return ((OwinContext)request.Properties["MS_OwinContext"]).Request.RemoteIpAddress;
		}
		throw new Exception("Client IP Address Not Found in HttpRequest");
	}

	protected override string GetUserIdentifier(HttpRequestMessage request)
	{
		IPrincipal principal = request.GetRequestContext().Principal;
		if (principal != null)
		{
			return principal.Identity.Name;
		}
		return GetClientIpAddress(request);
	}
}
