using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using M1.API.DTOs.BOM.Sales;
using M1.API.DTOs.Core;
using M1.API.Models.BOM.Sales;
using Swashbuckle.Swagger.Annotations;

namespace M1.API.Controllers.BOM.Sales;

[RoutePrefix("api/BOM/Sales")]
public class BOMQuotesController : BOMBaseController
{
	/// <summary>
	/// Returns all existing Quotes with pagination.
	/// </summary>
	/// <param name="pageSize">The required size of the pagination. By default 1000.</param>
	/// <param name="pageNumber">The required number of page from pagination. By default 0, which is the first page.</param>
	/// <returns>BOMQuote object</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(IList<BOMQuoteDto>))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/SALES" })]
	[AcceptVerbs("GET")]
	[Route("GetAllQuotes/{pageSize}/{pageNumber}")]
	public async Task<IHttpActionResult> GetAllQuoteAsync([FromUri(Name = "pageSize")] int pageSize = 1000, [FromUri(Name = "pageNumber")] int pageNumber = 0)
	{
		BOMQuoteModel bomQuoteModel = new BOMQuoteModel();
		try
		{
			return await RunApiMethod(base.Request, bomQuoteModel, () => bomQuoteModel.APIValidationIsTrueFunction(), () => bomQuoteModel.Process_GetAllQuotesAsync(pageSize, pageNumber), showReturnObject: true, showResponseMessage: false);
		}
		finally
		{
			if (bomQuoteModel != null)
			{
				((IDisposable)bomQuoteModel).Dispose();
			}
		}
	}

	/// <summary>
	/// Returns Quote for a given Quote id.
	/// </summary>
	/// <remarks>
	/// This endpoint retrieves a Quote based on its identifier.
	/// </remarks>
	/// <param name="quoteId">The Quote id as a string</param>
	/// <returns>The Quote information(BOMQuoteDto object).</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(BOMQuoteDto))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/SALES" })]
	[AcceptVerbs("GET")]
	[Route("GetQuote/{quoteId}")]
	public async Task<IHttpActionResult> GetQuoteAsync([FromUri(Name = "quoteId")] string quoteId)
	{
		BOMQuoteModel bomQuoteModel = new BOMQuoteModel();
		try
		{
			return await RunApiMethod(base.Request, bomQuoteModel, () => bomQuoteModel.ValidateRequest_GetQuoteAsync(quoteId).Result, () => bomQuoteModel.Process_GetQuoteAsync(quoteId), showReturnObject: true, showResponseMessage: false);
		}
		finally
		{
			if (bomQuoteModel != null)
			{
				((IDisposable)bomQuoteModel).Dispose();
			}
		}
	}

	/// <summary>
	/// Posts a new Quote.
	/// </summary>
	/// <param name="quote">The Quote data to be posted.</param>
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
	[SwaggerOperation(null, Tags = new string[] { "BOM/SALES" })]
	[AcceptVerbs("POST")]
	[Route("PostQuote")]
	public async Task<IHttpActionResult> PostQuoteAsync([FromBody] BOMCreateQuoteDto quote)
	{
		BOMQuoteModel bomQuoteModel = new BOMQuoteModel();
		try
		{
			return await RunApiMethod(base.Request, bomQuoteModel, () => bomQuoteModel.ValidateRequest_PostQuoteAsync(quote).Result, () => bomQuoteModel.Process_PostQuoteAsync(quote), showReturnObject: false, showResponseMessage: true);
		}
		finally
		{
			if (bomQuoteModel != null)
			{
				((IDisposable)bomQuoteModel).Dispose();
			}
		}
	}
}
