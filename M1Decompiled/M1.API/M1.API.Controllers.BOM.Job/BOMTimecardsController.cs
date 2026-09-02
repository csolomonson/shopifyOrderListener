using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using M1.API.DTOs.BOM.Job;
using M1.API.DTOs.Core;
using M1.API.Models.BOM.Job;
using Swashbuckle.Swagger.Annotations;

namespace M1.API.Controllers.BOM.Job;

[RoutePrefix("api/BOM/Job")]
public class BOMTimecardsController : BOMBaseController
{
	/// <summary>
	/// Returns all existing Timecards with pagination.
	/// </summary>
	/// <param name="pageSize">The required size of the pagination. By default 1000.</param>
	/// <param name="pageNumber">The required number of page from pagination. By default 0, which is the first page.</param>
	/// <returns>BOMTimecard object</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(IList<BOMTimecardDto>))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/JOB" })]
	[AcceptVerbs("GET")]
	[Route("GetAllTimecards/{pageSize}/{pageNumber}")]
	public async Task<IHttpActionResult> GetAllTimecardAsync([FromUri(Name = "pageSize")] int pageSize = 1000, [FromUri(Name = "pageNumber")] int pageNumber = 0)
	{
		BOMTimecardModel bomTimecardModel = new BOMTimecardModel();
		try
		{
			return await RunApiMethod(base.Request, bomTimecardModel, () => bomTimecardModel.APIValidationIsTrueFunction(), () => bomTimecardModel.Process_GetAllTimecards(pageSize, pageNumber), showReturnObject: true, showResponseMessage: false);
		}
		finally
		{
			if (bomTimecardModel != null)
			{
				((IDisposable)bomTimecardModel).Dispose();
			}
		}
	}

	/// <summary>
	/// Returns Timecard for a given Timecard id.
	/// </summary>
	/// <remarks>
	/// This endpoint retrieves a Timecard based on its identifier.
	/// </remarks>
	/// <param name="timecardId">The Timecard id as a string</param>
	/// <returns>The Timecard information(BOMTimecardDto object).</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(BOMTimecardDto))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/JOB" })]
	[AcceptVerbs("GET")]
	[Route("GetTimecard/{timecardId}")]
	public async Task<IHttpActionResult> GetTimecardAsync([FromUri(Name = "timecardId")] string timecardId)
	{
		BOMTimecardModel bomTimecardModel = new BOMTimecardModel();
		try
		{
			return await RunApiMethod(base.Request, bomTimecardModel, () => bomTimecardModel.ValidateRequest_GetTimecard(timecardId).Result, () => bomTimecardModel.Process_GetTimecard(timecardId), showReturnObject: true, showResponseMessage: false);
		}
		finally
		{
			if (bomTimecardModel != null)
			{
				((IDisposable)bomTimecardModel).Dispose();
			}
		}
	}

	/// <summary>
	/// Returns Timecard for a given Timecard id and Employee id.
	/// </summary>
	/// <remarks>
	/// This endpoint retrieves a Timecard based on timecard and employee identifier.
	/// </remarks>
	/// <param name="timecardId">The Timecard id as a string</param>
	/// <param name="employeeId">The Employee id as a string</param>
	/// <returns>The Timecard information(BOMTimecardDto object).</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(BOMTimecardDto))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/JOB" })]
	[AcceptVerbs("GET")]
	[Route("GetTimecard/{timecardId}/{employeeId}")]
	public async Task<IHttpActionResult> GetTimecardByEmployeeIdAsync([FromUri(Name = "timecardId")] string timecardId, [FromUri(Name = "employeeId")] string employeeId)
	{
		BOMTimecardModel bomTimecardModel = new BOMTimecardModel();
		try
		{
			return await RunApiMethod(base.Request, bomTimecardModel, () => bomTimecardModel.ValidateRequest_GetTimecard(timecardId, employeeId).Result, () => bomTimecardModel.Process_GetTimecard(timecardId, employeeId), showReturnObject: true, showResponseMessage: false);
		}
		finally
		{
			if (bomTimecardModel != null)
			{
				((IDisposable)bomTimecardModel).Dispose();
			}
		}
	}
}
