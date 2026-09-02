using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPSalesOrderLineModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all SalesOrderLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of SalesOrderLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllSalesOrderLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving SalesOrderLine information based on the specified SalesOrderLine Unique Id.
	/// </summary>
	/// <param name="salesOrderLineId">The Unique Id of the SalesOrderLine.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetSalesOrderLine(Guid salesOrderLineId);

	/// <summary>
	/// Validates the PUT request for creating or updating SalesOrderLine information based on the specified SalesOrderLine.
	/// </summary>
	/// <param name="salesOrderLine">The SalesOrderLine details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutSalesOrderLine(ERPSalesOrderLineDto salesOrderLine);

	/// <summary>
	/// Processes the request to retrieve all SalesOrderLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of SalesOrderLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of SalesOrderLines DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPSalesOrderLineDto>>> Process_GetAllSalesOrderLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific SalesOrderLine.
	/// </summary>
	/// <param name="salesOrderLineId">The Unique Id of the SalesOrderLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the SalesOrderLine DTO.</returns>
	Task<ERPResponseMessageDto<ERPSalesOrderLineDto>> Process_GetSalesOrderLine(Guid salesOrderLineId);

	/// <summary>
	/// Processes the creating or updating of a SalesOrderLine record.
	/// </summary>
	/// <param name="salesOrderLine">The SalesOrderLine data transfer object (DTO) containing the details of the SalesOrderLine to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the SalesOrderLine details.</returns>
	Task<ERPResponseMessageDto<ERPSalesOrderLineDto>> Process_PutSalesOrderLine(ERPSalesOrderLineDto salesOrderLine);

	/// <summary>
	/// Validates the request for deleting a SalesOrderLine record.
	/// </summary>
	/// <param name="salesOrderLineId">The Unique Id of the SalesOrderLine.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteSalesOrderLine(Guid salesOrderLineId);

	/// <summary>
	/// Processes the request to delete a SalesOrderLine record.
	/// </summary>
	/// <param name="salesOrderLineId">The Unique Id of the SalesOrderLine.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPSalesOrderLineDto>> Process_DeleteSalesOrderLine(Guid salesOrderLineId);
}
