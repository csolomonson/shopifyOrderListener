using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPSalesOrderModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all SalesOrders with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of SalesOrders to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllSalesOrders(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving SalesOrder information based on the specified SalesOrder Unique Id.
	/// </summary>
	/// <param name="salesOrderId">The Unique Id of the SalesOrder.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetSalesOrder(Guid salesOrderId);

	/// <summary>
	/// Validates the PUT request for creating or updating SalesOrder information based on the specified SalesOrder.
	/// </summary>
	/// <param name="salesOrder">The SalesOrder details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutSalesOrder(ERPSalesOrderDto salesOrder);

	/// <summary>
	/// Processes the request to retrieve all SalesOrders with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of SalesOrders to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of SalesOrders DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPSalesOrderDto>>> Process_GetAllSalesOrders(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific SalesOrder.
	/// </summary>
	/// <param name="salesOrderId">The Unique Id of the SalesOrder to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the SalesOrder DTO.</returns>
	Task<ERPResponseMessageDto<ERPSalesOrderDto>> Process_GetSalesOrder(Guid salesOrderId);

	/// <summary>
	/// Processes the creating or updating of a SalesOrder record.
	/// </summary>
	/// <param name="salesOrder">The SalesOrder data transfer object (DTO) containing the details of the SalesOrder to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the SalesOrder details.</returns>
	Task<ERPResponseMessageDto<ERPSalesOrderDto>> Process_PutSalesOrder(ERPSalesOrderDto salesOrder);

	/// <summary>
	/// Validates the request for deleting a SalesOrder record.
	/// </summary>
	/// <param name="salesOrderId">The Unique Id of the SalesOrder.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteSalesOrder(Guid salesOrderId);

	/// <summary>
	/// Processes the request to delete a SalesOrder record.
	/// </summary>
	/// <param name="salesOrderId">The Unique Id of the SalesOrder.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPSalesOrderDto>> Process_DeleteSalesOrder(Guid salesOrderId);
}
