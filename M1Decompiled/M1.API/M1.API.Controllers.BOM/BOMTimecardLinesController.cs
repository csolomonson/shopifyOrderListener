using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using M1.API.DTOs.BOM;
using M1.API.DTOs.Core;
using M1.API.Models.BOM;
using Swashbuckle.Swagger.Annotations;

namespace M1.API.Controllers.BOM;

[RoutePrefix("api/BOM/Job")]
public class BOMTimecardLinesController : BOMBaseController
{
	/// <summary>
	/// Returns all existing TimecardLines with pagination.
	/// </summary>
	/// <param name="pageSize">The required size of the pagination. By default 1000.</param>
	/// <param name="pageNumber">The required number of page from pagination. By default 0, which is the first page.</param>
	/// <returns>BOMTimecardLine object</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(IList<BOMTimecardLineDto>))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/JOB" })]
	[AcceptVerbs("GET")]
	[Route("GetAllTimecardLines/{pageSize}/{pageNumber}")]
	public async Task<IHttpActionResult> GetAllTimecardLineAsync([FromUri(Name = "pageSize")] int pageSize = 1000, [FromUri(Name = "pageNumber")] int pageNumber = 0)
	{
		BOMTimecardLineModel bomTimecardLineModel = new BOMTimecardLineModel();
		try
		{
			return await RunApiMethod(base.Request, bomTimecardLineModel, () => bomTimecardLineModel.APIValidationIsTrueFunction(), () => bomTimecardLineModel.Process_GetAllTimecardLines(pageSize, pageNumber), showReturnObject: true, showResponseMessage: false);
		}
		finally
		{
			if (bomTimecardLineModel != null)
			{
				((IDisposable)bomTimecardLineModel).Dispose();
			}
		}
	}

	/// <summary>
	/// Returns TimecardLine for a given Timecard id containing TimecardLine id.
	/// </summary>
	/// <remarks>
	/// This endpoint retrieves a TimecardLine based on its identifier.
	/// </remarks>
	/// <param name="timecardId">The Timecard id as a string</param>
	/// <param name="timecardLineId">The TimecardLine id as a string</param>
	/// <returns>The TimecardLine information(BOMTimecardLineDto object).</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(BOMTimecardLineDto))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/JOB" })]
	[AcceptVerbs("GET")]
	[Route("GetTimecardLine/{timecardId}/{timecardLineId}")]
	public async Task<IHttpActionResult> GetTimecardLineAsync([FromUri(Name = "timecardId")] string timecardId, [FromUri(Name = "timecardLineId")] string timecardLineId)
	{
		BOMTimecardLineModel bomTimecardLineModel = new BOMTimecardLineModel();
		try
		{
			return await RunApiMethod(base.Request, bomTimecardLineModel, () => bomTimecardLineModel.ValidateRequest_GetTimecardLine(timecardId, timecardLineId).Result, () => bomTimecardLineModel.Process_GetTimecardLine(timecardId, timecardLineId), showReturnObject: true, showResponseMessage: false);
		}
		finally
		{
			if (bomTimecardLineModel != null)
			{
				((IDisposable)bomTimecardLineModel).Dispose();
			}
		}
	}
}
