using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPSalesOrderComponentModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all SalesOrderComponents with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of SalesOrderComponents to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllSalesOrderComponents(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving SalesOrderComponent information based on the specified SalesOrderComponent Unique Id.
	/// </summary>
	/// <param name="salesOrderComponentId">The Unique Id of the SalesOrderComponent.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetSalesOrderComponent(Guid salesOrderComponentId);

	/// <summary>
	/// Validates the PUT request for creating or updating SalesOrderComponent information based on the specified SalesOrderComponent.
	/// </summary>
	/// <param name="salesOrderComponent">The SalesOrderComponent details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutSalesOrderComponent(ERPSalesOrderComponentDto salesOrderComponent);

	/// <summary>
	/// Processes the request to retrieve all SalesOrderComponents with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of SalesOrderComponents to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of SalesOrderComponents DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPSalesOrderComponentDto>>> Process_GetAllSalesOrderComponents(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific SalesOrderComponent.
	/// </summary>
	/// <param name="salesOrderComponentId">The Unique Id of the SalesOrderComponent to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the SalesOrderComponent DTO.</returns>
	Task<ERPResponseMessageDto<ERPSalesOrderComponentDto>> Process_GetSalesOrderComponent(Guid salesOrderComponentId);

	/// <summary>
	/// Processes the creating or updating of a SalesOrderComponent record.
	/// </summary>
	/// <param name="salesOrderComponent">The SalesOrderComponent data transfer object (DTO) containing the details of the SalesOrderComponent to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the SalesOrderComponent details.</returns>
	Task<ERPResponseMessageDto<ERPSalesOrderComponentDto>> Process_PutSalesOrderComponent(ERPSalesOrderComponentDto salesOrderComponent);

	/// <summary>
	/// Validates the request for deleting a SalesOrderComponent record.
	/// </summary>
	/// <param name="salesOrderComponentId">The Unique Id of the SalesOrderComponent.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteSalesOrderComponent(Guid salesOrderComponentId);

	/// <summary>
	/// Processes the request to delete a SalesOrderComponent record.
	/// </summary>
	/// <param name="salesOrderComponentId">The Unique Id of the SalesOrderComponent.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPSalesOrderComponentDto>> Process_DeleteSalesOrderComponent(Guid salesOrderComponentId);
}
