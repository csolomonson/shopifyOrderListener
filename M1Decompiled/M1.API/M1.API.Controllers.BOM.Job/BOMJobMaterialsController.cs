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
public class BOMJobMaterialsController : BOMBaseController
{
	/// <summary>
	/// Returns job material details for a given job,assembly and material id.
	/// </summary>
	/// <param name="jobId">The job id as string</param>
	/// <param name="jobAssemblyId">The job assembly id as integer</param>
	/// <param name="jobMaterialId">The job material id as integer</param>
	/// <returns>APIResponseMessageDto as object</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(BOMJobMaterialDto))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/JOB" })]
	[AcceptVerbs("GET")]
	[Route("GetJobMaterial/{jobId}/{jobAssemblyId}/{jobMaterialId}")]
	public async Task<IHttpActionResult> GetJobMaterialAsync([FromUri(Name = "jobId")] string jobId, [FromUri(Name = "jobAssemblyId")] int jobAssemblyId, [FromUri(Name = "jobMaterialId")] int jobMaterialId)
	{
		BOMJobMaterialModel bomJobMaterialModel = new BOMJobMaterialModel();
		try
		{
			return await RunApiMethod(base.Request, bomJobMaterialModel, () => bomJobMaterialModel.ValidateRequest_GetJobMaterial(jobId, jobAssemblyId, jobMaterialId).Result, () => bomJobMaterialModel.Process_GetJobMaterial(jobId, jobAssemblyId, jobMaterialId), showReturnObject: true, showResponseMessage: false);
		}
		finally
		{
			if (bomJobMaterialModel != null)
			{
				((IDisposable)bomJobMaterialModel).Dispose();
			}
		}
	}

	/// <summary>
	/// Returns all existing JobMaterials with pagination.
	/// </summary>
	/// <param name="pageSize">The required size of the pagination. By default 1000.</param>
	/// <param name="pageNumber">The required number of page from pagination. By default 0, which is the first page.</param>
	/// <returns>BOMJobMaterial object</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(IList<BOMJobMaterialDto>))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/JOB" })]
	[AcceptVerbs("GET")]
	[Route("GetAllJobMaterials/{pageSize}/{pageNumber}")]
	public async Task<IHttpActionResult> GetAllJobMaterialAsync([FromUri(Name = "pageSize")] int pageSize = 1000, [FromUri(Name = "pageNumber")] int pageNumber = 0)
	{
		BOMJobMaterialModel bomJobMaterialModel = new BOMJobMaterialModel();
		try
		{
			return await RunApiMethod(base.Request, bomJobMaterialModel, () => bomJobMaterialModel.APIValidationIsTrueFunction(), () => bomJobMaterialModel.Process_GetAllJobMaterials(pageSize, pageNumber), showReturnObject: true, showResponseMessage: false);
		}
		finally
		{
			if (bomJobMaterialModel != null)
			{
				((IDisposable)bomJobMaterialModel).Dispose();
			}
		}
	}

	/// <summary>
	/// Posts a new JobMaterial.
	/// </summary>
	/// <param name="jobMaterial">The JobMaterial data to be posted.</param>
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
	[Route("PostJobMaterial")]
	public async Task<IHttpActionResult> PostJobMaterialAsync([FromBody] BOMJobMaterialDto jobMaterial)
	{
		BOMJobMaterialModel bomJobMaterialModel = new BOMJobMaterialModel();
		try
		{
			return await RunApiMethod(base.Request, bomJobMaterialModel, () => bomJobMaterialModel.ValidateRequest_PostJobMaterial(jobMaterial).Result, () => bomJobMaterialModel.Process_PostJobMaterial(jobMaterial), showReturnObject: false, showResponseMessage: true);
		}
		finally
		{
			if (bomJobMaterialModel != null)
			{
				((IDisposable)bomJobMaterialModel).Dispose();
			}
		}
	}

	/// <summary>
	/// Deletes a job material based on input parameters.
	/// </summary>
	/// <param name="jobId">The job id as string</param>
	/// <param name="jobAssemblyId">The job assembly id as integer</param>
	/// <param name="jobMaterialId">The job material id as integer</param>
	/// <returns></returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(BOMJobMaterialDto))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/JOB" })]
	[AcceptVerbs("DELETE")]
	[Route("DeleteJobMaterial/{jobId}/{jobAssemblyId}/{jobMaterialId}")]
	public async Task<IHttpActionResult> DeleteJobMaterialAsync([FromUri(Name = "jobId")] string jobId, [FromUri(Name = "jobAssemblyId")] int jobAssemblyId, [FromUri(Name = "jobMaterialId")] int jobMaterialId)
	{
		BOMJobMaterialModel bomJobMaterialModel = new BOMJobMaterialModel();
		try
		{
			return await RunApiMethod(base.Request, bomJobMaterialModel, () => bomJobMaterialModel.ValidateRequest_DeleteJobMaterial(jobId, jobAssemblyId, jobMaterialId).Result, () => bomJobMaterialModel.Process_DeleteJobMaterial(jobId, jobAssemblyId, jobMaterialId), showReturnObject: false, showResponseMessage: true);
		}
		finally
		{
			if (bomJobMaterialModel != null)
			{
				((IDisposable)bomJobMaterialModel).Dispose();
			}
		}
	}
}
