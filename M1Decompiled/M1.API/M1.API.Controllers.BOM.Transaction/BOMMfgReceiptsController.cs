using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using M1.API.DTOs.BOM.Transaction;
using M1.API.DTOs.Core;
using M1.API.Models.BOM.Transaction;
using Swashbuckle.Swagger.Annotations;

namespace M1.API.Controllers.BOM.Transaction;

[RoutePrefix("api/BOM/Transaction/MfgReceipt")]
public class BOMMfgReceiptsController : BOMBaseController
{
	/// <summary>
	/// Returns all existing MfgReceipts with pagination.
	/// </summary>
	/// <param name="pageSize">The required size of the pagination. By default 1000.</param>
	/// <param name="pageNumber">The required number of page from pagination. By default 0, which is the first page.</param>
	/// <returns>BOMMfgReceipt object</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(IList<BOMMfgReceiptDto>))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/TRANSACTION" })]
	[AcceptVerbs("GET")]
	[Route("GetAllMfgReceipts/{pageSize}/{pageNumber}")]
	public async Task<IHttpActionResult> GetAllMfgReceiptAsync([FromUri(Name = "pageSize")] int pageSize = 1000, [FromUri(Name = "pageNumber")] int pageNumber = 0)
	{
		BOMMfgReceiptModel bomMfgReceiptModel = new BOMMfgReceiptModel();
		try
		{
			return await RunApiMethod(base.Request, bomMfgReceiptModel, () => bomMfgReceiptModel.APIValidationIsTrueFunction(), () => bomMfgReceiptModel.Process_GetAllMfgReceipts(pageSize, pageNumber), showReturnObject: true, showResponseMessage: false);
		}
		finally
		{
			if (bomMfgReceiptModel != null)
			{
				((IDisposable)bomMfgReceiptModel).Dispose();
			}
		}
	}

	/// <summary>
	/// Returns mfg receipt for a given mfg receipt id.
	/// </summary>
	/// <remarks>
	/// This endpoint retrieves a mfg receipt based on its identifier.
	/// </remarks>
	/// <param name="mfgReceiptId">The mfg receipt id as a string</param>
	/// <returns>The mfg receipt information(BOMMfgReceiptDto object).</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(BOMMfgReceiptDto))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/TRANSACTION" })]
	[AcceptVerbs("GET")]
	[Route("GetMfgReceipt/{mfgReceiptId}")]
	public async Task<IHttpActionResult> GetMfgReceiptAsync([FromUri(Name = "mfgReceiptId")] string mfgReceiptId)
	{
		BOMMfgReceiptModel bomMfgReceiptModel = new BOMMfgReceiptModel();
		try
		{
			return await RunApiMethod(base.Request, bomMfgReceiptModel, () => bomMfgReceiptModel.ValidateRequest_GetMfgReceipt(mfgReceiptId).Result, () => bomMfgReceiptModel.Process_GetMfgReceipt(mfgReceiptId), showReturnObject: true, showResponseMessage: false);
		}
		finally
		{
			if (bomMfgReceiptModel != null)
			{
				((IDisposable)bomMfgReceiptModel).Dispose();
			}
		}
	}
}
