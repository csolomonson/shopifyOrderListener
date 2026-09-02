using System;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using M1.API.App_Start;
using M1.API.DTOs.Core;
using M1.API.Filters;
using M1.API.Models.Core;
using M1.API.Utilities;

namespace M1.API.Controllers;

[HMACAuthentication]
[ApiKeyAuthentication]
public class HMACAuthBaseController : APIBaseController
{
	public async Task<APISessionDto> IntializeSessionFromPrincipleAsync(HttpRequestMessage request, IAPIClientModel apiClientModel)
	{
		APISessionDto aPISessionDto = new APISessionDto();
		if (!(request.GetRequestContext().Principal is ClaimsPrincipal claimsPrincipal))
		{
			aPISessionDto.Authenticated = false;
			return aPISessionDto;
		}
		string text = claimsPrincipal?.Identity?.Name;
		string moduleFromRequestUrl = APIClientModel.GetModuleFromRequestUrl(request);
		string M1ModuleId = APIClientModel.GetM1ModuleCodeForUriModuleCode(moduleFromRequestUrl);
		APIMetadataDto value = request.GetOwinContext().Get<APIMetadataDto>("ApiKeyMetadata");
		if (value == null)
		{
			APIStartup.APIKeyStore.TryGetValue(M1ModuleId.ToLower() + ":" + text.ToLower(), out value);
		}
		if (value != null)
		{
			if (value.APIID.Equals(text, StringComparison.CurrentCultureIgnoreCase))
			{
				aPISessionDto.APIID = text;
				aPISessionDto = await apiClientModel.GetDatabaseRelatedInfoFromDDAPIInfoAsync(value).ConfigureAwait(continueOnCapturedContext: false);
				aPISessionDto.M1ModuleCode = M1ModuleId;
			}
			else
			{
				aPISessionDto.Authenticated = false;
			}
		}
		else
		{
			aPISessionDto.Authenticated = false;
		}
		return aPISessionDto;
	}

	public override Task<bool> DisposeApiDataClientAsync(APIClientContext clientContextDto)
	{
		using IAPIClientModel iAPIClientModel = new APIClientModel();
		return iAPIClientModel.DisposeApiDataClientAsync(clientContextDto);
	}
}
