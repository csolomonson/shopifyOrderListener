using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using M1.API.DTOs.BOM.Transaction;
using M1.API.DTOs.Core;
using M1.API.Models.BOM.Transaction;
using Swashbuckle.Swagger.Annotations;

namespace M1.API.Controllers.BOM.Transaction;

/// <summary>
/// Controller for handling BOM receipt lines within the API.
/// </summary>
[RoutePrefix("api/BOM/Transaction/ReceiptLine")]
public class BOMReceiptLineController : BOMBaseController
{
	public IBOMReceiptLineModel bomReceiptLineModel;

	/// <summary>
	/// Retrieves all M1 receipt lines with pagination support.
	/// </summary>
	/// <remarks>
	/// This endpoint returns a paginated list of all receipt lines. Pagination is supported via the pageSize and pageNumber parameters.
	/// </remarks>
	/// <param name="pageSize">The number of receipt lines to return per page (default: 1000).</param>
	/// <param name="pageNumber">The page number to retrieve (default: 0).</param>
	/// <returns>A collection of receipt lines.</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(IList<BOMReceiptLineDto>))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/TRANSACTION" })]
	[AcceptVerbs("GET")]
	[Route("GetAllReceiptLines/{pageSize}/{pageNumber}")]
	public async Task<IHttpActionResult> GetAllReceiptLinesAsync([FromUri(Name = "pageSize")] int pageSize = 1000, [FromUri(Name = "pageNumber")] int pageNumber = 0)
	{
		using (bomReceiptLineModel = new BOMReceiptLineModel())
		{
			return await RunApiMethod(base.Request, bomReceiptLineModel, () => bomReceiptLineModel.APIValidationIsTrueFunction(), () => bomReceiptLineModel.Process_GetAllReceiptLines(pageSize, pageNumber), showReturnObject: true, showResponseMessage: false);
		}
	}

	/// <summary>
	/// Returns all receipt lines for a given receipt id.
	/// </summary>
	/// <remarks>
	/// This endpoint retrieves all receipt lines based on receipt identifier.
	/// </remarks>
	/// <param name="receiptId">The receipt id as a string</param>
	/// <returns>The receipt information(BOMReceiptLineDto object).</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(BOMReceiptLineDto))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/TRANSACTION" })]
	[AcceptVerbs("GET")]
	[Route("GetReceiptLine/{receiptId}")]
	public async Task<IHttpActionResult> GetReceiptAsync([FromUri(Name = "receiptId")] string receiptId)
	{
		BOMReceiptModel bomReceiptModel = new BOMReceiptModel();
		using (bomReceiptLineModel = new BOMReceiptLineModel(bomReceiptModel))
		{
			return await RunApiMethod(base.Request, bomReceiptLineModel, () => bomReceiptLineModel.ValidateRequest_GetReceipt(receiptId).Result, () => bomReceiptLineModel.Process_GetReceiptLine(receiptId), showReturnObject: true, showResponseMessage: false);
		}
	}
}
