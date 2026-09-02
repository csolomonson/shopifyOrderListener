using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPSalesOrderDeliveryModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all SalesOrderDeliveries with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of SalesOrderDeliveries to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllSalesOrderDeliveries(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving SalesOrderDelivery information based on the specified SalesOrderDelivery Unique Id.
	/// </summary>
	/// <param name="salesOrderDeliveryId">The Unique Id of the SalesOrderDelivery.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetSalesOrderDelivery(Guid salesOrderDeliveryId);

	/// <summary>
	/// Validates the PUT request for creating or updating SalesOrderDelivery information based on the specified SalesOrderDelivery.
	/// </summary>
	/// <param name="salesOrderDelivery">The SalesOrderDelivery details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutSalesOrderDelivery(ERPSalesOrderDeliveryDto salesOrderDelivery);

	/// <summary>
	/// Processes the request to retrieve all SalesOrderDeliveries with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of SalesOrderDeliveries to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of SalesOrderDeliveries DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPSalesOrderDeliveryDto>>> Process_GetAllSalesOrderDeliveries(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific SalesOrderDelivery.
	/// </summary>
	/// <param name="salesOrderDeliveryId">The Unique Id of the SalesOrderDelivery to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the SalesOrderDelivery DTO.</returns>
	Task<ERPResponseMessageDto<ERPSalesOrderDeliveryDto>> Process_GetSalesOrderDelivery(Guid salesOrderDeliveryId);

	/// <summary>
	/// Processes the creating or updating of a SalesOrderDelivery record.
	/// </summary>
	/// <param name="salesOrderDelivery">The SalesOrderDelivery data transfer object (DTO) containing the details of the SalesOrderDelivery to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the SalesOrderDelivery details.</returns>
	Task<ERPResponseMessageDto<ERPSalesOrderDeliveryDto>> Process_PutSalesOrderDelivery(ERPSalesOrderDeliveryDto salesOrderDelivery);

	/// <summary>
	/// Validates the request for deleting a SalesOrderDelivery record.
	/// </summary>
	/// <param name="salesOrderDeliveryId">The Unique Id of the SalesOrderDelivery.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteSalesOrderDelivery(Guid salesOrderDeliveryId);

	/// <summary>
	/// Processes the request to delete a SalesOrderDelivery record.
	/// </summary>
	/// <param name="salesOrderDeliveryId">The Unique Id of the SalesOrderDelivery.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPSalesOrderDeliveryDto>> Process_DeleteSalesOrderDelivery(Guid salesOrderDeliveryId);
}
