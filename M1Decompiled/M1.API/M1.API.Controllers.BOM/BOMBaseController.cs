using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using M1.API.App_Start;
using M1.API.DTOs.BOM;
using M1.API.DTOs.Core;
using M1.API.Models;
using M1.API.Models.BOM;
using M1.API.Models.BOM.Inventory;
using M1.API.Models.Core;
using M1.API.Utilities;

namespace M1.API.Controllers.BOM;

public class BOMBaseController : HMACAuthBaseController
{
	public IBOMPartModel bomPartModel;

	public IAPICoreModel apiCoreModel;

	public IBOMPartBinDetailModel bomPartBinDetail;

	/// <summary>
	/// Creates new API client context 
	/// </summary>
	/// <param name="request">The request as HTTP Request Message</param>
	/// <param name="apiClientModel">The apiClientModel as IAPIClientModel</param>
	/// <returns>The APIClientContext object</returns>
	public override Task<APIClientContext> GetApiClientContextAsync(HttpRequestMessage request)
	{
		APIClientContext result = null;
		using (IAPIClientModel iAPIClientModel = new APIBOMClientModel())
		{
			CurrentSession = IntializeSessionFromPrincipleAsync(request, iAPIClientModel).Result;
			if (CurrentSession.Authenticated)
			{
				result = iAPIClientModel.CreateApiDataClientAsync(CurrentSession, iAPIClientModel.ApiModuleId).Result;
			}
		}
		return Task.FromResult(result);
	}

	public async Task<IHttpActionResult> RunApiMethod<T>(HttpRequestMessage request, IAPIBaseModel apiModel, Func<APIValidationInfoDto> validatinMethod, Func<Task<BOMResponseMessageDto<T>>> processingMethod, bool showReturnObject, bool showResponseMessage)
	{
		IHttpActionResult result;
		try
		{
			_ = 1;
			try
			{
				SetMediaTypeFromRequest(request);
				if (!base.ModelState.IsValid)
				{
					APIResponseMessageDto data = ResponseMessageBuilderFunctions.BuildResponseObject(GetErrorListFromModelState(base.ModelState), null, string.Empty, string.Empty, ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Error);
					result = new CustomHttpActionResult<APIResponseMessageDto>(HttpStatusCode.BadRequest, data, base.MediaType);
				}
				else
				{
					using (ApiClientContext = await GetApiClientContextAsync(request))
					{
						if (!(ApiClientContext?.LoginAuthenticated).Value)
						{
							APILogger.LogError(ApiClientContext?.APIID, ApiClientContext.LoginErrorOutputString);
							APIResponseMessageDto data2 = ResponseMessageBuilderFunctions.BuildResponseObject(ApiClientContext.LoginErrorOutputString, string.Empty, string.Empty, string.Empty, ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Error);
							result = new CustomHttpActionResult<APIResponseMessageDto>(HttpStatusCode.InternalServerError, data2, base.MediaType);
						}
						else
						{
							apiModel.ApiClientContext = ApiClientContext;
							APIValidationInfoDto validationInfo = validatinMethod();
							if (!validationInfo.IsValidationOk)
							{
								APILogger.LogError(ApiClientContext?.APIID, validationInfo.ErrorsList, validationInfo.WarningsList);
								APIResponseMessageDto data3 = ResponseMessageBuilderFunctions.BuildResponseObject(validationInfo.ErrorsList, validationInfo.WarningsList, string.Empty, string.Empty, ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Error);
								result = new CustomHttpActionResult<APIResponseMessageDto>(validationInfo.HttpValidationStatusCode, data3, base.MediaType);
							}
							else
							{
								BOMResponseMessageDto<T> bOMResponseMessageDto = await processingMethod();
								if (validationInfo.WarningsList.Count > 0)
								{
									bOMResponseMessageDto.ValidationInfo.WarningsList.AddRange(new List<string>(validationInfo.WarningsList));
								}
								if (!bOMResponseMessageDto.ValidationInfo.IsValidationOk)
								{
									APILogger.LogError(ApiClientContext?.APIID, validationInfo.ErrorsList, validationInfo.WarningsList);
									APIResponseMessageDto data4 = ResponseMessageBuilderFunctions.BuildResponseObject(bOMResponseMessageDto.ValidationInfo.ErrorsList.ToList(), bOMResponseMessageDto.ValidationInfo.WarningsList.ToList(), string.Empty, string.Empty, ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Error);
									result = new CustomHttpActionResult<APIResponseMessageDto>(bOMResponseMessageDto.ValidationInfo.HttpValidationStatusCode, data4, base.MediaType);
								}
								else
								{
									APIResponseMessageDto aPIResponseMessage = ResponseMessageBuilderFunctions.BuildResponseObject(null, bOMResponseMessageDto.ValidationInfo.WarningsList.ToList(), string.Empty, string.Empty, bOMResponseMessageDto.ValidationInfo.APIValidationStatusCode);
									bOMResponseMessageDto.APIResponseMessage = aPIResponseMessage;
									if (!showResponseMessage)
									{
										bOMResponseMessageDto.APIResponseMessage = null;
									}
									if (!showReturnObject)
									{
										bOMResponseMessageDto.ReturnObject = default(T);
									}
									result = new CustomHttpActionResult<BOMResponseMessageDto<T>>(bOMResponseMessageDto.ValidationInfo.HttpValidationStatusCode, bOMResponseMessageDto, base.MediaType);
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				APILogger.LogError(ApiClientContext?.APIID, ex.Message + " - " + ex.InnerException?.Message);
				APIResponseMessageDto data5 = ResponseMessageBuilderFunctions.BuildResponseObject(ex.Message + " - " + ex.InnerException?.Message, string.Empty, string.Empty, string.Empty, ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Error);
				CustomHttpActionResult<APIResponseMessageDto> customHttpActionResult = new CustomHttpActionResult<APIResponseMessageDto>(HttpStatusCode.InternalServerError, data5, base.MediaType);
				result = customHttpActionResult;
			}
		}
		finally
		{
			if (CurrentSession != null)
			{
				APIMetadataDto value = null;
				APIStartup.APIKeyStore.TryRemove(CurrentSession.KeyStoreKey, out value);
			}
			if (ApiClientContext != null)
			{
				await DisposeApiDataClientAsync(ApiClientContext);
				ApiClientContext = null;
			}
		}
		return result;
	}
}
