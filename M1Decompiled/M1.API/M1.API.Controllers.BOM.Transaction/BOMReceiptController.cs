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
/// Controller for handling BOM receipts within the API.
/// </summary>
[RoutePrefix("api/BOM/Transaction/Receipt")]
public class BOMReceiptController : BOMBaseController
{
	public IBOMReceiptModel BomReceiptModel;

	/// <summary>
	/// Retrieves all M1 receipts with pagination support.
	/// </summary>
	/// <remarks>
	/// This endpoint returns a paginated list of all receipts. Pagination is supported via the pageSize and pageNumber parameters.
	/// </remarks>
	/// <param name="pageSize">The number of receipts to return per page (default: 1000).</param>
	/// <param name="pageNumber">The page number to retrieve (default: 0).</param>
	/// <returns>A collection of receipts.</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(IList<BOMReceiptDto>))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/TRANSACTION" })]
	[AcceptVerbs("GET")]
	[Route("GetAllReceipts/{pageSize}/{pageNumber}")]
	public async Task<IHttpActionResult> GetAllReceiptsAsync([FromUri(Name = "pageSize")] int pageSize = 1000, [FromUri(Name = "pageNumber")] int pageNumber = 0)
	{
		using (BomReceiptModel = new BOMReceiptModel())
		{
			return await RunApiMethod(base.Request, BomReceiptModel, () => BomReceiptModel.APIValidationIsTrueFunction(), () => BomReceiptModel.Process_GetAllReceipts(pageSize, pageNumber), showReturnObject: true, showResponseMessage: false);
		}
	}

	/// <summary>
	/// Returns receipt for a given receipt id.
	/// </summary>
	/// <remarks>
	/// This endpoint retrieves a receipt based on its identifier.
	/// </remarks>
	/// <param name="receiptId">The receipt id as a string</param>
	/// <returns>The receipt information(BOMReceiptDto object).</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(BOMReceiptDto))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/TRANSACTION" })]
	[AcceptVerbs("GET")]
	[Route("GetReceipt/{receiptId}")]
	public async Task<IHttpActionResult> GetReceiptAsync([FromUri(Name = "receiptId")] string receiptId)
	{
		using (BomReceiptModel = new BOMReceiptModel())
		{
			return await RunApiMethod(base.Request, BomReceiptModel, () => BomReceiptModel.ValidateRequest_GetReceipt(receiptId).Result, () => BomReceiptModel.Process_GetReceipt(receiptId), showReturnObject: true, showResponseMessage: false);
		}
	}
}
