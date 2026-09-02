using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPAttachmentTypeModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all AttachmentTypes with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of AttachmentTypes to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllAttachmentTypes(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving AttachmentType information based on the specified AttachmentType Unique Id.
	/// </summary>
	/// <param name="attachmentTypeId">The Unique Id of the AttachmentType.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAttachmentType(Guid attachmentTypeId);

	/// <summary>
	/// Validates the PUT request for creating or updating AttachmentType information based on the specified AttachmentType.
	/// </summary>
	/// <param name="attachmentType">The AttachmentType details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutAttachmentType(ERPAttachmentTypeDto attachmentType);

	/// <summary>
	/// Processes the request to retrieve all AttachmentTypes with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of AttachmentTypes to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of AttachmentTypes DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPAttachmentTypeDto>>> Process_GetAllAttachmentTypes(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific AttachmentType.
	/// </summary>
	/// <param name="attachmentTypeId">The Unique Id of the AttachmentType to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the AttachmentType DTO.</returns>
	Task<ERPResponseMessageDto<ERPAttachmentTypeDto>> Process_GetAttachmentType(Guid attachmentTypeId);

	/// <summary>
	/// Processes the creating or updating of a AttachmentType record.
	/// </summary>
	/// <param name="attachmentType">The AttachmentType data transfer object (DTO) containing the details of the AttachmentType to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the AttachmentType details.</returns>
	Task<ERPResponseMessageDto<ERPAttachmentTypeDto>> Process_PutAttachmentType(ERPAttachmentTypeDto attachmentType);

	/// <summary>
	/// Validates the request for deleting a AttachmentType record.
	/// </summary>
	/// <param name="attachmentTypeId">The Unique Id of the AttachmentType.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteAttachmentType(Guid attachmentTypeId);

	/// <summary>
	/// Processes the request to delete a AttachmentType record.
	/// </summary>
	/// <param name="attachmentTypeId">The Unique Id of the AttachmentType.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPAttachmentTypeDto>> Process_DeleteAttachmentType(Guid attachmentTypeId);
}
