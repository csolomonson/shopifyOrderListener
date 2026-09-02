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
public class BOMQuoteMaterialsController : BOMBaseController
{
	/// <summary>
	/// Returns all existing QuoteMaterials with pagination.
	/// </summary>
	/// <param name="pageSize">The required size of the pagination. By default 1000.</param>
	/// <param name="pageNumber">The required number of page from pagination. By default 0, which is the first page.</param>
	/// <returns>BOMQuoteMaterial object</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(IList<BOMQuoteMaterialDto>))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/SALES" })]
	[AcceptVerbs("GET")]
	[Route("GetAllQuoteMaterials/{pageSize}/{pageNumber}")]
	public async Task<IHttpActionResult> GetAllQuoteMaterialAsync([FromUri(Name = "pageSize")] int pageSize = 1000, [FromUri(Name = "pageNumber")] int pageNumber = 0)
	{
		BOMQuoteMaterialModel bomQuoteMaterialModel = new BOMQuoteMaterialModel();
		try
		{
			return await RunApiMethod(base.Request, bomQuoteMaterialModel, () => bomQuoteMaterialModel.APIValidationIsTrueFunction(), () => bomQuoteMaterialModel.Process_GetAllQuoteMaterials(pageSize, pageNumber), showReturnObject: true, showResponseMessage: false);
		}
		finally
		{
			if (bomQuoteMaterialModel != null)
			{
				((IDisposable)bomQuoteMaterialModel).Dispose();
			}
		}
	}

	/// <summary>
	/// Returns a list of QuoteMaterial for a given Quote id.
	/// </summary>
	/// <remarks>
	/// This endpoint retrieves QuoteMaterials based on Quote identifier.
	/// </remarks>
	/// <param name="quoteId">The Quote id as a string</param>
	/// <returns>An <see cref="T:System.Web.Http.IHttpActionResult" /> containing the list of QuoteMaterial information(BOMQuoteMaterialDto object).</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(IList<BOMQuoteMaterialDto>))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/SALES" })]
	[AcceptVerbs("GET")]
	[Route("GetQuoteMaterials/{quoteId}")]
	public async Task<IHttpActionResult> GetQuoteMaterialAsync([FromUri(Name = "quoteId")] string quoteId)
	{
		BOMQuoteMaterialModel bomQuoteMaterialModel = new BOMQuoteMaterialModel();
		try
		{
			return await RunApiMethod(base.Request, bomQuoteMaterialModel, () => bomQuoteMaterialModel.ValidateRequest_GetQuoteMaterialsAsync(quoteId).Result, () => bomQuoteMaterialModel.Process_GetQuoteMaterialsAsync(quoteId), showReturnObject: true, showResponseMessage: false);
		}
		finally
		{
			if (bomQuoteMaterialModel != null)
			{
				((IDisposable)bomQuoteMaterialModel).Dispose();
			}
		}
	}

	/// <summary>
	/// Returns a list of QuoteMaterial for a given QuoteId and QuoteLineId.
	/// </summary>
	/// <remarks>
	/// This endpoint retrieves QuoteMaterials based on QuoteId and QuoteLineId.
	/// </remarks>
	/// <param name="quoteId">The Quote id as a string</param>
	/// <param name="quoteLineId">The QuoteLine id as a string</param>
	/// <returns>An <see cref="T:System.Web.Http.IHttpActionResult" /> containing the list of QuoteMaterial information(BOMQuoteMaterialDto object).</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(IList<BOMQuoteMaterialDto>))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/SALES" })]
	[AcceptVerbs("GET")]
	[Route("GetQuoteMaterials/{quoteId}/{quoteLineId}")]
	public async Task<IHttpActionResult> GetQuoteMaterialAsync([FromUri(Name = "quoteId")] string quoteId, [FromUri(Name = "quoteLineId")] string quoteLineId)
	{
		BOMQuoteMaterialModel bomQuoteMaterialModel = new BOMQuoteMaterialModel();
		try
		{
			return await RunApiMethod(base.Request, bomQuoteMaterialModel, () => bomQuoteMaterialModel.ValidateRequest_GetQuoteMaterialsAsync(quoteId, quoteLineId).Result, () => bomQuoteMaterialModel.Process_GetQuoteMaterialsAsync(quoteId, quoteLineId), showReturnObject: true, showResponseMessage: false);
		}
		finally
		{
			if (bomQuoteMaterialModel != null)
			{
				((IDisposable)bomQuoteMaterialModel).Dispose();
			}
		}
	}

	/// <summary>
	/// Returns a list of QuoteMaterial for a given QuoteId, QuoteLineId and QuoteAssemblyId.
	/// </summary>
	/// <remarks>
	/// This endpoint retrieves QuoteMaterials based on QuoteId, QuoteLineId and QuoteAssemblyId.
	/// </remarks>
	/// <param name="quoteId">The Quote id as a string</param>
	/// <param name="quoteLineId">The QuoteLine id as a string</param>
	/// <param name="quoteAssemblyId">The QuoteAssembly id as a string</param>
	/// <returns>An <see cref="T:System.Web.Http.IHttpActionResult" /> containing the list of QuoteMaterial information(BOMQuoteMaterialDto object).</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(IList<BOMQuoteMaterialDto>))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/SALES" })]
	[AcceptVerbs("GET")]
	[Route("GetQuoteMaterials/{quoteId}/{quoteLineId}/{quoteAssemblyId}")]
	public async Task<IHttpActionResult> GetQuoteMaterialAsync([FromUri(Name = "quoteId")] string quoteId, [FromUri(Name = "quoteLineId")] string quoteLineId, [FromUri(Name = "quoteAssemblyId")] string quoteAssemblyId)
	{
		BOMQuoteMaterialModel bomQuoteMaterialModel = new BOMQuoteMaterialModel();
		try
		{
			return await RunApiMethod(base.Request, bomQuoteMaterialModel, () => bomQuoteMaterialModel.ValidateRequest_GetQuoteMaterialsAsync(quoteId, quoteLineId, quoteAssemblyId).Result, () => bomQuoteMaterialModel.Process_GetQuoteMaterialsAsync(quoteId, quoteLineId, quoteAssemblyId), showReturnObject: true, showResponseMessage: false);
		}
		finally
		{
			if (bomQuoteMaterialModel != null)
			{
				((IDisposable)bomQuoteMaterialModel).Dispose();
			}
		}
	}

	/// <summary>
	/// Posts a new QuoteMaterial.
	/// </summary>
	/// <param name="quoteMaterial">The QuoteMaterial data to be posted.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:System.Web.Http.IHttpActionResult" /> indicating the result of the post quote material,
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
	[Route("PostQuoteMaterial")]
	public async Task<IHttpActionResult> PostQuoteMaterialAsync([FromBody] BOMCreateQuoteMaterialDto quoteMaterial)
	{
		BOMQuoteMaterialModel bomQuoteMaterialModel = new BOMQuoteMaterialModel();
		try
		{
			return await RunApiMethod(base.Request, bomQuoteMaterialModel, () => bomQuoteMaterialModel.ValidateRequest_PostQuoteMaterialAsync(quoteMaterial).Result, () => bomQuoteMaterialModel.Process_PostQuoteMaterialAsync(quoteMaterial), showReturnObject: false, showResponseMessage: true);
		}
		finally
		{
			if (bomQuoteMaterialModel != null)
			{
				((IDisposable)bomQuoteMaterialModel).Dispose();
			}
		}
	}
}
