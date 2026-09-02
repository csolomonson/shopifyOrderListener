using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using M1.API.DTOs.BOM.Transaction;
using M1.API.DTOs.Core;
using M1.API.Models.BOM.Transaction;
using Swashbuckle.Swagger.Annotations;

namespace M1.API.Controllers.BOM.Transaction;

[RoutePrefix("api/BOM/Transaction/MaterialIssue")]
public class BOMMaterialIssuesController : BOMBaseController
{
	/// <summary>
	/// Returns all existing MaterialIssues with pagination.
	/// </summary>
	/// <param name="pageSize">The required size of the pagination. By default 1000.</param>
	/// <param name="pageNumber">The required number of page from pagination. By default 0, which is the first page.</param>
	/// <returns>BOMMaterialIssue object</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(IList<BOMMaterialIssueDto>))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/TRANSACTION" })]
	[AcceptVerbs("GET")]
	[Route("GetAllMaterialIssues/{pageSize}/{pageNumber}")]
	public async Task<IHttpActionResult> GetAllMaterialIssuesAsync([FromUri(Name = "pageSize")] int pageSize = 1000, [FromUri(Name = "pageNumber")] int pageNumber = 0)
	{
		BOMMaterialIssueModel bomMaterialIssueModel = new BOMMaterialIssueModel();
		try
		{
			return await RunApiMethod(base.Request, bomMaterialIssueModel, () => bomMaterialIssueModel.APIValidationIsTrueFunction(), () => bomMaterialIssueModel.Process_GetAllMaterialIssues(pageSize, pageNumber), showReturnObject: true, showResponseMessage: false);
		}
		finally
		{
			if (bomMaterialIssueModel != null)
			{
				((IDisposable)bomMaterialIssueModel).Dispose();
			}
		}
	}

	/// <summary>
	/// Returns material issue for a given M1 material issue id. Do not pass material issue id if it has special characters (other than Aa-Zz0-9.-) pass GUID instead
	/// </summary>
	/// <param name="materialIssueId">The M1 material issue Id or GUID of the material issue as a string</param>
	/// <returns>BOMMaterialIssueDto object</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(BOMMaterialIssueDto))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/TRANSACTION" })]
	[AcceptVerbs("GET")]
	[Route("GetMaterialIssue/{materialIssueId}")]
	public async Task<IHttpActionResult> GetMaterialIssueAsync([FromUri] string materialIssueId)
	{
		BOMMaterialIssueModel bomMaterialIssueModel = new BOMMaterialIssueModel();
		try
		{
			return await RunApiMethod(base.Request, bomMaterialIssueModel, () => bomMaterialIssueModel.ValidateRequest_GetMaterialIssue(materialIssueId).Result, () => bomMaterialIssueModel.Process_GetMaterialIssue(bomMaterialIssueModel.MaterialIssueKeyDictionary["iniMaterialIssueID"].ToString()), showReturnObject: true, showResponseMessage: false);
		}
		finally
		{
			if (bomMaterialIssueModel != null)
			{
				((IDisposable)bomMaterialIssueModel).Dispose();
			}
		}
	}
}
