using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPQuoteMemoModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all QuoteMemos with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of QuoteMemos to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllQuoteMemos(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving QuoteMemo information based on the specified QuoteMemo Unique Id.
	/// </summary>
	/// <param name="quoteMemoId">The Unique Id of the QuoteMemo.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetQuoteMemo(Guid quoteMemoId);

	/// <summary>
	/// Validates the PUT request for creating or updating QuoteMemo information based on the specified QuoteMemo.
	/// </summary>
	/// <param name="quoteMemo">The QuoteMemo details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutQuoteMemo(ERPQuoteMemoDto quoteMemo);

	/// <summary>
	/// Processes the request to retrieve all QuoteMemos with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of QuoteMemos to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of QuoteMemos DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPQuoteMemoDto>>> Process_GetAllQuoteMemos(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific QuoteMemo.
	/// </summary>
	/// <param name="quoteMemoId">The Unique Id of the QuoteMemo to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the QuoteMemo DTO.</returns>
	Task<ERPResponseMessageDto<ERPQuoteMemoDto>> Process_GetQuoteMemo(Guid quoteMemoId);

	/// <summary>
	/// Processes the creating or updating of a QuoteMemo record.
	/// </summary>
	/// <param name="quoteMemo">The QuoteMemo data transfer object (DTO) containing the details of the QuoteMemo to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the QuoteMemo details.</returns>
	Task<ERPResponseMessageDto<ERPQuoteMemoDto>> Process_PutQuoteMemo(ERPQuoteMemoDto quoteMemo);

	/// <summary>
	/// Validates the request for deleting a QuoteMemo record.
	/// </summary>
	/// <param name="quoteMemoId">The Unique Id of the QuoteMemo.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteQuoteMemo(Guid quoteMemoId);

	/// <summary>
	/// Processes the request to delete a QuoteMemo record.
	/// </summary>
	/// <param name="quoteMemoId">The Unique Id of the QuoteMemo.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPQuoteMemoDto>> Process_DeleteQuoteMemo(Guid quoteMemoId);
}
