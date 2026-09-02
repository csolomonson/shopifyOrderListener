using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPJobModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all Jobs with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Jobs to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllJobs(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving Job information based on the specified Job Unique Id.
	/// </summary>
	/// <param name="jobId">The Unique Id of the Job.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetJob(Guid jobId);

	/// <summary>
	/// Validates the PUT request for creating or updating Job information based on the specified Job.
	/// </summary>
	/// <param name="job">The Job details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutJob(ERPJobDto job);

	/// <summary>
	/// Processes the request to retrieve all Jobs with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Jobs to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of Jobs DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPJobDto>>> Process_GetAllJobs(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific Job.
	/// </summary>
	/// <param name="jobId">The Unique Id of the Job to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the Job DTO.</returns>
	Task<ERPResponseMessageDto<ERPJobDto>> Process_GetJob(Guid jobId);

	/// <summary>
	/// Processes the creating or updating of a Job record.
	/// </summary>
	/// <param name="job">The Job data transfer object (DTO) containing the details of the Job to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the Job details.</returns>
	Task<ERPResponseMessageDto<ERPJobDto>> Process_PutJob(ERPJobDto job);

	/// <summary>
	/// Validates the request for deleting a Job record.
	/// </summary>
	/// <param name="jobId">The Unique Id of the Job.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteJob(Guid jobId);

	/// <summary>
	/// Processes the request to delete a Job record.
	/// </summary>
	/// <param name="jobId">The Unique Id of the Job.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPJobDto>> Process_DeleteJob(Guid jobId);
}
