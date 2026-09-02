using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using M1.API.DTOs.BOM.Transaction;
using M1.API.DTOs.Core;
using M1.API.DTOs.Custom.Transaction;
using M1.API.Models.BOM.Transaction;
using Swashbuckle.Swagger.Annotations;

namespace M1.API.Controllers.BOM.Transaction;

[RoutePrefix("api/BOM/Transaction/MaterialIssueLine")]
public class BOMMaterialIssueLinesController : BOMBaseController
{
	/// <summary>
	/// Returns all existing MaterialIssueLines with pagination.
	/// </summary>
	/// <param name="pageSize">The required size of the pagination. By default 1000.</param>
	/// <param name="pageNumber">The required number of page from pagination. By default 0, which is the first page.</param>
	/// <returns>BOMMaterialIssueLine object</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(IList<BOMMaterialIssueLineDto>))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/TRANSACTION" })]
	[AcceptVerbs("GET")]
	[Route("GetAllMaterialIssueLines/{pageSize}/{pageNumber}")]
	public async Task<IHttpActionResult> GetAllMaterialIssueLineAsync([FromUri(Name = "pageSize")] int pageSize = 1000, [FromUri(Name = "pageNumber")] int pageNumber = 0)
	{
		BOMMaterialIssueLineModel bomMaterialIssueLineModel = new BOMMaterialIssueLineModel();
		try
		{
			return await RunApiMethod(base.Request, bomMaterialIssueLineModel, () => bomMaterialIssueLineModel.APIValidationIsTrueFunction(), () => bomMaterialIssueLineModel.Process_GetAllMaterialIssueLines(pageSize, pageNumber), showReturnObject: true, showResponseMessage: false);
		}
		finally
		{
			if (bomMaterialIssueLineModel != null)
			{
				((IDisposable)bomMaterialIssueLineModel).Dispose();
			}
		}
	}

	/// <summary>
	/// Returns material issue lines for a given M1 material issue id. Do not pass material issue id if it has special characters (other than Aa-Zz0-9.-) pass GUID instead
	/// </summary>
	/// <param name="materialIssueId">The M1 material issue Id or GUID of the material issue as a string</param>
	/// <returns>BOMMaterialIssueLine object</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(CTMBOMMaterialIssueLineDto))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/TRANSACTION" })]
	[AcceptVerbs("GET")]
	[Route("GetMaterialIssueLines/{materialIssueId}")]
	public async Task<IHttpActionResult> GetMaterialIssueLinesAsync([FromUri] string materialIssueId)
	{
		BOMMaterialIssueModel bomMaterialIssueModel = new BOMMaterialIssueModel();
		BOMMaterialIssueLineModel bomMaterialIssueLineModel = new BOMMaterialIssueLineModel(bomMaterialIssueModel);
		try
		{
			return await RunApiMethod(base.Request, bomMaterialIssueLineModel, () => bomMaterialIssueLineModel.ValidateRequest_GetMaterialIssue(materialIssueId).Result, () => bomMaterialIssueLineModel.Process_GetMaterialIssueLines(materialIssueId), showReturnObject: true, showResponseMessage: false);
		}
		finally
		{
			if (bomMaterialIssueLineModel != null)
			{
				((IDisposable)bomMaterialIssueLineModel).Dispose();
			}
		}
	}
}
