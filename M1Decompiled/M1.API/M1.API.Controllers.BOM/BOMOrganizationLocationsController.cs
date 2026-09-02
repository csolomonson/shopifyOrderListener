using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using M1.API.DTOs.BOM;
using M1.API.DTOs.Core;
using M1.API.DTOs.Custom;
using M1.API.Models.BOM;
using Swashbuckle.Swagger.Annotations;

namespace M1.API.Controllers.BOM;

[RoutePrefix("api/BOM/Organization")]
public class BOMOrganizationLocationsController : BOMBaseController
{
	/// <summary>
	/// Returns all existing OrganizationLocations with pagination.
	/// </summary>
	/// <param name="pageSize">The required size of the pagination. By default 1000.</param>
	/// <param name="pageNumber">The required number of page from pagination. By default 0, which is the first page.</param>
	/// <returns>BOMOrganizationLocation object</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(IList<CTMOrganizationLocationDto>))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/ORGANIZATION" })]
	[AcceptVerbs("GET")]
	[Route("GetAllOrganizationLocations/{pageSize}/{pageNumber}")]
	public async Task<IHttpActionResult> GetAllOrganizationLocationAsync([FromUri(Name = "pageSize")] int pageSize = 1000, [FromUri(Name = "pageNumber")] int pageNumber = 0)
	{
		BOMOrganizationLocationModel bomOrganizationLocationModel = new BOMOrganizationLocationModel();
		try
		{
			return await RunApiMethod(base.Request, bomOrganizationLocationModel, () => bomOrganizationLocationModel.APIValidationIsTrueFunction(), () => bomOrganizationLocationModel.Process_GetAllOrganizationLocations(pageSize, pageNumber), showReturnObject: true, showResponseMessage: false);
		}
		finally
		{
			if (bomOrganizationLocationModel != null)
			{
				((IDisposable)bomOrganizationLocationModel).Dispose();
			}
		}
	}

	/// <summary>
	/// Returns OrganizationLocation for a given Organization Id and OrganizationLocation id.
	/// </summary>
	/// <remarks>
	/// This endpoint retrieves a OrganizationLocation based on Organization Id and its identifier.
	/// </remarks>
	/// <param name="organizationId">The Organization id as a string</param>
	/// <param name="organizationLocationId">The OrganizationLocation id as a string</param>
	/// <returns>The OrganizationLocation information(CTMOrganizationLocationDto object).</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(CTMOrganizationLocationDto))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/ORGANIZATION" })]
	[AcceptVerbs("GET")]
	[Route("GetOrganizationLocation/{organizationId}/{organizationLocationId}")]
	public async Task<IHttpActionResult> GetOrganizationLocationAsync([FromUri(Name = "organizationId")] string organizationId, [FromUri(Name = "organizationLocationId")] string organizationLocationId)
	{
		BOMOrganizationLocationModel bomOrganizationLocationModel = new BOMOrganizationLocationModel();
		try
		{
			return await RunApiMethod(base.Request, bomOrganizationLocationModel, () => bomOrganizationLocationModel.ValidateRequest_GetOrganizationLocation(organizationId, organizationLocationId).Result, () => bomOrganizationLocationModel.Process_GetOrganizationLocation(organizationId, organizationLocationId), showReturnObject: true, showResponseMessage: false);
		}
		finally
		{
			if (bomOrganizationLocationModel != null)
			{
				((IDisposable)bomOrganizationLocationModel).Dispose();
			}
		}
	}

	/// <summary>
	/// Posts a new OrganizationLocation.
	/// </summary>
	/// <param name="organizationLocation">The OrganizationLocation data to be posted.</param>
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
	[SwaggerOperation(null, Tags = new string[] { "BOM/ORGANIZATION" })]
	[AcceptVerbs("POST")]
	[Route("PostOrganizationLocation")]
	public async Task<IHttpActionResult> PostOrganizationLocationAsync([FromBody] BOMOrganizationLocationDto organizationLocation)
	{
		BOMOrganizationLocationModel bomOrganizationLocationModel = new BOMOrganizationLocationModel();
		try
		{
			return await RunApiMethod(base.Request, bomOrganizationLocationModel, () => bomOrganizationLocationModel.ValidateRequest_PostOrganizationLocation(organizationLocation).Result, () => bomOrganizationLocationModel.Process_PostOrganizationLocation(organizationLocation), showReturnObject: false, showResponseMessage: true);
		}
		finally
		{
			if (bomOrganizationLocationModel != null)
			{
				((IDisposable)bomOrganizationLocationModel).Dispose();
			}
		}
	}
}
