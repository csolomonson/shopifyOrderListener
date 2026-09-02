using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPAttachmentModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all Attachments with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Attachments to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllAttachments(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving Attachment information based on the specified Attachment Unique Id.
	/// </summary>
	/// <param name="attachmentId">The Unique Id of the Attachment.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAttachment(Guid attachmentId);

	/// <summary>
	/// Validates the PUT request for creating or updating Attachment information based on the specified Attachment.
	/// </summary>
	/// <param name="attachment">The Attachment details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutAttachment(ERPAttachmentDto attachment);

	/// <summary>
	/// Processes the request to retrieve all Attachments with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Attachments to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of Attachments DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPAttachmentDto>>> Process_GetAllAttachments(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific Attachment.
	/// </summary>
	/// <param name="attachmentId">The Unique Id of the Attachment to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the Attachment DTO.</returns>
	Task<ERPResponseMessageDto<ERPAttachmentDto>> Process_GetAttachment(Guid attachmentId);

	/// <summary>
	/// Processes the creating or updating of a Attachment record.
	/// </summary>
	/// <param name="attachment">The Attachment data transfer object (DTO) containing the details of the Attachment to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the Attachment details.</returns>
	Task<ERPResponseMessageDto<ERPAttachmentDto>> Process_PutAttachment(ERPAttachmentDto attachment);

	/// <summary>
	/// Validates the request for deleting a Attachment record.
	/// </summary>
	/// <param name="attachmentId">The Unique Id of the Attachment.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteAttachment(Guid attachmentId);

	/// <summary>
	/// Processes the request to delete a Attachment record.
	/// </summary>
	/// <param name="attachmentId">The Unique Id of the Attachment.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPAttachmentDto>> Process_DeleteAttachment(Guid attachmentId);
}
