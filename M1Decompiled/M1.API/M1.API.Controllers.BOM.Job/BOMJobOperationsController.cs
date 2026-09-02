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
public class BOMJobOperationsController : BOMBaseController
{
	/// <summary>
	/// Returns job operation details for a given job,assembly and operation id.
	/// </summary>
	/// <param name="jobId">The job id as string</param>
	/// <param name="jobAssemblyId">The job assembly id as integer</param>
	/// <param name="jobOperationId">The job operation id as integer</param>
	/// <returns>BOMJobOperationDto object</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(BOMJobOperationDto))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/JOB" })]
	[AcceptVerbs("GET")]
	[Route("GetJobOperation/{jobId}/{jobAssemblyId}/{jobOperationId}")]
	public async Task<IHttpActionResult> GetJobOperationAsync([FromUri(Name = "jobId")] string jobId, [FromUri(Name = "jobAssemblyId")] int jobAssemblyId, [FromUri(Name = "jobOperationId")] int jobOperationId)
	{
		BOMJobOperationModel bomJobOperationModel = new BOMJobOperationModel();
		try
		{
			return await RunApiMethod(base.Request, bomJobOperationModel, () => bomJobOperationModel.ValidateRequest_GetJobOperation(jobId, jobAssemblyId, jobOperationId).Result, () => bomJobOperationModel.Process_GetJobOperation(jobId, jobAssemblyId, jobOperationId), showReturnObject: true, showResponseMessage: false);
		}
		finally
		{
			if (bomJobOperationModel != null)
			{
				((IDisposable)bomJobOperationModel).Dispose();
			}
		}
	}

	/// <summary>
	/// Returns all existing JobOperations with pagination.
	/// </summary>
	/// <param name="pageSize">The required size of the pagination. By default 1000.</param>
	/// <param name="pageNumber">The required number of page from pagination. By default 0, which is the first page.</param>
	/// <returns>BOMJobOperation object</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(IList<BOMJobOperationDto>))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/JOB" })]
	[AcceptVerbs("GET")]
	[Route("GetAllJobOperations/{pageSize}/{pageNumber}")]
	public async Task<IHttpActionResult> GetAllJobOperationAsync([FromUri(Name = "pageSize")] int pageSize = 1000, [FromUri(Name = "pageNumber")] int pageNumber = 0)
	{
		BOMJobOperationModel bomJobOperationModel = new BOMJobOperationModel();
		try
		{
			return await RunApiMethod(base.Request, bomJobOperationModel, () => bomJobOperationModel.APIValidationIsTrueFunction(), () => bomJobOperationModel.Process_GetAllJobOperations(pageSize, pageNumber), showReturnObject: true, showResponseMessage: false);
		}
		finally
		{
			if (bomJobOperationModel != null)
			{
				((IDisposable)bomJobOperationModel).Dispose();
			}
		}
	}

	/// <summary>
	/// Posts a new JobOperation.
	/// </summary>
	/// <param name="jobOperation">The JobOperation data to be posted.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:System.Web.Http.IHttpActionResult" /> indicating the result of the post operation,
	/// or an appropriate error message depending on the outcome of the request.
	/// </returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/JOB" })]
	[AcceptVerbs("POST")]
	[Route("PostJobOperation")]
	public async Task<IHttpActionResult> PostJobOperationAsync([FromBody] BOMJobOperationDto jobOperation)
	{
		BOMJobOperationModel bomJobOperationModel = new BOMJobOperationModel();
		try
		{
			return await RunApiMethod(base.Request, bomJobOperationModel, () => bomJobOperationModel.ValidateRequest_PostJobOperation(jobOperation).Result, () => bomJobOperationModel.Process_PostJobOperation(jobOperation), showReturnObject: false, showResponseMessage: true);
		}
		finally
		{
			if (bomJobOperationModel != null)
			{
				((IDisposable)bomJobOperationModel).Dispose();
			}
		}
	}

	/// <summary>
	/// Deletes a job operation based on input parameters.
	/// </summary>
	/// <param name="jobId">The job id as string</param>
	/// <param name="jobAssemblyId">The job assembly id as integer</param>
	/// <param name="jobOperationId">The job operation id as integer</param>
	/// <returns></returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(BOMJobOperationDto))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/JOB" })]
	[AcceptVerbs("DELETE")]
	[Route("DeleteJobOperation/{jobId}/{jobAssemblyId}/{jobOperationId}")]
	public async Task<IHttpActionResult> DeleteJobOperationAsync([FromUri(Name = "jobId")] string jobId, [FromUri(Name = "jobAssemblyId")] int jobAssemblyId, [FromUri(Name = "jobOperationId")] int jobOperationId)
	{
		BOMJobOperationModel bomJobOperationModel = new BOMJobOperationModel();
		try
		{
			return await RunApiMethod(base.Request, bomJobOperationModel, () => bomJobOperationModel.ValidateRequest_DeleteJobOperation(jobId, jobAssemblyId, jobOperationId).Result, () => bomJobOperationModel.Process_DeleteJobOperation(jobId, jobAssemblyId, jobOperationId), showReturnObject: false, showResponseMessage: true);
		}
		finally
		{
			if (bomJobOperationModel != null)
			{
				((IDisposable)bomJobOperationModel).Dispose();
			}
		}
	}
}
