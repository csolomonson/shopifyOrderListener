using System;
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

[RoutePrefix("api/EDI/850")]
public class EDI850SalesOrderController : EDIBaseController
{
	private IEDISalesOrderModel ediSalesOrderModel;

	/// <summary>
	/// Returns sales order details for a given M1 order id.
	/// </summary>
	/// <param name="m1SalesOrderId">The M1 sales Order Id as string.</param>
	/// <returns>SalesOrderDto object</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(SalesOrderDto))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "EDI" })]
	[AcceptVerbs("GET")]
	[Route("GetOrder/{salesOrderId}")]
	public async Task<IHttpActionResult> GetOrderAsync([FromUri(Name = "salesOrderId")] string m1SalesOrderId)
	{
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
						using (ediSalesOrderModel = new EDISalesOrderModel(ApiClientContext))
						{
							APIValidationInfoDto result2 = ediSalesOrderModel.ValidateRequest_GetOrder(m1SalesOrderId).Result;
							if (!result2.IsValidationOk)
							{
								APILogger.LogError(ApiClientContext?.APIID, result2.ErrorsList, result2.WarningsList);
								APIResponseMessageDto data2 = ResponseMessageBuilderFunctions.BuildResponseObject(result2.ErrorsList, result2.WarningsList, string.Empty, string.Empty, ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Error);
								result = new CustomHttpActionResult<APIResponseMessageDto>(result2.HttpValidationStatusCode, data2, base.MediaType);
							}
							else
							{
								GetOrderResponseDto getOrderResponseDto = await ediSalesOrderModel.Process_GetOrder(m1SalesOrderId);
								if (!getOrderResponseDto.ValidationInfo.IsValidationOk)
								{
									APILogger.LogError(ApiClientContext?.APIID, getOrderResponseDto.ValidationInfo.ErrorsList, getOrderResponseDto.ValidationInfo.WarningsList);
									APIResponseMessageDto data3 = ResponseMessageBuilderFunctions.BuildResponseObject(getOrderResponseDto.ValidationInfo.ErrorsList, getOrderResponseDto.ValidationInfo.WarningsList, string.Empty, string.Empty, ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Error);
									result = new CustomHttpActionResult<APIResponseMessageDto>(getOrderResponseDto.ValidationInfo.HttpValidationStatusCode, data3, base.MediaType);
								}
								else
								{
									result = new CustomHttpActionResult<SalesOrderDto>(HttpStatusCode.OK, getOrderResponseDto.SalesOrder, base.MediaType);
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				APILogger.LogError(ApiClientContext?.APIID, ex.Message);
				APIResponseMessageDto data4 = ResponseMessageBuilderFunctions.BuildResponseObject(ex.Message, string.Empty, string.Empty, string.Empty, ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Error);
				CustomHttpActionResult<APIResponseMessageDto> customHttpActionResult = new CustomHttpActionResult<APIResponseMessageDto>(HttpStatusCode.InternalServerError, data4, base.MediaType);
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
	/// Creates sales orders based on input parameter.
	/// </summary>
	/// <param name="salesOrders">The list of sales orders as EDI850SalesOrdersIN.</param>
	/// <returns>EDIOrderResponseMessageDto object</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(EDIOrderResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(EDIOrderResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "EDI" })]
	[AcceptVerbs("POST")]
	[Route("PostOrder")]
	public async Task<IHttpActionResult> PostOrderAsync([FromBody] EDI850SalesOrdersIN salesOrders)
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
							using (ediSalesOrderModel = new EDISalesOrderModel(ApiClientContext))
							{
								PostOrderResponseDto result2 = ediSalesOrderModel.ValidateRequest_PostOrder(salesOrders.EDISalesOrderSet).Result;
								if (!result2.IsValidationOk)
								{
									EDIOrderResponseMessageDto data3 = ResponseMessageBuilderFunctions.BuildResponseOblect(result2.GeneralValidatationInfo.ErrorsList.ToList(), result2.GeneralValidatationInfo.WarningsList.ToList(), result2, string.Empty, string.Empty);
									result = new CustomHttpActionResult<EDIOrderResponseMessageDto>(HttpStatusCode.BadRequest, data3, base.MediaType);
								}
								else
								{
									PostOrderResponseDto postOrderResponseDto = await ediSalesOrderModel.Process_PostOrder(result2);
									EDIOrderResponseMessageDto data4 = ResponseMessageBuilderFunctions.BuildResponseOblect(postOrderResponseDto.GeneralValidatationInfo.ErrorsList.ToList(), postOrderResponseDto.GeneralValidatationInfo.WarningsList.ToList(), postOrderResponseDto, string.Empty, string.Empty);
									result = ((!postOrderResponseDto.IsValidationOk) ? new CustomHttpActionResult<EDIOrderResponseMessageDto>(HttpStatusCode.BadRequest, data4, base.MediaType) : new CustomHttpActionResult<EDIOrderResponseMessageDto>(HttpStatusCode.OK, data4, base.MediaType));
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
