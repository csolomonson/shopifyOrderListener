using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using M1.API.App_Start;
using M1.API.DTOs.Core;
using M1.API.DTOs.EDI;
using M1.API.Models.EDI;
using M1.API.Utilities;
using Swashbuckle.Swagger.Annotations;

namespace M1.API.Controllers.EDI;

/// <summary>
/// EDI 810 service.
/// </summary>
[RoutePrefix("api/EDI/810")]
public class EDI810InvoiceController : EDIBaseController
{
	private IEDIInvoiceModel ediInvoiceModel;

	/// <summary>
	/// Returns all pending EDI AR invoices.
	/// </summary>
	/// <param name="page">The page number if wants to get a specific page as integer.</param>
	/// <param name="pagesize">The total numbers of invoices should be on a page as integer.</param>
	/// <returns>EDI810InvoiceCollectionDto object</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(EDI810InvoiceCollectionDto))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[AcceptVerbs("GET")]
	[SwaggerOperation(null, Tags = new string[] { "EDI" })]
	[Route("AllUnmapped/{pageNo?}/{pageSize?}")]
	public async Task<IHttpActionResult> GetAllUnmappedAsync([FromUri(Name = "pageNo")] int page = 0, [FromUri(Name = "pageSize")] int pagesize = 20)
	{
		new EDI810InvoiceCollectionDto();
		IHttpActionResult result;
		try
		{
			_ = 1;
			try
			{
				SetMediaTypeFromRequest(base.Request);
				using (ApiClientContext = await GetApiClientContextAsync(base.Request))
				{
					if (!(ApiClientContext?.LoginAuthenticated).Value)
					{
						APILogger.LogError(ApiClientContext?.APIID, ApiClientContext.LoginErrorOutputString);
						APIResponseMessageDto data = ResponseMessageBuilderFunctions.BuildResponseObject(ApiClientContext.LoginErrorOutputString, string.Empty, string.Empty, string.Empty, ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Error);
						result = new CustomHttpActionResult<APIResponseMessageDto>(HttpStatusCode.BadRequest, data, base.MediaType);
					}
					else
					{
						using (ediInvoiceModel = new EDIInvoiceModel(ApiClientContext))
						{
							EDI810InvoiceCollectionDto eDI810InvoiceCollectionDto = await ediInvoiceModel.Process_AllUnmapped(page, pagesize);
							if (eDI810InvoiceCollectionDto == null)
							{
								goto IL_0206;
							}
							List<EDI810OutboundInvoice> eDI810InvoiceSet = eDI810InvoiceCollectionDto.EDI810InvoiceSet;
							if (eDI810InvoiceSet != null && eDI810InvoiceSet.Count() == 0)
							{
								goto IL_0206;
							}
							result = new CustomHttpActionResult<EDI810InvoiceCollectionDto>(HttpStatusCode.OK, eDI810InvoiceCollectionDto, base.MediaType);
							goto end_IL_01e4;
							IL_0206:
							APIResponseMessageDto data2 = ResponseMessageBuilderFunctions.BuildResponseObject("Unmapped invoices are not found or invalid page requested.", string.Empty, string.Empty, string.Empty, ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Error);
							result = new CustomHttpActionResult<APIResponseMessageDto>(HttpStatusCode.OK, data2, base.MediaType);
							end_IL_01e4:;
						}
					}
				}
			}
			catch (Exception ex)
			{
				APILogger.LogError(ApiClientContext?.APIID, ex.Message);
				APIResponseMessageDto data3 = ResponseMessageBuilderFunctions.BuildResponseObject(ex.Message, string.Empty, string.Empty, string.Empty, ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Error);
				CustomHttpActionResult<APIResponseMessageDto> customHttpActionResult = new CustomHttpActionResult<APIResponseMessageDto>(HttpStatusCode.InternalServerError, data3, base.MediaType);
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

	/// <summary>
	/// Updates EDI Flags of the given AR invoices.
	/// </summary>
	/// <param name="ediInvoices">The list of invoices as EDI810InvoicesIN type.</param>
	/// <returns>APIResponseMessageDto object</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "EDI" })]
	[AcceptVerbs("POST")]
	[Route("SetEDIFlag")]
	public async Task<IHttpActionResult> SetEDIFlagAsync([FromBody] EDI810InvoicesIN ediInvoices)
	{
		IHttpActionResult result;
		try
		{
			_ = 1;
			try
			{
				SetMediaTypeFromRequest(base.Request);
				if (!base.ModelState.IsValid)
				{
					APIResponseMessageDto data = ResponseMessageBuilderFunctions.BuildResponseObject(GetErrorListFromModelState(base.ModelState), null, string.Empty, string.Empty, ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Error);
					result = new CustomHttpActionResult<APIResponseMessageDto>(HttpStatusCode.BadRequest, data, base.MediaType);
				}
				else
				{
					using (ApiClientContext = await GetApiClientContextAsync(base.Request))
					{
						if (!(ApiClientContext?.LoginAuthenticated).Value)
						{
							APILogger.LogError(ApiClientContext?.APIID, ApiClientContext.LoginErrorOutputString);
							APIResponseMessageDto data2 = ResponseMessageBuilderFunctions.BuildResponseObject(ApiClientContext.LoginErrorOutputString, string.Empty, string.Empty, string.Empty, ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Error);
							result = new CustomHttpActionResult<APIResponseMessageDto>(HttpStatusCode.InternalServerError, data2, base.MediaType);
						}
						else
						{
							using (ediInvoiceModel = new EDIInvoiceModel(ApiClientContext))
							{
								APIValidationInfoDto validationInfo = ediInvoiceModel.ValidateRequest_SetEDIFlag(ediInvoices).Result;
								if (!validationInfo.IsValidationOk)
								{
									APIResponseMessageDto data3 = ResponseMessageBuilderFunctions.BuildResponseObject(validationInfo.ErrorsList.ToList(), validationInfo.WarningsList.ToList(), string.Empty, string.Empty, validationInfo.APIValidationStatusCode);
									result = new CustomHttpActionResult<APIResponseMessageDto>(validationInfo.HttpValidationStatusCode, data3, base.MediaType);
								}
								else
								{
									APIValidationInfoDto aPIValidationInfoDto = await ediInvoiceModel.Process_SetEDIFlag(ediInvoices);
									APIResponseMessageDto data4 = ResponseMessageBuilderFunctions.BuildResponseObject(aPIValidationInfoDto.ErrorsList.ToList(), aPIValidationInfoDto.WarningsList.ToList(), string.Empty, string.Empty, validationInfo.APIValidationStatusCode);
									result = ((!aPIValidationInfoDto.IsValidationOk) ? new CustomHttpActionResult<APIResponseMessageDto>(HttpStatusCode.BadRequest, data4, base.MediaType) : new CustomHttpActionResult<APIResponseMessageDto>(HttpStatusCode.OK, data4, base.MediaType));
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				APILogger.LogError(ApiClientContext?.APIID, ex.Message);
				APIResponseMessageDto data5 = ResponseMessageBuilderFunctions.BuildResponseObject(ex.Message, string.Empty, string.Empty, string.Empty, ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Error);
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
