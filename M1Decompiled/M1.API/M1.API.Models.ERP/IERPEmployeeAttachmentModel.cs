using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPEmployeeAttachmentModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all EmployeeAttachments with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of EmployeeAttachments to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllEmployeeAttachments(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving EmployeeAttachment information based on the specified EmployeeAttachment Unique Id.
	/// </summary>
	/// <param name="employeeAttachmentId">The Unique Id of the EmployeeAttachment.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetEmployeeAttachment(Guid employeeAttachmentId);

	/// <summary>
	/// Validates the PUT request for creating or updating EmployeeAttachment information based on the specified EmployeeAttachment.
	/// </summary>
	/// <param name="employeeAttachment">The EmployeeAttachment details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutEmployeeAttachment(ERPEmployeeAttachmentDto employeeAttachment);

	/// <summary>
	/// Processes the request to retrieve all EmployeeAttachments with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of EmployeeAttachments to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of EmployeeAttachments DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPEmployeeAttachmentDto>>> Process_GetAllEmployeeAttachments(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific EmployeeAttachment.
	/// </summary>
	/// <param name="employeeAttachmentId">The Unique Id of the EmployeeAttachment to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the EmployeeAttachment DTO.</returns>
	Task<ERPResponseMessageDto<ERPEmployeeAttachmentDto>> Process_GetEmployeeAttachment(Guid employeeAttachmentId);

	/// <summary>
	/// Processes the creating or updating of a EmployeeAttachment record.
	/// </summary>
	/// <param name="employeeAttachment">The EmployeeAttachment data transfer object (DTO) containing the details of the EmployeeAttachment to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the EmployeeAttachment details.</returns>
	Task<ERPResponseMessageDto<ERPEmployeeAttachmentDto>> Process_PutEmployeeAttachment(ERPEmployeeAttachmentDto employeeAttachment);

	/// <summary>
	/// Validates the request for deleting a EmployeeAttachment record.
	/// </summary>
	/// <param name="employeeAttachmentId">The Unique Id of the EmployeeAttachment.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteEmployeeAttachment(Guid employeeAttachmentId);

	/// <summary>
	/// Processes the request to delete a EmployeeAttachment record.
	/// </summary>
	/// <param name="employeeAttachmentId">The Unique Id of the EmployeeAttachment.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPEmployeeAttachmentDto>> Process_DeleteEmployeeAttachment(Guid employeeAttachmentId);
}
