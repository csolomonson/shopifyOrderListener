using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using M1.API.App_Start;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Models;
using M1.API.Models.Core;
using M1.API.Utilities;
using log4net;

namespace M1.API.Controllers.ERP;

public class ERPBaseController : HMACAuthBaseController
{
	/// <summary>
	/// Creates new API client context 
	/// </summary>
	/// <param name="request">The request as HTTP Request Message</param>
	/// <returns>The APIClientContext object</returns>
	public override async Task<APIClientContext> GetApiClientContextAsync(HttpRequestMessage request)
	{
		APIClientContext apiClientContext = null;
		using (IAPIClientModel apiClientModel = new APIERPClientModel())
		{
			CurrentSession = await IntializeSessionFromPrincipleAsync(request, apiClientModel).ConfigureAwait(continueOnCapturedContext: false);
			if (CurrentSession.Authenticated)
			{
				apiClientContext = await apiClientModel.CreateApiDataClientAsync(CurrentSession, apiClientModel.ApiModuleId).ConfigureAwait(continueOnCapturedContext: false);
			}
		}
		return apiClientContext;
	}

	public async Task<IHttpActionResult> RunApiMethod<T>(HttpRequestMessage request, IAPIBaseModel apiModel, Func<APIValidationInfoDto> validationMethod, Func<Task<ERPResponseMessageDto<T>>> processingMethod, bool showReturnObject, bool showResponseMessage, bool showRecordCount)
	{
		_ = string.Empty;
		IEnumerable<string> source = default(IEnumerable<string>);
		string text;
		if (((HttpHeaders)base.Request.Headers).TryGetValues("traceparent", ref source))
		{
			text = source.FirstOrDefault();
			if (text.Length > 55)
			{
				text = text.Substring(0, 55);
			}
			if (!TraceParent.IsValidTraceparent(text))
			{
				text = TraceParent.GenerateTraceParent();
			}
		}
		else
		{
			text = TraceParent.GenerateTraceParent();
		}
		string traceId = string.Empty;
		string parentId = string.Empty;
		string[] traceIdFromTraceParent = TraceParent.GetTraceIdFromTraceParent(text);
		if (traceIdFromTraceParent.Length == 2)
		{
			traceId = traceIdFromTraceParent[0];
			parentId = traceIdFromTraceParent[1];
		}
		IHttpActionResult result;
		try
		{
			_ = 1;
			try
			{
				using (ApiClientContext = await GetApiClientContextAsync(request))
				{
					ThreadContext.Properties["DatabaseID"] = ApiClientContext?.DatabaseID;
					ThreadContext.Properties["APIModule"] = ApiClientContext?.Module;
					ThreadContext.Properties["RequestMethod"] = request.Method.Method;
					ThreadContext.Properties["RequestPath"] = request.RequestUri.PathAndQuery;
					ThreadContext.Properties["TraceId"] = traceId;
					ThreadContext.Properties["ParentId"] = parentId;
					APILogger.LogInfo("Started Processing Request: " + request.Method.Method + " " + request.RequestUri.PathAndQuery);
					base.MediaType = "text/json";
					if (ApiClientContext == null)
					{
						APILogger.LogError("Error Authenticating Request: " + request.Method.Method + " " + request.RequestUri.PathAndQuery, "Could not authenticate request.");
						APIResponseMessageDto data = ResponseMessageBuilderFunctions.BuildResponseObject("Could not authenticate request.", string.Empty, string.Empty, string.Empty, ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Error);
						result = new CustomHttpActionResult<APIResponseMessageDto>(HttpStatusCode.Unauthorized, data, base.MediaType, traceId);
					}
					else if (ApiClientContext.IsReadOnly && request.Method != HttpMethod.Get)
					{
						APILogger.LogError("Error Authenticating Request: " + request.Method.Method + " " + request.RequestUri.PathAndQuery, "API Key is read-only and does not support this request type.");
						APIResponseMessageDto data2 = ResponseMessageBuilderFunctions.BuildResponseObject("API Key is read-only and does not support this request type.", string.Empty, string.Empty, string.Empty, ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Error);
						result = new CustomHttpActionResult<APIResponseMessageDto>(HttpStatusCode.MethodNotAllowed, data2, base.MediaType, traceId);
					}
					else if (!base.ModelState.IsValid)
					{
						List<string> errorListFromModelState = GetErrorListFromModelState(base.ModelState);
						APILogger.LogError("Error Authenticating Request: " + request.Method.Method + " " + request.RequestUri.PathAndQuery, errorListFromModelState, new List<string>());
						APIResponseMessageDto data3 = ResponseMessageBuilderFunctions.BuildResponseObject(errorListFromModelState, null, string.Empty, string.Empty, ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Error);
						result = new CustomHttpActionResult<APIResponseMessageDto>(HttpStatusCode.BadRequest, data3, base.MediaType, traceId);
					}
					else if (!(ApiClientContext?.LoginAuthenticated).Value)
					{
						APILogger.LogError("Error Authenticating Request: " + request.Method.Method + " " + request.RequestUri.PathAndQuery, ApiClientContext.LoginErrorOutputString);
						APIResponseMessageDto data4 = ResponseMessageBuilderFunctions.BuildResponseObject(ApiClientContext.LoginErrorOutputString, string.Empty, string.Empty, string.Empty, ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Error);
						result = new CustomHttpActionResult<APIResponseMessageDto>(HttpStatusCode.InternalServerError, data4, base.MediaType, traceId);
					}
					else
					{
						apiModel.ApiClientContext = ApiClientContext;
						APIValidationInfoDto validationInfo = validationMethod();
						if (!validationInfo.IsValidationOk)
						{
							APILogger.LogError("Error Validating Request: " + request.Method.Method + " " + request.RequestUri.PathAndQuery, validationInfo.ErrorsList, validationInfo.WarningsList);
							APIResponseMessageDto data5 = ResponseMessageBuilderFunctions.BuildResponseObject(validationInfo.ErrorsList, validationInfo.WarningsList, string.Empty, string.Empty, ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Error);
							result = new CustomHttpActionResult<APIResponseMessageDto>(validationInfo.HttpValidationStatusCode, data5, base.MediaType, traceId);
						}
						else
						{
							ERPResponseMessageDto<T> eRPResponseMessageDto = await processingMethod();
							if (validationInfo.WarningsList.Count > 0)
							{
								eRPResponseMessageDto.ValidationInfo.WarningsList.AddRange(new List<string>(validationInfo.WarningsList));
							}
							if (!eRPResponseMessageDto.ValidationInfo.IsValidationOk)
							{
								APILogger.LogError("Error Processing Request: " + request.Method.Method + " " + request.RequestUri.PathAndQuery, validationInfo.ErrorsList, validationInfo.WarningsList);
								APIResponseMessageDto data6 = ResponseMessageBuilderFunctions.BuildResponseObject(eRPResponseMessageDto.ValidationInfo.ErrorsList.ToList(), eRPResponseMessageDto.ValidationInfo.WarningsList.ToList(), string.Empty, string.Empty, ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Error);
								result = new CustomHttpActionResult<APIResponseMessageDto>(eRPResponseMessageDto.ValidationInfo.HttpValidationStatusCode, data6, base.MediaType, traceId);
							}
							else
							{
								APIResponseMessageDto aPIResponseMessage = ResponseMessageBuilderFunctions.BuildResponseObject(null, eRPResponseMessageDto.ValidationInfo.WarningsList.ToList(), string.Empty, string.Empty, eRPResponseMessageDto.ValidationInfo.APIValidationStatusCode);
								eRPResponseMessageDto.APIResponseMessage = aPIResponseMessage;
								if (!showResponseMessage)
								{
									eRPResponseMessageDto.APIResponseMessage = null;
								}
								if (!showReturnObject)
								{
									eRPResponseMessageDto.ReturnObject = default(T);
								}
								if (!showRecordCount)
								{
									eRPResponseMessageDto.RecordCount = null;
								}
								result = new CustomHttpActionResult<ERPResponseMessageDto<T>>(eRPResponseMessageDto.ValidationInfo.HttpValidationStatusCode, eRPResponseMessageDto, base.MediaType, traceId);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				APILogger.LogError("Error Processing Request: " + request.Method.Method + " " + request.RequestUri.PathAndQuery, ex.Message + " - " + ex.InnerException?.Message);
				APIResponseMessageDto data7 = ResponseMessageBuilderFunctions.BuildResponseObject(ex.Message + " - " + ex.InnerException?.Message, string.Empty, string.Empty, string.Empty, ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Error);
				CustomHttpActionResult<APIResponseMessageDto> customHttpActionResult = new CustomHttpActionResult<APIResponseMessageDto>(HttpStatusCode.InternalServerError, data7, base.MediaType, traceId);
				result = customHttpActionResult;
			}
		}
		finally
		{
			APILogger.LogInfo("Finished Processing Request: " + request.Method.Method + " " + request.RequestUri.PathAndQuery);
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
