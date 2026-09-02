using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Caching;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Http.Results;
using M1.API.App_Start;
using M1.API.DTOs.Core;
using M1.API.Models.Core;

namespace M1.API.Filters;

public class HMACAuthenticationAttribute : Attribute, IAuthenticationFilter, IFilter
{
	private class APIAuthHeaderInfoDto
	{
		public string APIID { get; private set; }

		public string RequestContentBase64Signature { get; private set; }

		public string Nonce { get; private set; }

		public string RequestTimeStamp { get; private set; }

		public bool IsValid { get; set; }

		public APIAuthHeaderInfoDto(string aPIID, string requestContentBase64Signature, string nonce, string requestTimeStamp)
		{
			APIID = aPIID;
			RequestTimeStamp = requestTimeStamp;
			Nonce = nonce;
			RequestContentBase64Signature = requestContentBase64Signature;
		}
	}

	/// <summary>
	///
	/// </summary>
	public class ResultWithChallenge : IHttpActionResult
	{
		private readonly string authenticationScheme = "amx";

		private readonly IHttpActionResult next;

		public ResultWithChallenge(IHttpActionResult next)
		{
			this.next = next;
		}

		public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
		{
			HttpResponseMessage val = await next.ExecuteAsync(cancellationToken);
			if (val.StatusCode == HttpStatusCode.Unauthorized)
			{
				val.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue(authenticationScheme));
			}
			return val;
		}
	}

	private readonly ulong requestMaxAgeInSeconds = 300uL;

	private string AmxAuthenicationScheme = "amx";

	public bool AllowMultiple => false;

	private string[] GetAuthorizationHeaderValues(string headerParameters)
	{
		string[] array = headerParameters.Split(':');
		if (array.Length == 4)
		{
			return array;
		}
		return null;
	}

	private bool IsReplayRequest(string nonce, string requestTimeStamp)
	{
		if (MemoryCache.Default.Contains(nonce))
		{
			return true;
		}
		DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
		ulong num = Convert.ToUInt64((DateTime.UtcNow - dateTime).TotalSeconds);
		ulong num2 = Convert.ToUInt64(requestTimeStamp);
		if (num - num2 > requestMaxAgeInSeconds)
		{
			return true;
		}
		MemoryCache.Default.Add(nonce, requestTimeStamp, DateTimeOffset.UtcNow.AddSeconds(requestMaxAgeInSeconds));
		return false;
	}

	private static async Task<byte[]> ComputeHash(HttpContent httpContent)
	{
		bool isFipsEnabled = CryptoConfig.AllowOnlyFipsAlgorithms;
		byte[] hash = null;
		byte[] array = await httpContent.ReadAsByteArrayAsync();
		if (isFipsEnabled)
		{
			using (SHA256 sHA = SHA256.Create())
			{
				if (array.Length != 0)
				{
					hash = sHA.ComputeHash(array);
				}
				return hash;
			}
		}
		using MD5 mD = MD5.Create();
		if (array.Length != 0)
		{
			hash = mD.ComputeHash(array);
		}
		return hash;
	}

	private async Task<bool> IsValidAmxRequest(HttpRequestMessage request, APIAuthHeaderInfoDto apiAuthenticatinDto)
	{
		_ = string.Empty;
		string text = string.Empty;
		string requestContentBase64String = string.Empty;
		string requestUri = HttpUtility.UrlEncode(request.RequestUri.AbsoluteUri.ToLower());
		string requestHttpMethod = request.Method.Method;
		APIMetadataDto aPIMetadataDto = null;
		using (APIClientModel aPIClientModel = new APIClientModel())
		{
			text = APIClientModel.GetM1ModuleCodeForUriModuleCode(APIClientModel.GetModuleFromRequestUrl(request));
			if (!aPIClientModel.FillAPIKeyStoreAsync(text, apiAuthenticatinDto.APIID).Result)
			{
				apiAuthenticatinDto.IsValid = false;
				return false;
			}
		}
		string key = (text + ":" + apiAuthenticatinDto.APIID).ToLower();
		if (APIStartup.APIKeyStore.ContainsKey(key))
		{
			aPIMetadataDto = APIStartup.APIKeyStore[key];
			if (!aPIMetadataDto.APIID.Equals(apiAuthenticatinDto.APIID, StringComparison.CurrentCultureIgnoreCase))
			{
				apiAuthenticatinDto.IsValid = false;
				return false;
			}
		}
		string requestApiKey = aPIMetadataDto?.APIKey ?? string.Empty;
		Convert.FromBase64String(requestApiKey);
		if (IsReplayRequest(apiAuthenticatinDto.Nonce, apiAuthenticatinDto.RequestTimeStamp))
		{
			return false;
		}
		byte[] array = await ComputeHash(request.Content);
		if (array != null)
		{
			requestContentBase64String = Convert.ToBase64String(array);
		}
		UTF8Encoding uTF8Encoding = new UTF8Encoding();
		byte[] bytes = uTF8Encoding.GetBytes(requestApiKey);
		string s = apiAuthenticatinDto.APIID + requestHttpMethod + requestUri + apiAuthenticatinDto.RequestTimeStamp + apiAuthenticatinDto.Nonce + requestContentBase64String;
		byte[] bytes2 = uTF8Encoding.GetBytes(s);
		using HMACSHA256 hMACSHA = new HMACSHA256(bytes);
		string value = Convert.ToBase64String(hMACSHA.ComputeHash(bytes2));
		return apiAuthenticatinDto.RequestContentBase64Signature.Equals(value, StringComparison.Ordinal);
	}

	/// <summary>
	/// Authenticates the request by validating credentials in the request, if present
	/// </summary>
	/// <param name="context">The context as HttpAuthenticationContext </param>
	/// <param name="cancellationToken">The cancellationToken as CancellationToken</param>
	/// <returns>The GenericPrincipal object if a valid request else an error</returns>
	public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
	{
		HttpRequestMessage request = context.Request;
		if (request.Headers.Authorization == null || !AmxAuthenicationScheme.Equals(request.Headers.Authorization.Scheme, StringComparison.CurrentCultureIgnoreCase))
		{
			return;
		}
		string parameter = request.Headers.Authorization.Parameter;
		string[] authorizationHeaderValues = GetAuthorizationHeaderValues(parameter);
		if (authorizationHeaderValues != null)
		{
			APIAuthHeaderInfoDto apiAuthenticatinDto = new APIAuthHeaderInfoDto(authorizationHeaderValues[0], authorizationHeaderValues[1], authorizationHeaderValues[2], authorizationHeaderValues[3]);
			if (await IsValidAmxRequest(request, apiAuthenticatinDto))
			{
				GenericPrincipal principal = new GenericPrincipal(new GenericIdentity(apiAuthenticatinDto.APIID.ToString()), null);
				context.Principal = principal;
			}
			else
			{
				context.ErrorResult = new UnauthorizedResult((IEnumerable<AuthenticationHeaderValue>)(object)new AuthenticationHeaderValue[0], context.Request);
			}
		}
		else
		{
			context.ErrorResult = new UnauthorizedResult((IEnumerable<AuthenticationHeaderValue>)(object)new AuthenticationHeaderValue[0], context.Request);
		}
	}

	/// <summary>
	/// Adds an authentication challenge to the HTTP response, if needed.
	/// </summary>
	/// <param name="context">The context as HttpAuthenticationContext </param>
	/// <param name="cancellationToken">The cancellationToken as CancellationToken</param>
	/// <returns></returns>
	public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
	{
		context.Result = new ResultWithChallenge(context.Result);
		return Task.FromResult(0);
	}
}
