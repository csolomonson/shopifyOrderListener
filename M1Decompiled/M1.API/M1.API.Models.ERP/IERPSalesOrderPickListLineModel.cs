using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPSalesOrderPickListLineModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all SalesOrderPickListLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of SalesOrderPickListLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllSalesOrderPickListLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving SalesOrderPickListLine information based on the specified SalesOrderPickListLine Unique Id.
	/// </summary>
	/// <param name="salesOrderPickListLineId">The Unique Id of the SalesOrderPickListLine.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetSalesOrderPickListLine(Guid salesOrderPickListLineId);

	/// <summary>
	/// Validates the PUT request for creating or updating SalesOrderPickListLine information based on the specified SalesOrderPickListLine.
	/// </summary>
	/// <param name="salesOrderPickListLine">The SalesOrderPickListLine details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutSalesOrderPickListLine(ERPSalesOrderPickListLineDto salesOrderPickListLine);

	/// <summary>
	/// Processes the request to retrieve all SalesOrderPickListLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of SalesOrderPickListLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of SalesOrderPickListLines DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPSalesOrderPickListLineDto>>> Process_GetAllSalesOrderPickListLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific SalesOrderPickListLine.
	/// </summary>
	/// <param name="salesOrderPickListLineId">The Unique Id of the SalesOrderPickListLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the SalesOrderPickListLine DTO.</returns>
	Task<ERPResponseMessageDto<ERPSalesOrderPickListLineDto>> Process_GetSalesOrderPickListLine(Guid salesOrderPickListLineId);

	/// <summary>
	/// Processes the creating or updating of a SalesOrderPickListLine record.
	/// </summary>
	/// <param name="salesOrderPickListLine">The SalesOrderPickListLine data transfer object (DTO) containing the details of the SalesOrderPickListLine to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the SalesOrderPickListLine details.</returns>
	Task<ERPResponseMessageDto<ERPSalesOrderPickListLineDto>> Process_PutSalesOrderPickListLine(ERPSalesOrderPickListLineDto salesOrderPickListLine);

	/// <summary>
	/// Validates the request for deleting a SalesOrderPickListLine record.
	/// </summary>
	/// <param name="salesOrderPickListLineId">The Unique Id of the SalesOrderPickListLine.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteSalesOrderPickListLine(Guid salesOrderPickListLineId);

	/// <summary>
	/// Processes the request to delete a SalesOrderPickListLine record.
	/// </summary>
	/// <param name="salesOrderPickListLineId">The Unique Id of the SalesOrderPickListLine.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPSalesOrderPickListLineDto>> Process_DeleteSalesOrderPickListLine(Guid salesOrderPickListLineId);
}
