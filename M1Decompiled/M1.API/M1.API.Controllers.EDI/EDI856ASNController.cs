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

[RoutePrefix("api/EDI/856")]
public class EDI856ASNController : EDIBaseController
{
	private IEDIShipmentModel ediShipmentModel;

	/// <summary>
	/// Returns all pending EDI shipments.
	/// </summary>
	/// <param name="page">The page number if wants to get a specific page as integer.</param>
	/// <param name="pagesize">The total numbers of Advance Ship Notice (ASN) should be on a page as integer.</param>
	/// <returns>EDI856ASNCollectionDto object</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(EDI856ASNCollectionDto))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(EDIOrderResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "EDI" })]
	[AcceptVerbs("GET")]
	[Route("AllUnmapped/{pageNo?}/{pageSize?}")]
	public async Task<IHttpActionResult> GetAllUnmappedAsync([FromUri(Name = "pageNo")] int page = 0, [FromUri(Name = "pageSize")] int pagesize = 20)
	{
		new EDI856ASNCollectionDto();
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
						result = new CustomHttpActionResult<APIResponseMessageDto>(HttpStatusCode.InternalServerError, data, base.MediaType);
					}
					else
					{
						using (ediShipmentModel = new EDIShipmentModel(ApiClientContext))
						{
							EDI856ASNCollectionDto eDI856ASNCollectionDto = await ediShipmentModel.Process_AllUnmapped(page, pagesize);
							if (eDI856ASNCollectionDto == null)
							{
								goto IL_0206;
							}
							List<EDI856OutboundASN> eDI856ShipmentSet = eDI856ASNCollectionDto.EDI856ShipmentSet;
							if (eDI856ShipmentSet != null && eDI856ShipmentSet.Count() == 0)
							{
								goto IL_0206;
							}
							result = new CustomHttpActionResult<EDI856ASNCollectionDto>(HttpStatusCode.OK, eDI856ASNCollectionDto, base.MediaType);
							goto end_IL_01e4;
							IL_0206:
							APIResponseMessageDto aPIResponseMessageDto = ResponseMessageBuilderFunctions.BuildResponseObject("Unmapped shipments are not found or invalid page requested.", string.Empty, string.Empty, string.Empty, ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Error);
							result = new CustomHttpActionResult<APIResponseMessageDto>(aPIResponseMessageDto.HttpErrorStatusCode, aPIResponseMessageDto, base.MediaType);
							end_IL_01e4:;
						}
					}
				}
			}
			catch (Exception ex)
			{
				APILogger.LogError(ApiClientContext?.APIID, ex.Message);
				APIResponseMessageDto data2 = ResponseMessageBuilderFunctions.BuildResponseObject(ex.Message, string.Empty, string.Empty, string.Empty, ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Error);
				CustomHttpActionResult<APIResponseMessageDto> customHttpActionResult = new CustomHttpActionResult<APIResponseMessageDto>(HttpStatusCode.InternalServerError, data2, base.MediaType);
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
	/// Updates EDI Flags of the given shipments.
	/// </summary>
	/// <param name="ediShipments">The list of shipments as EDI856ASNsIN type.</param>
	/// <returns>APIResponseMessageDto object</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "EDI" })]
	[AcceptVerbs("POST")]
	[Route("SetEDIFlag")]
	public async Task<IHttpActionResult> SetEDIFlagAsync([FromBody] EDI856ASNsIN ediShipments)
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
							using (ediShipmentModel = new EDIShipmentModel(ApiClientContext))
							{
								APIValidationInfoDto validationInfo = ediShipmentModel.ValidateRequest_SetEDIFlag(ediShipments).Result;
								if (!validationInfo.IsValidationOk)
								{
									APIResponseMessageDto data3 = ResponseMessageBuilderFunctions.BuildResponseObject(validationInfo.ErrorsList.ToList(), validationInfo.WarningsList.ToList(), string.Empty, string.Empty, validationInfo.APIValidationStatusCode);
									result = new CustomHttpActionResult<APIResponseMessageDto>(validationInfo.HttpValidationStatusCode, data3, base.MediaType);
								}
								else
								{
									APIValidationInfoDto aPIValidationInfoDto = await ediShipmentModel.Process_SetEDIFlag(ediShipments);
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
