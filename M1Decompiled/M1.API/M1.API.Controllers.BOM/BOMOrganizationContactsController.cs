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
public class BOMOrganizationContactsController : BOMBaseController
{
	/// <summary>
	/// Returns all existing OrganizationContacts with pagination.
	/// </summary>
	/// <param name="pageSize">The required size of the pagination. By default 1000.</param>
	/// <param name="pageNumber">The required number of page from pagination. By default 0, which is the first page.</param>
	/// <returns>CTMOrganizationContactDto object</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(IList<CTMOrganizationContactDto>))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/ORGANIZATION" })]
	[AcceptVerbs("GET")]
	[Route("GetAllOrganizationContacts/{pageSize}/{pageNumber}")]
	public async Task<IHttpActionResult> GetAllOrganizationContactAsync([FromUri(Name = "pageSize")] int pageSize = 1000, [FromUri(Name = "pageNumber")] int pageNumber = 0)
	{
		BOMOrganizationContactModel bomOrganizationContactModel = new BOMOrganizationContactModel();
		try
		{
			return await RunApiMethod(base.Request, bomOrganizationContactModel, () => bomOrganizationContactModel.APIValidationIsTrueFunction(), () => bomOrganizationContactModel.Process_GetAllOrganizationContacts(pageSize, pageNumber), showReturnObject: true, showResponseMessage: false);
		}
		finally
		{
			if (bomOrganizationContactModel != null)
			{
				((IDisposable)bomOrganizationContactModel).Dispose();
			}
		}
	}

	/// <summary>
	/// Returns OrganizationContact for a given Organization id, Location id and Contact id
	/// </summary>
	/// <remarks>
	/// This endpoint retrieves a OrganizationContact based on Organization Id, Location Id and its identifier.
	/// </remarks>
	/// <param name="organizationId">The Organization id as a string</param>
	/// <param name="locationId">The Location id as a string</param>
	/// <param name="contactId">The Contact id as a string</param>
	/// <returns>The OrganizationContact information(CTMOrganizationContactDto object).</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(CTMOrganizationContactDto))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/ORGANIZATION" })]
	[AcceptVerbs("GET")]
	[Route("GetOrganizationContact/{organizationId}/{locationId}/{contactId}")]
	public async Task<IHttpActionResult> GetOrganizationContactAsync([FromUri(Name = "organizationId")] string organizationId, [FromUri(Name = "locationId")] string locationId, [FromUri(Name = "contactId")] string contactId)
	{
		BOMOrganizationContactModel bomOrganizationContactModel = new BOMOrganizationContactModel();
		try
		{
			return await RunApiMethod(base.Request, bomOrganizationContactModel, () => bomOrganizationContactModel.ValidateRequest_GetOrganizationContact(organizationId, locationId, contactId).Result, () => bomOrganizationContactModel.Process_GetOrganizationContact(organizationId, locationId, contactId), showReturnObject: true, showResponseMessage: false);
		}
		finally
		{
			if (bomOrganizationContactModel != null)
			{
				((IDisposable)bomOrganizationContactModel).Dispose();
			}
		}
	}

	/// <summary>
	/// Posts a new OrganizationContact.
	/// </summary>
	/// <param name="organizationContact">The OrganizationContact data to be posted.</param>
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
	[Route("PostOrganizationContact")]
	public async Task<IHttpActionResult> PostOrganizationContactAsync([FromBody] BOMOrganizationContactDto organizationContact)
	{
		BOMOrganizationContactModel bomOrganizationContactModel = new BOMOrganizationContactModel();
		try
		{
			return await RunApiMethod(base.Request, bomOrganizationContactModel, () => bomOrganizationContactModel.ValidateRequest_PostOrganizationContact(organizationContact).Result, () => bomOrganizationContactModel.Process_PostOrganizationContact(organizationContact), showReturnObject: false, showResponseMessage: true);
		}
		finally
		{
			if (bomOrganizationContactModel != null)
			{
				((IDisposable)bomOrganizationContactModel).Dispose();
			}
		}
	}
}
