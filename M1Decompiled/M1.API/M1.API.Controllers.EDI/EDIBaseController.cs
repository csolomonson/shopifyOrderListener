using System.Net.Http;
using System.Threading.Tasks;
using M1.API.Models.Core;
using M1.API.Utilities;

namespace M1.API.Controllers.EDI;

/// <summary>
/// EDI Services.
/// </summary>
public abstract class EDIBaseController : HMACAuthBaseController
{
	/// <summary>
	/// Creates new API client context 
	/// </summary>
	/// <param name="request">The request as HTTP Request Message</param>
	/// <param name="apiClientModel">The apiClientModel as IAPIClientModel</param>
	/// <returns>The APIClientContext object</returns>
	public override Task<APIClientContext> GetApiClientContextAsync(HttpRequestMessage request)
	{
		APIClientContext result = null;
		using (IAPIClientModel iAPIClientModel = new APIEDIClientModel())
		{
			CurrentSession = IntializeSessionFromPrincipleAsync(request, iAPIClientModel).Result;
			if (CurrentSession.Authenticated)
			{
				result = iAPIClientModel.CreateApiDataClientAsync(CurrentSession, iAPIClientModel.ApiModuleId).Result;
			}
		}
		return Task.FromResult(result);
	}
}
