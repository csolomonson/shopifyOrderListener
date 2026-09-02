using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using M1.API.DTOs.BOM.Sales;
using M1.API.DTOs.Core;
using M1.API.DTOs.Custom.Sales;
using M1.API.Models.BOM.Sales;
using Swashbuckle.Swagger.Annotations;

namespace M1.API.Controllers.BOM.Sales;

[RoutePrefix("api/BOM/Sales")]
public class BOMQuoteLinesController : BOMBaseController
{
	/// <summary>
	/// Returns all existing QuoteLines with pagination.
	/// </summary>
	/// <param name="pageSize">The required size of the pagination. By default 1000.</param>
	/// <param name="pageNumber">The required number of page from pagination. By default 0, which is the first page.</param>
	/// <returns>BOMQuoteLine object</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(IList<BOMQuoteLineDto>))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/SALES" })]
	[AcceptVerbs("GET")]
	[Route("GetAllQuoteLines/{pageSize}/{pageNumber}")]
	public async Task<IHttpActionResult> GetAllQuoteLineAsync([FromUri(Name = "pageSize")] int pageSize = 1000, [FromUri(Name = "pageNumber")] int pageNumber = 0)
	{
		BOMQuoteLineModel bomQuoteLineModel = new BOMQuoteLineModel();
		try
		{
			return await RunApiMethod(base.Request, bomQuoteLineModel, () => bomQuoteLineModel.APIValidationIsTrueFunction(), () => bomQuoteLineModel.Process_GetAllQuoteLines(pageSize, pageNumber), showReturnObject: true, showResponseMessage: false);
		}
		finally
		{
			if (bomQuoteLineModel != null)
			{
				((IDisposable)bomQuoteLineModel).Dispose();
			}
		}
	}

	/// <summary>
	/// Returns quote and quote lines details for a given M1 quote id or GUID. Do not pass quote id if it has special characters (other than Aa-Zz0-9.-) pass GUID instead
	/// </summary>
	/// <param name="quoteId">The M1 Quote Id or GUID of the quote as a string</param>
	/// <returns>CTMBOMQuoteLineDto object</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(CTMBOMQuoteLineDto))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/SALES" })]
	[AcceptVerbs("GET")]
	[Route("GetQuoteLines/{quoteId}")]
	public async Task<IHttpActionResult> GetQuoteLinesAsync([FromUri(Name = "quoteId")] string quoteId)
	{
		BOMQuoteModel bomQuoteModel = new BOMQuoteModel();
		try
		{
			return await RunApiMethod(base.Request, bomQuoteModel, () => bomQuoteModel.ValidateRequest_GetQuoteAsync(quoteId).Result, () => bomQuoteModel.Process_GetQuoteLinesAsync(bomQuoteModel.QuoteKeyDictionary["qmpQuoteID"].ToString()), showReturnObject: true, showResponseMessage: false);
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
	/// Returns QuoteLine for a given Quote id and QuoteLine Id.
	/// </summary>
	/// <remarks>
	/// This endpoint retrieves a QuoteLine identified by the Quote identifier and its own unique identifier.
	/// </remarks>
	/// <param name="quoteId">The Quote id as a string</param>
	/// <param name="quoteLineId">The QuoteLine id as a string</param>
	/// <returns>The QuoteLine information(BOMQuoteLineDto object).</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(BOMQuoteLineDto))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/SALES" })]
	[AcceptVerbs("GET")]
	[Route("GetQuoteLine/{quoteId}/{quoteLineId}")]
	public async Task<IHttpActionResult> GetQuoteLineAsync([FromUri(Name = "quoteId")] string quoteId, [FromUri(Name = "quoteLineId")] string quoteLineId)
	{
		BOMQuoteLineModel bomQuoteLineModel = new BOMQuoteLineModel();
		try
		{
			return await RunApiMethod(base.Request, bomQuoteLineModel, () => bomQuoteLineModel.ValidateRequest_GetQuoteLine(quoteId, quoteLineId).Result, () => bomQuoteLineModel.Process_GetQuoteLine(quoteId, quoteLineId), showReturnObject: true, showResponseMessage: false);
		}
		finally
		{
			if (bomQuoteLineModel != null)
			{
				((IDisposable)bomQuoteLineModel).Dispose();
			}
		}
	}

	/// <summary>
	/// Posts a new QuoteLine.
	/// </summary>
	/// <param name="quoteLine">The QuoteLine data to be posted.</param>
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
	[Route("PostQuoteLine")]
	public async Task<IHttpActionResult> PostQuoteLineAsync([FromBody] BOMCreateQuoteLineDto quoteLine)
	{
		BOMQuoteLineModel bomQuoteLineModel = new BOMQuoteLineModel();
		try
		{
			return await RunApiMethod(base.Request, bomQuoteLineModel, () => bomQuoteLineModel.ValidateRequest_PostQuoteLineAsync(quoteLine).Result, () => bomQuoteLineModel.Process_PostQuoteLineAsync(quoteLine), showReturnObject: false, showResponseMessage: true);
		}
		finally
		{
			if (bomQuoteLineModel != null)
			{
				((IDisposable)bomQuoteLineModel).Dispose();
			}
		}
	}
}
