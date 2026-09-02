using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPARInvoiceMemoModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all ARInvoiceMemos with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ARInvoiceMemos to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllARInvoiceMemos(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving ARInvoiceMemo information based on the specified ARInvoiceMemo Unique Id.
	/// </summary>
	/// <param name="aRInvoiceMemoId">The Unique Id of the ARInvoiceMemo.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetARInvoiceMemo(Guid aRInvoiceMemoId);

	/// <summary>
	/// Validates the PUT request for creating or updating ARInvoiceMemo information based on the specified ARInvoiceMemo.
	/// </summary>
	/// <param name="aRInvoiceMemo">The ARInvoiceMemo details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutARInvoiceMemo(ERPARInvoiceMemoDto aRInvoiceMemo);

	/// <summary>
	/// Processes the request to retrieve all ARInvoiceMemos with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ARInvoiceMemos to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ARInvoiceMemos DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPARInvoiceMemoDto>>> Process_GetAllARInvoiceMemos(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific ARInvoiceMemo.
	/// </summary>
	/// <param name="aRInvoiceMemoId">The Unique Id of the ARInvoiceMemo to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the ARInvoiceMemo DTO.</returns>
	Task<ERPResponseMessageDto<ERPARInvoiceMemoDto>> Process_GetARInvoiceMemo(Guid aRInvoiceMemoId);

	/// <summary>
	/// Processes the creating or updating of a ARInvoiceMemo record.
	/// </summary>
	/// <param name="aRInvoiceMemo">The ARInvoiceMemo data transfer object (DTO) containing the details of the ARInvoiceMemo to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the ARInvoiceMemo details.</returns>
	Task<ERPResponseMessageDto<ERPARInvoiceMemoDto>> Process_PutARInvoiceMemo(ERPARInvoiceMemoDto aRInvoiceMemo);

	/// <summary>
	/// Validates the request for deleting a ARInvoiceMemo record.
	/// </summary>
	/// <param name="aRInvoiceMemoId">The Unique Id of the ARInvoiceMemo.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteARInvoiceMemo(Guid aRInvoiceMemoId);

	/// <summary>
	/// Processes the request to delete a ARInvoiceMemo record.
	/// </summary>
	/// <param name="aRInvoiceMemoId">The Unique Id of the ARInvoiceMemo.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPARInvoiceMemoDto>> Process_DeleteARInvoiceMemo(Guid aRInvoiceMemoId);
}
