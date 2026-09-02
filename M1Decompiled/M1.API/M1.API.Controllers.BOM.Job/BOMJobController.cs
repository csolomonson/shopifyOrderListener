using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using M1.API.DTOs.BOM;
using M1.API.DTOs.BOM.Job;
using M1.API.DTOs.Core;
using M1.API.DTOs.Custom;
using M1.API.Models.BOM.Job;
using Swashbuckle.Swagger.Annotations;

namespace M1.API.Controllers.BOM.Job;

[RoutePrefix("api/BOM/Job")]
public class BOMJobController : BOMBaseController
{
	public IBOMJobModel bomJobModel;

	/// <summary>
	/// Returns GUIDs for a given jobid and/or partid. Do not use special charactors or url reserved charactors for parameter values.
	/// </summary>
	/// <param name="jobId">The jobId or section of jobid as string</param>
	/// <param name="partId">The partId or section of partid as string (optional)</param>
	/// <returns>BOMJobGuidsDto object</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(BOMJobGuidsDto))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/JOB" })]
	[AcceptVerbs("GET")]
	[Route("GetJobGUIDs/{jobId}/{partId?}")]
	public async Task<IHttpActionResult> GetJobGUIDsAsync([FromUri(Name = "jobId")] string jobId, [FromUri(Name = "partId")] string partId = "")
	{
		using (bomJobModel = new BOMJobModel())
		{
			return await RunApiMethod(base.Request, bomJobModel, () => bomJobModel.ValidateRequest_GetJobGUIDs(jobId, partId).Result, () => bomJobModel.Process_GetJobGUIDs(jobId, partId), showReturnObject: true, showResponseMessage: false);
		}
	}

	/// <summary>
	/// Returns job details for a given M1 job id or GUID. Do not pass job id if it has special characters (other than Aa-Zz0-9.-) pass GUID instead
	/// </summary>
	/// <param name="jobId">The M1 job Id or GUID of the job as a string</param>
	/// <returns>CTMBOMJobInfoDto object</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(CTMBOMJobMethodDto))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/JOB" })]
	[AcceptVerbs("GET")]
	[Route("GetJobMethod/{jobId}")]
	public async Task<IHttpActionResult> GetJobMethodAsync([FromUri] string jobId)
	{
		using (bomJobModel = new BOMJobModel())
		{
			return await RunApiMethod(base.Request, bomJobModel, () => bomJobModel.ValidateRequest_GetJobMethod(jobId).Result, () => bomJobModel.Process_GetJobMethod(bomJobModel.JobKeyDictionary["jmpJobID"].ToString()), showReturnObject: true, showResponseMessage: false);
		}
	}

	/// <summary>
	/// Returns a list of all BOM jobs with pagination.
	/// </summary>
	/// <param name="pageSize">The number of items to include in each page. Default is 1000.</param>
	/// <param name="pageNumber">The page number to retrieve. Default is 0.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:System.Web.Http.IHttpActionResult" /> containing the list of BOM jobs,
	/// or an appropriate error message depending on the outcome of the request.
	/// </returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(IList<BOMJobDto>))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/JOB" })]
	[AcceptVerbs("GET")]
	[Route("GetAllJobs/{pageSize}/{pageNumber}")]
	public async Task<IHttpActionResult> GetAllJobAsync([FromUri(Name = "pageSize")] int pageSize = 1000, [FromUri(Name = "pageNumber")] int pageNumber = 0)
	{
		BOMJobModel bomJobModel = new BOMJobModel();
		try
		{
			return await RunApiMethod(base.Request, bomJobModel, () => bomJobModel.APIValidationIsTrueFunction(), () => bomJobModel.Process_GetAllJobs(pageSize, pageNumber), showReturnObject: true, showResponseMessage: false);
		}
		finally
		{
			if (bomJobModel != null)
			{
				((IDisposable)bomJobModel).Dispose();
			}
		}
	}

	/// <summary>
	/// Returns a specific BOM job based on the provided job ID.
	/// </summary>
	/// <param name="jobId">The unique identifier of the job to retrieve.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:System.Web.Http.IHttpActionResult" /> containing the details of the specified BOM job,
	/// or an appropriate error message depending on the outcome of the request.
	/// </returns>
	/// <returns>The Job information(BOMJobDto object).</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(BOMJobDto))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/JOB" })]
	[AcceptVerbs("GET")]
	[Route("GetJob/{jobId}")]
	public async Task<IHttpActionResult> GetJobAsync([FromUri(Name = "jobId")] string jobId)
	{
		BOMJobModel bomJobModel = new BOMJobModel();
		try
		{
			return await RunApiMethod(base.Request, bomJobModel, () => bomJobModel.ValidateRequest_GetJob(jobId).Result, () => bomJobModel.Process_GetJob(jobId), showReturnObject: true, showResponseMessage: false);
		}
		finally
		{
			if (bomJobModel != null)
			{
				((IDisposable)bomJobModel).Dispose();
			}
		}
	}

	/// <summary>
	/// Posts a new BOM job.
	/// </summary>
	/// <param name="job">The BOM job data to be posted.</param>
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
	[Route("PostJob")]
	public async Task<IHttpActionResult> PostJobAsync([FromBody] CTMJobDto job)
	{
		using (bomJobModel = new BOMJobModel())
		{
			return await RunApiMethod(base.Request, bomJobModel, () => bomJobModel.ValidateRequest_PostJob(job).Result, () => bomJobModel.Process_PostJob(job), showReturnObject: false, showResponseMessage: true);
		}
	}
}
