using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPLeadMemoModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all LeadMemos with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of LeadMemos to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllLeadMemos(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving LeadMemo information based on the specified LeadMemo Unique Id.
	/// </summary>
	/// <param name="leadMemoId">The Unique Id of the LeadMemo.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetLeadMemo(Guid leadMemoId);

	/// <summary>
	/// Validates the PUT request for creating or updating LeadMemo information based on the specified LeadMemo.
	/// </summary>
	/// <param name="leadMemo">The LeadMemo details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutLeadMemo(ERPLeadMemoDto leadMemo);

	/// <summary>
	/// Processes the request to retrieve all LeadMemos with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of LeadMemos to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of LeadMemos DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPLeadMemoDto>>> Process_GetAllLeadMemos(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific LeadMemo.
	/// </summary>
	/// <param name="leadMemoId">The Unique Id of the LeadMemo to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the LeadMemo DTO.</returns>
	Task<ERPResponseMessageDto<ERPLeadMemoDto>> Process_GetLeadMemo(Guid leadMemoId);

	/// <summary>
	/// Processes the creating or updating of a LeadMemo record.
	/// </summary>
	/// <param name="leadMemo">The LeadMemo data transfer object (DTO) containing the details of the LeadMemo to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the LeadMemo details.</returns>
	Task<ERPResponseMessageDto<ERPLeadMemoDto>> Process_PutLeadMemo(ERPLeadMemoDto leadMemo);

	/// <summary>
	/// Validates the request for deleting a LeadMemo record.
	/// </summary>
	/// <param name="leadMemoId">The Unique Id of the LeadMemo.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteLeadMemo(Guid leadMemoId);

	/// <summary>
	/// Processes the request to delete a LeadMemo record.
	/// </summary>
	/// <param name="leadMemoId">The Unique Id of the LeadMemo.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPLeadMemoDto>> Process_DeleteLeadMemo(Guid leadMemoId);
}
