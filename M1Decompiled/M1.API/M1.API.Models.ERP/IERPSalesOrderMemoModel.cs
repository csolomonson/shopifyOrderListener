using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPSalesOrderMemoModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all SalesOrderMemos with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of SalesOrderMemos to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllSalesOrderMemos(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving SalesOrderMemo information based on the specified SalesOrderMemo Unique Id.
	/// </summary>
	/// <param name="salesOrderMemoId">The Unique Id of the SalesOrderMemo.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetSalesOrderMemo(Guid salesOrderMemoId);

	/// <summary>
	/// Validates the PUT request for creating or updating SalesOrderMemo information based on the specified SalesOrderMemo.
	/// </summary>
	/// <param name="salesOrderMemo">The SalesOrderMemo details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutSalesOrderMemo(ERPSalesOrderMemoDto salesOrderMemo);

	/// <summary>
	/// Processes the request to retrieve all SalesOrderMemos with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of SalesOrderMemos to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of SalesOrderMemos DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPSalesOrderMemoDto>>> Process_GetAllSalesOrderMemos(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific SalesOrderMemo.
	/// </summary>
	/// <param name="salesOrderMemoId">The Unique Id of the SalesOrderMemo to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the SalesOrderMemo DTO.</returns>
	Task<ERPResponseMessageDto<ERPSalesOrderMemoDto>> Process_GetSalesOrderMemo(Guid salesOrderMemoId);

	/// <summary>
	/// Processes the creating or updating of a SalesOrderMemo record.
	/// </summary>
	/// <param name="salesOrderMemo">The SalesOrderMemo data transfer object (DTO) containing the details of the SalesOrderMemo to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the SalesOrderMemo details.</returns>
	Task<ERPResponseMessageDto<ERPSalesOrderMemoDto>> Process_PutSalesOrderMemo(ERPSalesOrderMemoDto salesOrderMemo);

	/// <summary>
	/// Validates the request for deleting a SalesOrderMemo record.
	/// </summary>
	/// <param name="salesOrderMemoId">The Unique Id of the SalesOrderMemo.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteSalesOrderMemo(Guid salesOrderMemoId);

	/// <summary>
	/// Processes the request to delete a SalesOrderMemo record.
	/// </summary>
	/// <param name="salesOrderMemoId">The Unique Id of the SalesOrderMemo.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPSalesOrderMemoDto>> Process_DeleteSalesOrderMemo(Guid salesOrderMemoId);
}
