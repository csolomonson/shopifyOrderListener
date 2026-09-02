using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using M1.API.DTOs.BOM;
using M1.API.DTOs.BOM.Sales;
using M1.API.DTOs.Core;
using M1.API.Models.BOM;
using Swashbuckle.Swagger.Annotations;

namespace M1.API.Controllers.BOM;

[RoutePrefix("api/BOM/Sales")]
public class BOMQuoteAssembliesController : BOMBaseController
{
	/// <summary>
	/// Returns all existing QuoteAssemblies with pagination.
	/// </summary>
	/// <param name="pageSize">The required size of the pagination. By default 1000.</param>
	/// <param name="pageNumber">The required number of page from pagination. By default 0, which is the first page.</param>
	/// <returns>BOMQuoteAssembly object</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(IList<BOMQuoteAssemblyDto>))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/SALES" })]
	[AcceptVerbs("GET")]
	[Route("GetAllQuoteAssemblies/{pageSize}/{pageNumber}")]
	public async Task<IHttpActionResult> GetAllQuoteAssemblyAsync([FromUri(Name = "pageSize")] int pageSize = 1000, [FromUri(Name = "pageNumber")] int pageNumber = 0)
	{
		BOMQuoteAssemblyModel bomQuoteAssemblyModel = new BOMQuoteAssemblyModel();
		try
		{
			return await RunApiMethod(base.Request, bomQuoteAssemblyModel, () => bomQuoteAssemblyModel.APIValidationIsTrueFunction(), () => bomQuoteAssemblyModel.Process_GetAllQuoteAssemblies(pageSize, pageNumber), showReturnObject: true, showResponseMessage: false);
		}
		finally
		{
			if (bomQuoteAssemblyModel != null)
			{
				((IDisposable)bomQuoteAssemblyModel).Dispose();
			}
		}
	}

	/// <summary>
	/// Returns QuoteAssemblies for a given Quote id.
	/// </summary>
	/// <remarks>
	/// This endpoint retrieves a list of QuoteAssemblies based on Quote identifier.
	/// </remarks>
	/// <param name="quoteId">The Quote id as a string</param>
	/// <returns>A list of QuoteAssemblies represented as <see cref="T:System.Collections.Generic.IList`1" />.</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(IList<BOMQuoteAssemblyDto>))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/SALES" })]
	[AcceptVerbs("GET")]
	[Route("GetQuoteAssemblies/{quoteId}")]
	public async Task<IHttpActionResult> GetQuoteAssemblyAsync([FromUri(Name = "quoteId")] string quoteId)
	{
		BOMQuoteAssemblyModel bomQuoteAssemblyModel = new BOMQuoteAssemblyModel();
		try
		{
			return await RunApiMethod(base.Request, bomQuoteAssemblyModel, () => bomQuoteAssemblyModel.ValidateRequest_GetQuoteAssembly(quoteId).Result, () => bomQuoteAssemblyModel.Process_GetQuoteAssemblies(quoteId), showReturnObject: true, showResponseMessage: false);
		}
		finally
		{
			if (bomQuoteAssemblyModel != null)
			{
				((IDisposable)bomQuoteAssemblyModel).Dispose();
			}
		}
	}

	/// <summary>
	/// Returns QuoteAssemblies for a given Quote id and Quote Line id.
	/// </summary>
	/// <remarks>
	/// This endpoint retrieves a list of QuoteAssemblies based on Quote and QuoteLine identifier.
	/// </remarks>
	/// <param name="quoteId">The Quote id as a string</param>
	/// <param name="quoteLineId">The QuoteLine id as a string</param>
	/// <returns>A list of QuoteAssemblies represented as <see cref="T:System.Collections.Generic.IList`1" />.</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(IList<BOMQuoteAssemblyDto>))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/SALES" })]
	[AcceptVerbs("GET")]
	[Route("GetQuoteAssemblies/{quoteId}/{quoteLineId}")]
	public async Task<IHttpActionResult> GetQuoteAssemblyAsync([FromUri(Name = "quoteId")] string quoteId, [FromUri(Name = "quoteLineId")] string quoteLineId)
	{
		BOMQuoteAssemblyModel bomQuoteAssemblyModel = new BOMQuoteAssemblyModel();
		try
		{
			return await RunApiMethod(base.Request, bomQuoteAssemblyModel, () => bomQuoteAssemblyModel.ValidateRequest_GetQuoteAssembly(quoteId, quoteLineId).Result, () => bomQuoteAssemblyModel.Process_GetQuoteAssemblies(quoteId, quoteLineId), showReturnObject: true, showResponseMessage: false);
		}
		finally
		{
			if (bomQuoteAssemblyModel != null)
			{
				((IDisposable)bomQuoteAssemblyModel).Dispose();
			}
		}
	}

	/// <summary>
	/// Posts a new QuoteAssembly.
	/// </summary>
	/// <param name="quoteAssembly">The QuoteAssembly data to be posted.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:System.Web.Http.IHttpActionResult" /> indicating the result of the post quote assembly,
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
	[Route("PostQuoteAssembly")]
	public async Task<IHttpActionResult> PostQuoteAssemblyAsync([FromBody] BOMCreateQuoteAssemblyDto quoteAssembly)
	{
		BOMQuoteAssemblyModel bomQuoteAssemblyModel = new BOMQuoteAssemblyModel();
		try
		{
			return await RunApiMethod(base.Request, bomQuoteAssemblyModel, () => bomQuoteAssemblyModel.ValidateRequest_PostQuoteAssemblyAsync(quoteAssembly).Result, () => bomQuoteAssemblyModel.Process_PostQuoteAssemblyAsync(quoteAssembly), showReturnObject: false, showResponseMessage: true);
		}
		finally
		{
			if (bomQuoteAssemblyModel != null)
			{
				((IDisposable)bomQuoteAssemblyModel).Dispose();
			}
		}
	}
}
