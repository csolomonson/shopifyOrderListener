using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.DTOs.BOM.Sales;
using M1.API.DTOs.Core;

namespace M1.API.Models.BOM.Sales;

public interface IBOMSalesOrderModel : IBOMBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving SalesOrder information based on the specified SalesOrder ID.
	/// </summary>
	/// <param name="salesOrderId">The ID of the SalesOrder.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetSalesOrder(string salesOrderId);

	/// <summary>
	/// Validates the POST request for retrieving SalesOrder information based on the specified SalesOrder.
	/// </summary>
	/// <param name="salesOrder">The SalesOrder details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PostSalesOrder(BOMSalesOrderDto salesOrder);

	/// <summary>
	/// Processes the request to retrieve all SalesOrders with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of SalesOrders to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a BOM response message DTO with a list of SalesOrders DTOs.</returns>
	Task<BOMResponseMessageDto<IList<BOMSalesOrderDto>>> Process_GetAllSalesOrders(int pageSize, int pageNumber);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific SalesOrder.
	/// </summary>
	/// <param name="salesOrderId">The ID of the SalesOrder to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a BOM response message DTO with the SalesOrder DTO.</returns>
	Task<BOMResponseMessageDto<BOMSalesOrderDto>> Process_GetSalesOrder(string salesOrderId);
}
