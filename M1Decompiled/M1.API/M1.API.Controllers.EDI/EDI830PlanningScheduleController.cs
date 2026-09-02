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

[RoutePrefix("api/EDI/830")]
public class EDI830PlanningScheduleController : EDIBaseController
{
	private IEDIPlanningScheduleModel ediPlanningScheduleModel;

	/// <summary>
	/// Creates planning schedules based on input parameter.
	/// </summary>
	/// <param name="schedules">The list of schedules as EDI830SchedulesIN.</param>
	/// <returns>EDIOrderResponseMessageDto object</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(EDIOrderResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(EDIOrderResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "EDI" })]
	[AcceptVerbs("POST")]
	[Route("PostSchedule")]
	public async Task<IHttpActionResult> PostOrderAsync([FromBody] EDI830SchedulesIN schedules)
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
							using (ediPlanningScheduleModel = new EDIPlanningScheduleModel(ApiClientContext))
							{
								PostOrderResponseDto result2 = ediPlanningScheduleModel.ValidateRequest_PostSchedule(schedules.EDI830ScheduleSet).Result;
								if (!result2.IsValidationOk)
								{
									EDIOrderResponseMessageDto data3 = ResponseMessageBuilderFunctions.BuildResponseOblect(result2.GeneralValidatationInfo.ErrorsList.ToList(), result2.GeneralValidatationInfo.WarningsList.ToList(), result2, string.Empty, string.Empty);
									result = new CustomHttpActionResult<EDIOrderResponseMessageDto>(HttpStatusCode.BadRequest, data3, base.MediaType);
								}
								else
								{
									PostOrderResponseDto postOrderResponseDto = await ediPlanningScheduleModel.Process_PostSchedule(result2);
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
