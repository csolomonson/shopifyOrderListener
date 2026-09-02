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
public class BOMSalesOrdersController : BOMBaseController
{
	/// <summary>
	/// Returns all existing SalesOrders with pagination.
	/// </summary>
	/// <param name="pageSize">The required size of the pagination. By default 1000.</param>
	/// <param name="pageNumber">The required number of page from pagination. By default 0, which is the first page.</param>
	/// <returns>BOMSalesOrder object</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(IList<BOMSalesOrderDto>))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/SALES" })]
	[AcceptVerbs("GET")]
	[Route("GetAllSalesOrders/{pageSize}/{pageNumber}")]
	public async Task<IHttpActionResult> GetAllSalesOrderAsync([FromUri(Name = "pageSize")] int pageSize = 1000, [FromUri(Name = "pageNumber")] int pageNumber = 0)
	{
		BOMSalesOrderModel bomSalesOrderModel = new BOMSalesOrderModel();
		try
		{
			return await RunApiMethod(base.Request, bomSalesOrderModel, () => bomSalesOrderModel.APIValidationIsTrueFunction(), () => bomSalesOrderModel.Process_GetAllSalesOrders(pageSize, pageNumber), showReturnObject: true, showResponseMessage: false);
		}
		finally
		{
			if (bomSalesOrderModel != null)
			{
				((IDisposable)bomSalesOrderModel).Dispose();
			}
		}
	}

	/// <summary>
	/// Returns SalesOrder for a given SalesOrder id.
	/// </summary>
	/// <remarks>
	/// This endpoint retrieves a SalesOrder based on its identifier.
	/// </remarks>
	/// <param name="salesOrderId">The SalesOrder id as a string</param>
	/// <returns>The SalesOrder information(BOMSalesOrderDto object).</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(BOMSalesOrderDto))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/SALES" })]
	[AcceptVerbs("GET")]
	[Route("GetSalesOrder/{salesOrderId}")]
	public async Task<IHttpActionResult> GetSalesOrderAsync([FromUri(Name = "salesOrderId")] string salesOrderId)
	{
		BOMSalesOrderModel bomSalesOrderModel = new BOMSalesOrderModel();
		try
		{
			return await RunApiMethod(base.Request, bomSalesOrderModel, () => bomSalesOrderModel.ValidateRequest_GetSalesOrder(salesOrderId).Result, () => bomSalesOrderModel.Process_GetSalesOrder(salesOrderId), showReturnObject: true, showResponseMessage: false);
		}
		finally
		{
			if (bomSalesOrderModel != null)
			{
				((IDisposable)bomSalesOrderModel).Dispose();
			}
		}
	}
}
