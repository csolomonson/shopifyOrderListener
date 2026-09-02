using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPAttachmentMemoModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all AttachmentMemos with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of AttachmentMemos to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllAttachmentMemos(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving AttachmentMemo information based on the specified AttachmentMemo Unique Id.
	/// </summary>
	/// <param name="attachmentMemoId">The Unique Id of the AttachmentMemo.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAttachmentMemo(Guid attachmentMemoId);

	/// <summary>
	/// Validates the PUT request for creating or updating AttachmentMemo information based on the specified AttachmentMemo.
	/// </summary>
	/// <param name="attachmentMemo">The AttachmentMemo details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutAttachmentMemo(ERPAttachmentMemoDto attachmentMemo);

	/// <summary>
	/// Processes the request to retrieve all AttachmentMemos with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of AttachmentMemos to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of AttachmentMemos DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPAttachmentMemoDto>>> Process_GetAllAttachmentMemos(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific AttachmentMemo.
	/// </summary>
	/// <param name="attachmentMemoId">The Unique Id of the AttachmentMemo to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the AttachmentMemo DTO.</returns>
	Task<ERPResponseMessageDto<ERPAttachmentMemoDto>> Process_GetAttachmentMemo(Guid attachmentMemoId);

	/// <summary>
	/// Processes the creating or updating of a AttachmentMemo record.
	/// </summary>
	/// <param name="attachmentMemo">The AttachmentMemo data transfer object (DTO) containing the details of the AttachmentMemo to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the AttachmentMemo details.</returns>
	Task<ERPResponseMessageDto<ERPAttachmentMemoDto>> Process_PutAttachmentMemo(ERPAttachmentMemoDto attachmentMemo);

	/// <summary>
	/// Validates the request for deleting a AttachmentMemo record.
	/// </summary>
	/// <param name="attachmentMemoId">The Unique Id of the AttachmentMemo.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteAttachmentMemo(Guid attachmentMemoId);

	/// <summary>
	/// Processes the request to delete a AttachmentMemo record.
	/// </summary>
	/// <param name="attachmentMemoId">The Unique Id of the AttachmentMemo.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPAttachmentMemoDto>> Process_DeleteAttachmentMemo(Guid attachmentMemoId);
}
