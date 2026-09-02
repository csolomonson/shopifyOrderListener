using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Http.Results;
using M1.API.DTOs.Core;
using M1.API.Models.Core;
using M1.API.Utilities;
using Microsoft.Win32;

namespace M1.API.Filters;

/// <summary>
/// Support for static API key authentication using "Authorization: ApiKey {APIID}:{APIKey}" header
/// </summary>
public class ApiKeyAuthenticationAttribute : Attribute, IAuthenticationFilter, IFilter
{
	public class AddChallengeOnUnauthorizedResult : IHttpActionResult
	{
		public AuthenticationHeaderValue Challenge { get; private set; }

		public IHttpActionResult InnerResult { get; private set; }

		public AddChallengeOnUnauthorizedResult(AuthenticationHeaderValue challenge, IHttpActionResult innerResult)
		{
			Challenge = challenge;
			InnerResult = innerResult;
		}

		public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
		{
			HttpResponseMessage val = await InnerResult.ExecuteAsync(cancellationToken);
			if (val.StatusCode == HttpStatusCode.Unauthorized && !((IEnumerable<AuthenticationHeaderValue>)val.Headers.WwwAuthenticate).Any((AuthenticationHeaderValue h) => h.Scheme == Challenge.Scheme))
			{
				val.Headers.WwwAuthenticate.Add(Challenge);
			}
			return val;
		}
	}

	private const string ApiKeyAuthenicationScheme = "apikey";

	public bool AllowMultiple => false;

	public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
	{
		HttpRequestMessage request = context.Request;
		if (!HostedKeyValidOrNotRequired(context))
		{
			context.ErrorResult = new UnauthorizedResult((IEnumerable<AuthenticationHeaderValue>)(object)new AuthenticationHeaderValue[1]
			{
				new AuthenticationHeaderValue("mfg-key")
			}, context.Request);
		}
		else if (request.Headers.Authorization != null && "apikey".Equals(request.Headers.Authorization.Scheme, StringComparison.CurrentCultureIgnoreCase))
		{
			ClaimsPrincipal claimsPrincipal = await AuthorizeApiKeyRequest(request, request.Headers.Authorization.Parameter);
			if (claimsPrincipal != null)
			{
				context.Principal = claimsPrincipal;
				return;
			}
			context.ErrorResult = new UnauthorizedResult((IEnumerable<AuthenticationHeaderValue>)(object)new AuthenticationHeaderValue[1]
			{
				new AuthenticationHeaderValue("apikey")
			}, context.Request);
		}
	}

	public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		context.Result = new AddChallengeOnUnauthorizedResult(new AuthenticationHeaderValue("apikey"), context.Result);
		return Task.FromResult(0);
	}

	protected bool HostedKeyValidOrNotRequired(HttpAuthenticationContext context)
	{
		string m1ModuleCodeForUriModuleCode = APIClientModel.GetM1ModuleCodeForUriModuleCode(APIClientModel.GetModuleFromRequestUrl(context.Request));
		if (APIEnums.WebAPIModules.ERP.ToString().Equals(m1ModuleCodeForUriModuleCode, StringComparison.OrdinalIgnoreCase))
		{
			string value = Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\ECI\\M1", "HostedApiAuthenticationKey", string.Empty) as string;
			if (string.IsNullOrEmpty(value))
			{
				return true;
			}
			IEnumerable<string> source = default(IEnumerable<string>);
			if (((HttpHeaders)context.Request.Headers).TryGetValues("mfg-key", ref source) && source.Contains(value))
			{
				return true;
			}
			return false;
		}
		return true;
	}

	private async Task<ClaimsPrincipal> AuthorizeApiKeyRequest(HttpRequestMessage request, string authorizationHeaderValue)
	{
		try
		{
			_ = string.Empty;
			_ = string.Empty;
			_ = string.Empty;
			HttpUtility.UrlEncode(request.RequestUri.AbsoluteUri.ToLower());
			_ = request.Method.Method;
			string[] array = authorizationHeaderValue.Split(new char[1] { ':' }, 2);
			if (array.Length != 2)
			{
				return null;
			}
			string providedApiId = array[0];
			string providedApiKey = array[1];
			if (string.IsNullOrWhiteSpace(providedApiId) || string.IsNullOrWhiteSpace(providedApiKey))
			{
				return null;
			}
			if (providedApiId.Length != 32 || providedApiKey.Length != 32)
			{
				return null;
			}
			string m1ModuleCodeForUriModuleCode = APIClientModel.GetM1ModuleCodeForUriModuleCode(APIClientModel.GetModuleFromRequestUrl(request));
			APIMetadataDto aPIMetadataDto = await ApiKeyService.Current.LoadApiKeyAsync(request, m1ModuleCodeForUriModuleCode, providedApiId);
			if (aPIMetadataDto == null || !aPIMetadataDto.APIID.Equals(providedApiId, StringComparison.CurrentCultureIgnoreCase))
			{
				return null;
			}
			if (!ConstantTimeEquals(aPIMetadataDto.APIKey, providedApiKey))
			{
				return null;
			}
			SetContextItems(aPIMetadataDto, request);
			return new GenericPrincipal(new GenericIdentity(providedApiId), null);
		}
		catch (Exception ex)
		{
			APILogger.LogError("ApiKeyAuthenticationError: " + ex.Message);
			return null;
		}
	}

	private void SetContextItems(APIMetadataDto metadata, HttpRequestMessage request)
	{
		request.GetOwinContext().Set("ApiKeyMetadata", metadata);
	}

	[MethodImpl(MethodImplOptions.NoOptimization)]
	private static bool ConstantTimeEquals(string a, string b)
	{
		if (a.Length != b.Length)
		{
			return false;
		}
		for (int i = 0; i < a.Length; i++)
		{
			if ((a[i] ^ b[i]) != 0)
			{
				return false;
			}
		}
		return true;
	}
}
