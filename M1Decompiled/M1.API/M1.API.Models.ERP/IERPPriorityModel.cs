using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPPriorityModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all Priorities with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Priorities to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllPriorities(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving Priority information based on the specified Priority Unique Id.
	/// </summary>
	/// <param name="priorityId">The Unique Id of the Priority.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetPriority(Guid priorityId);

	/// <summary>
	/// Validates the PUT request for creating or updating Priority information based on the specified Priority.
	/// </summary>
	/// <param name="priority">The Priority details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutPriority(ERPPriorityDto priority);

	/// <summary>
	/// Processes the request to retrieve all Priorities with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Priorities to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of Priorities DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPPriorityDto>>> Process_GetAllPriorities(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific Priority.
	/// </summary>
	/// <param name="priorityId">The Unique Id of the Priority to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the Priority DTO.</returns>
	Task<ERPResponseMessageDto<ERPPriorityDto>> Process_GetPriority(Guid priorityId);

	/// <summary>
	/// Processes the creating or updating of a Priority record.
	/// </summary>
	/// <param name="priority">The Priority data transfer object (DTO) containing the details of the Priority to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the Priority details.</returns>
	Task<ERPResponseMessageDto<ERPPriorityDto>> Process_PutPriority(ERPPriorityDto priority);

	/// <summary>
	/// Validates the request for deleting a Priority record.
	/// </summary>
	/// <param name="priorityId">The Unique Id of the Priority.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeletePriority(Guid priorityId);

	/// <summary>
	/// Processes the request to delete a Priority record.
	/// </summary>
	/// <param name="priorityId">The Unique Id of the Priority.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPPriorityDto>> Process_DeletePriority(Guid priorityId);
}
