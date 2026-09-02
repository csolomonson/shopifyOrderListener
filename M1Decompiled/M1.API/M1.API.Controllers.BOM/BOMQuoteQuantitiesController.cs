using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using M1.API.DTOs.BOM;
using M1.API.DTOs.Core;
using M1.API.Models.BOM;
using Swashbuckle.Swagger.Annotations;

namespace M1.API.Controllers.BOM;

[RoutePrefix("api/BOM/Sales")]
public class BOMQuoteQuantitiesController : BOMBaseController
{
	/// <summary>
	/// Returns all existing QuoteQuantities with pagination.
	/// </summary>
	/// <param name="pageSize">The required size of the pagination. By default 1000.</param>
	/// <param name="pageNumber">The required number of page from pagination. By default 0, which is the first page.</param>
	/// <returns>A List of BOMQuoteQuantity object</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(IList<BOMQuoteQuantityDto>))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/SALES" })]
	[AcceptVerbs("GET")]
	[Route("GetAllQuoteQuantities/{pageSize}/{pageNumber}")]
	public async Task<IHttpActionResult> GetAllQuoteQuantityAsync([FromUri(Name = "pageSize")] int pageSize = 1000, [FromUri(Name = "pageNumber")] int pageNumber = 0)
	{
		BOMQuoteQuantityModel bomQuoteQuantityModel = new BOMQuoteQuantityModel();
		try
		{
			return await RunApiMethod(base.Request, bomQuoteQuantityModel, () => bomQuoteQuantityModel.APIValidationIsTrueFunction(), () => bomQuoteQuantityModel.Process_GetAllQuoteQuantities(pageSize, pageNumber), showReturnObject: true, showResponseMessage: false);
		}
		finally
		{
			if (bomQuoteQuantityModel != null)
			{
				((IDisposable)bomQuoteQuantityModel).Dispose();
			}
		}
	}

	/// <summary>
	/// Returns QuoteQuantities for a given Quote Id.
	/// </summary>
	/// <remarks>
	/// Returns all existing QuoteQuantities for a QuoteId.
	/// </remarks>
	/// <param name="quoteId">The Quote id as a string</param>
	/// <returns>A List of BOMQuoteQuantityDto object</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(IList<BOMQuoteQuantityDto>))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/SALES" })]
	[AcceptVerbs("GET")]
	[Route("GetQuoteQuantities/{quoteId}")]
	public async Task<IHttpActionResult> GetQuoteQuantityAsync([FromUri(Name = "quoteId")] string quoteId)
	{
		BOMQuoteQuantityModel bomQuoteQuantityModel = new BOMQuoteQuantityModel();
		try
		{
			return await RunApiMethod(base.Request, bomQuoteQuantityModel, () => bomQuoteQuantityModel.ValidateRequest_GetQuoteQuantity(quoteId).Result, () => bomQuoteQuantityModel.Process_GetQuoteQuantities(quoteId), showReturnObject: true, showResponseMessage: false);
		}
		finally
		{
			if (bomQuoteQuantityModel != null)
			{
				((IDisposable)bomQuoteQuantityModel).Dispose();
			}
		}
	}

	/// <summary>
	/// Returns QuoteQuantities for a given Quote Id and Quote Line Id.
	/// </summary>
	/// <remarks>
	/// Returns all existing QuoteQuantities for a QuoteId and Quote Line Id.
	/// </remarks>
	/// <param name="quoteId">The Quote id as a string</param>
	/// <param name="quoteLineId">The Quote Line id as a string</param>
	/// <returns>A List of BOMQuoteQuantityDto object</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(IList<BOMQuoteQuantityDto>))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/SALES" })]
	[AcceptVerbs("GET")]
	[Route("GetQuoteQuantities/{quoteId}/{quoteLineId}")]
	public async Task<IHttpActionResult> GetQuoteQuantityAsync([FromUri(Name = "quoteId")] string quoteId, [FromUri(Name = "quoteLineId")] string quoteLineId)
	{
		BOMQuoteQuantityModel bomQuoteQuantityModel = new BOMQuoteQuantityModel();
		try
		{
			return await RunApiMethod(base.Request, bomQuoteQuantityModel, () => bomQuoteQuantityModel.ValidateRequest_GetQuoteQuantity(quoteId, quoteLineId).Result, () => bomQuoteQuantityModel.Process_GetQuoteQuantities(quoteId, quoteLineId), showReturnObject: true, showResponseMessage: false);
		}
		finally
		{
			if (bomQuoteQuantityModel != null)
			{
				((IDisposable)bomQuoteQuantityModel).Dispose();
			}
		}
	}
}
