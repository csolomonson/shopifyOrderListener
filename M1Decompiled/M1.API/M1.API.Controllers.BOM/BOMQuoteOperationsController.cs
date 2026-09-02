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
public class BOMQuoteOperationsController : BOMBaseController
{
	/// <summary>
	/// Returns all existing QuoteOperations with pagination.
	/// </summary>
	/// <param name="pageSize">The required size of the pagination. By default 1000.</param>
	/// <param name="pageNumber">The required number of page from pagination. By default 0, which is the first page.</param>
	/// <returns>A list of <see cref="T:M1.API.DTOs.BOM.BOMQuoteOperationDto" /> objects if the request is successful.</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(IList<BOMQuoteOperationDto>))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/SALES" })]
	[AcceptVerbs("GET")]
	[Route("GetAllQuoteOperations/{pageSize}/{pageNumber}")]
	public async Task<IHttpActionResult> GetAllQuoteOperationAsync([FromUri(Name = "pageSize")] int pageSize = 1000, [FromUri(Name = "pageNumber")] int pageNumber = 0)
	{
		BOMQuoteOperationModel bomQuoteOperationModel = new BOMQuoteOperationModel();
		try
		{
			return await RunApiMethod(base.Request, bomQuoteOperationModel, () => bomQuoteOperationModel.APIValidationIsTrueFunction(), () => bomQuoteOperationModel.Process_GetAllQuoteOperations(pageSize, pageNumber), showReturnObject: true, showResponseMessage: false);
		}
		finally
		{
			if (bomQuoteOperationModel != null)
			{
				((IDisposable)bomQuoteOperationModel).Dispose();
			}
		}
	}

	/// <summary>
	/// Returns a list of QuoteOperations for a given Quote id.
	/// </summary>
	/// <remarks>
	/// This endpoint retrieves a list of QuoteOperations based on Quote identifier.
	/// </remarks>
	/// <param name="quoteId">The Quote id as a string</param>
	/// <returns>A list of <see cref="T:M1.API.DTOs.BOM.BOMQuoteOperationDto" /> objects if the request is successful.</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(IList<BOMQuoteOperationDto>))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/SALES" })]
	[AcceptVerbs("GET")]
	[Route("GetQuoteOperations/{quoteId}")]
	public async Task<IHttpActionResult> GetQuoteOperationsAsync([FromUri(Name = "quoteId")] string quoteId)
	{
		BOMQuoteOperationModel bomQuoteOperationModel = new BOMQuoteOperationModel();
		try
		{
			return await RunApiMethod(base.Request, bomQuoteOperationModel, () => bomQuoteOperationModel.ValidateRequest_GetQuoteOperation(quoteId).Result, () => bomQuoteOperationModel.Process_GetQuoteOperations(quoteId), showReturnObject: true, showResponseMessage: false);
		}
		finally
		{
			if (bomQuoteOperationModel != null)
			{
				((IDisposable)bomQuoteOperationModel).Dispose();
			}
		}
	}

	/// <summary>
	/// Returns QuoteOperation for a given Quote and QuoteLine id.
	/// </summary>
	/// <remarks>
	/// This endpoint retrieves a list of QuoteOperations based on Quote and QuoteLine identifiers.
	/// </remarks>
	/// <param name="quoteId">The Quote id as a string</param>
	/// <param name="quoteLineId">The QuoteLine id as a string</param>
	/// <returns>A list of <see cref="T:M1.API.DTOs.BOM.BOMQuoteOperationDto" /> objects if the request is successful.</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(IList<BOMQuoteOperationDto>))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/SALES" })]
	[AcceptVerbs("GET")]
	[Route("GetQuoteOperations/{quoteId}/{quoteLineId}")]
	public async Task<IHttpActionResult> GetQuoteOperationsAsync([FromUri(Name = "quoteId")] string quoteId, [FromUri(Name = "quoteLineId")] string quoteLineId)
	{
		BOMQuoteOperationModel bomQuoteOperationModel = new BOMQuoteOperationModel();
		try
		{
			return await RunApiMethod(base.Request, bomQuoteOperationModel, () => bomQuoteOperationModel.ValidateRequest_GetQuoteOperation(quoteId, quoteLineId).Result, () => bomQuoteOperationModel.Process_GetQuoteOperations(quoteId, quoteLineId), showReturnObject: true, showResponseMessage: false);
		}
		finally
		{
			if (bomQuoteOperationModel != null)
			{
				((IDisposable)bomQuoteOperationModel).Dispose();
			}
		}
	}

	/// <summary>
	/// Returns QuoteOperation for a given Quote, QuoteLine, and QuoteAssembly id.
	/// </summary>
	/// <remarks>
	/// This endpoint retrieves a list of QuoteOperations based on Quote, QuoteLine and QuoteAssembly identifiers.
	/// </remarks>
	/// <param name="quoteId">The Quote id as a string</param>
	/// <param name="quoteLineId">The QuoteLine id as a string</param>
	/// <param name="quoteAssemblyId">The QuoteAssembly id as a string</param>
	/// <returns>A list of <see cref="T:M1.API.DTOs.BOM.BOMQuoteOperationDto" /> objects if the request is successful.</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(IList<BOMQuoteOperationDto>))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/SALES" })]
	[AcceptVerbs("GET")]
	[Route("GetQuoteOperations/{quoteId}/{quoteLineId}/{quoteAssemblyId}")]
	public async Task<IHttpActionResult> GetQuoteOperationsAsync([FromUri(Name = "quoteId")] string quoteId, [FromUri(Name = "quoteLineId")] string quoteLineId, [FromUri(Name = "quoteAssemblyId")] string quoteAssemblyId)
	{
		BOMQuoteOperationModel bomQuoteOperationModel = new BOMQuoteOperationModel();
		try
		{
			return await RunApiMethod(base.Request, bomQuoteOperationModel, () => bomQuoteOperationModel.ValidateRequest_GetQuoteOperation(quoteId, quoteLineId, quoteAssemblyId).Result, () => bomQuoteOperationModel.Process_GetQuoteOperations(quoteId, quoteLineId, quoteAssemblyId), showReturnObject: true, showResponseMessage: false);
		}
		finally
		{
			if (bomQuoteOperationModel != null)
			{
				((IDisposable)bomQuoteOperationModel).Dispose();
			}
		}
	}
}
