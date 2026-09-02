using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPJobPriorityModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all JobPriorities with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of JobPriorities to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllJobPriorities(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving JobPriority information based on the specified JobPriority Unique Id.
	/// </summary>
	/// <param name="jobPriorityId">The Unique Id of the JobPriority.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetJobPriority(Guid jobPriorityId);

	/// <summary>
	/// Validates the PUT request for creating or updating JobPriority information based on the specified JobPriority.
	/// </summary>
	/// <param name="jobPriority">The JobPriority details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutJobPriority(ERPJobPriorityDto jobPriority);

	/// <summary>
	/// Processes the request to retrieve all JobPriorities with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of JobPriorities to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of JobPriorities DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPJobPriorityDto>>> Process_GetAllJobPriorities(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific JobPriority.
	/// </summary>
	/// <param name="jobPriorityId">The Unique Id of the JobPriority to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the JobPriority DTO.</returns>
	Task<ERPResponseMessageDto<ERPJobPriorityDto>> Process_GetJobPriority(Guid jobPriorityId);

	/// <summary>
	/// Processes the creating or updating of a JobPriority record.
	/// </summary>
	/// <param name="jobPriority">The JobPriority data transfer object (DTO) containing the details of the JobPriority to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the JobPriority details.</returns>
	Task<ERPResponseMessageDto<ERPJobPriorityDto>> Process_PutJobPriority(ERPJobPriorityDto jobPriority);

	/// <summary>
	/// Validates the request for deleting a JobPriority record.
	/// </summary>
	/// <param name="jobPriorityId">The Unique Id of the JobPriority.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteJobPriority(Guid jobPriorityId);

	/// <summary>
	/// Processes the request to delete a JobPriority record.
	/// </summary>
	/// <param name="jobPriorityId">The Unique Id of the JobPriority.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPJobPriorityDto>> Process_DeleteJobPriority(Guid jobPriorityId);
}
