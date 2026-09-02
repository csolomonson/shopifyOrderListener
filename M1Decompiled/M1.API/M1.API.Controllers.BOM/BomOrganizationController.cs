using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using M1.API.DTOs.BOM;
using M1.API.DTOs.Core;
using M1.API.Models.BOM;
using Swashbuckle.Swagger.Annotations;

namespace M1.API.Controllers.BOM;

[RoutePrefix("api/BOM/Organization")]
public class BomOrganizationController : BOMBaseController
{
	public IBomOrganizationModel BomOrganizationModel;

	/// <summary>
	/// Returns all existing organizations with pagination. 
	/// </summary>
	/// <param name="pageSize">The required size of the pagination. By default 1000.</param>
	/// <param name="pageNumber">The required number of page from pagination. By default 0, which is the first page</param>
	/// <returns>BOMOrganizationDto object</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(IList<BomOrganizationDto>))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/ORGANIZATION" })]
	[AcceptVerbs("GET")]
	[Route("GetAllOrganizations/{pageSize}/{pageNumber}")]
	public async Task<IHttpActionResult> GetAllOrganizationsAsync([FromUri(Name = "pageSize")] int pageSize = 1000, [FromUri(Name = "pageNumber")] int pageNumber = 0)
	{
		using (BomOrganizationModel = new BomOrganizationModel())
		{
			return await RunApiMethod(base.Request, BomOrganizationModel, () => BomOrganizationModel.APIValidationIsTrueFunction(), () => BomOrganizationModel.Process_GetAllOrganizations(pageSize, pageNumber), showReturnObject: true, showResponseMessage: false);
		}
	}

	/// <summary>
	/// Returns organization for a given organization id.
	/// </summary>
	/// <param name="organizationId">The organization id as a string</param>
	/// <returns>BOMOrganizationDto object</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(BomOrganizationDto))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/ORGANIZATION" })]
	[AcceptVerbs("GET")]
	[Route("GetOrganization/{organizationId}")]
	public async Task<IHttpActionResult> GetOrganizationAsync([FromUri(Name = "organizationId")] string organizationId)
	{
		using (BomOrganizationModel = new BomOrganizationModel())
		{
			return await RunApiMethod(base.Request, BomOrganizationModel, () => BomOrganizationModel.ValidateRequest_GetOrganization(organizationId).Result, () => BomOrganizationModel.Process_GetOrganization(organizationId), showReturnObject: true, showResponseMessage: false);
		}
	}
}
