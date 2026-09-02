using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using M1.API.DTOs.BOM.Job;
using M1.API.DTOs.Core;
using M1.API.Models.BOM;
using Swashbuckle.Swagger.Annotations;

namespace M1.API.Controllers.BOM.Job;

[RoutePrefix("api/BOM/Job")]
public class BOMJobAssembliesController : BOMBaseController
{
	/// <summary>
	/// Returns job assembly details for a given job and assembly id.
	/// </summary>
	/// <param name="jobId">The job id as string</param>
	/// <param name="jobAssemblyId">The job assembly id as integer</param>
	/// <returns>BOMJobAssemblyDto object</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(BOMJobAssemblyDto))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/JOB" })]
	[AcceptVerbs("GET")]
	[Route("GetJobAssembly/{jobId}/{jobAssemblyId}")]
	public async Task<IHttpActionResult> GetJobAssemblyAsync([FromUri(Name = "jobId")] string jobId, [FromUri(Name = "jobAssemblyId")] int jobAssemblyId)
	{
		BOMJobAssemblyModel bomJobAssemblyModel = new BOMJobAssemblyModel();
		try
		{
			return await RunApiMethod(base.Request, bomJobAssemblyModel, () => bomJobAssemblyModel.ValidateRequest_GetJobAssembly(jobId, jobAssemblyId).Result, () => bomJobAssemblyModel.Process_GetJobAssembly(jobId, jobAssemblyId), showReturnObject: true, showResponseMessage: false);
		}
		finally
		{
			if (bomJobAssemblyModel != null)
			{
				((IDisposable)bomJobAssemblyModel).Dispose();
			}
		}
	}

	/// <summary>
	/// Returns all existing JobAssemblies with pagination.
	/// </summary>
	/// <param name="pageSize">The required size of the pagination. By default 1000.</param>
	/// <param name="pageNumber">The required number of page from pagination. By default 0, which is the first page.</param>
	/// <returns>BOMJobAssembly object</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(IList<BOMJobAssemblyDto>))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/JOB" })]
	[AcceptVerbs("GET")]
	[Route("GetAllJobAssemblies/{pageSize}/{pageNumber}")]
	public async Task<IHttpActionResult> GetAllJobAssemblyAsync([FromUri(Name = "pageSize")] int pageSize = 1000, [FromUri(Name = "pageNumber")] int pageNumber = 0)
	{
		BOMJobAssemblyModel bomJobAssemblyModel = new BOMJobAssemblyModel();
		try
		{
			return await RunApiMethod(base.Request, bomJobAssemblyModel, () => bomJobAssemblyModel.APIValidationIsTrueFunction(), () => bomJobAssemblyModel.Process_GetAllJobAssemblies(pageSize, pageNumber), showReturnObject: true, showResponseMessage: false);
		}
		finally
		{
			if (bomJobAssemblyModel != null)
			{
				((IDisposable)bomJobAssemblyModel).Dispose();
			}
		}
	}

	/// <summary>
	/// Returns JobAssembly for a given JobAssembly id.
	/// </summary>
	/// <remarks>
	/// This endpoint retrieves a JobAssembly based on its identifier.
	/// </remarks>
	/// <param name="jobId">The Job id as a string</param>
	/// <returns>The JobAssembly information(BOMJobAssemblyDto object).</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(BOMJobAssemblyDto))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/JOB" })]
	[AcceptVerbs("GET")]
	[Route("GetJobAssemblies/{jobId}")]
	public async Task<IHttpActionResult> GetJobAssemblyAsync([FromUri(Name = "jobId")] string jobId)
	{
		BOMJobAssemblyModel bomJobAssemblyModel = new BOMJobAssemblyModel();
		try
		{
			return await RunApiMethod(base.Request, bomJobAssemblyModel, () => bomJobAssemblyModel.ValidateRequest_GetJobId(jobId).Result, () => bomJobAssemblyModel.Process_GetJobAssembly(jobId), showReturnObject: true, showResponseMessage: false);
		}
		finally
		{
			if (bomJobAssemblyModel != null)
			{
				((IDisposable)bomJobAssemblyModel).Dispose();
			}
		}
	}

	/// <summary>
	/// Creates a job assembly based on input parameter.
	/// </summary>
	/// <param name="jobAssembly">The jobassembly as BOMJobAssemblyDto</param>
	/// <returns>APIResponseMessageDto object</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/JOB" })]
	[AcceptVerbs("POST")]
	[Route("PostJobAssembly")]
	public async Task<IHttpActionResult> PostJobAssemblyAsync([FromBody] BOMJobAssemblyDto jobAssembly)
	{
		BOMJobAssemblyModel bomJobAssemblyModel = new BOMJobAssemblyModel();
		try
		{
			return await RunApiMethod(base.Request, bomJobAssemblyModel, () => bomJobAssemblyModel.ValidateRequest_PostJobAssembly(jobAssembly).Result, () => bomJobAssemblyModel.Process_PostJobAssembly(jobAssembly), showReturnObject: false, showResponseMessage: true);
		}
		finally
		{
			if (bomJobAssemblyModel != null)
			{
				((IDisposable)bomJobAssemblyModel).Dispose();
			}
		}
	}

	/// <summary>
	/// Deletes a job assembly based on input parameters.
	/// </summary>
	/// <param name="jobId">The job id as string</param>
	/// <param name="jobAssemblyId">The job assembly id as integer</param>
	/// <returns></returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/JOB" })]
	[AcceptVerbs("DELETE")]
	[Route("DeleteJobAssembly/{jobId}/{jobAssemblyId}")]
	public async Task<IHttpActionResult> DeleteJobAssemblyAsync([FromUri(Name = "jobId")] string jobId, [FromUri(Name = "jobAssemblyId")] int jobAssemblyId)
	{
		BOMJobAssemblyModel bomJobAssemblyModel = new BOMJobAssemblyModel();
		try
		{
			return await RunApiMethod(base.Request, bomJobAssemblyModel, () => bomJobAssemblyModel.ValidateRequest_DeleteJobAssembly(jobId, jobAssemblyId).Result, () => bomJobAssemblyModel.Process_DeleteJobAssembly(jobId, jobAssemblyId), showReturnObject: false, showResponseMessage: true);
		}
		finally
		{
			if (bomJobAssemblyModel != null)
			{
				((IDisposable)bomJobAssemblyModel).Dispose();
			}
		}
	}
}
