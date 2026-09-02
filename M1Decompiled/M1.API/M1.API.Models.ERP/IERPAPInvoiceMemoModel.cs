using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPAPInvoiceMemoModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all APInvoiceMemos with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of APInvoiceMemos to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllAPInvoiceMemos(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving APInvoiceMemo information based on the specified APInvoiceMemo Unique Id.
	/// </summary>
	/// <param name="aPInvoiceMemoId">The Unique Id of the APInvoiceMemo.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAPInvoiceMemo(Guid aPInvoiceMemoId);

	/// <summary>
	/// Validates the PUT request for creating or updating APInvoiceMemo information based on the specified APInvoiceMemo.
	/// </summary>
	/// <param name="aPInvoiceMemo">The APInvoiceMemo details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutAPInvoiceMemo(ERPAPInvoiceMemoDto aPInvoiceMemo);

	/// <summary>
	/// Processes the request to retrieve all APInvoiceMemos with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of APInvoiceMemos to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of APInvoiceMemos DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPAPInvoiceMemoDto>>> Process_GetAllAPInvoiceMemos(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific APInvoiceMemo.
	/// </summary>
	/// <param name="aPInvoiceMemoId">The Unique Id of the APInvoiceMemo to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the APInvoiceMemo DTO.</returns>
	Task<ERPResponseMessageDto<ERPAPInvoiceMemoDto>> Process_GetAPInvoiceMemo(Guid aPInvoiceMemoId);

	/// <summary>
	/// Processes the creating or updating of a APInvoiceMemo record.
	/// </summary>
	/// <param name="aPInvoiceMemo">The APInvoiceMemo data transfer object (DTO) containing the details of the APInvoiceMemo to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the APInvoiceMemo details.</returns>
	Task<ERPResponseMessageDto<ERPAPInvoiceMemoDto>> Process_PutAPInvoiceMemo(ERPAPInvoiceMemoDto aPInvoiceMemo);

	/// <summary>
	/// Validates the request for deleting a APInvoiceMemo record.
	/// </summary>
	/// <param name="aPInvoiceMemoId">The Unique Id of the APInvoiceMemo.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteAPInvoiceMemo(Guid aPInvoiceMemoId);

	/// <summary>
	/// Processes the request to delete a APInvoiceMemo record.
	/// </summary>
	/// <param name="aPInvoiceMemoId">The Unique Id of the APInvoiceMemo.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPAPInvoiceMemoDto>> Process_DeleteAPInvoiceMemo(Guid aPInvoiceMemoId);
}
