using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.DTOs.BOM.Job;
using M1.API.DTOs.Core;

namespace M1.API.Models.BOM.Job;

public interface IBOMJobOperationModel : IBOMBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving information about a specific job operation.
	/// </summary>
	/// <param name="jobId">The identifier of the job to be validated.</param>
	/// <param name="jobAssemblyId">The identifier of the job assembly to be validated.</param>
	/// <param name="jobOperationId">The identifier of the job operation to be validated.</param>
	/// <returns>
	/// A task that represents the asynchronous validation operation. The task result contains an 
	/// <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> object with details of the validation result.
	/// </returns>
	Task<APIValidationInfoDto> ValidateRequest_GetJobOperation(string jobId, int jobAssemblyId, int jobOperationId);

	/// <summary>
	/// Validates the POST request for retrieving JobOperation information based on the specified JobOperation.
	/// </summary>
	/// <param name="jobOperation">The job operation details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PostJobOperation(BOMJobOperationDto jobOperation);

	/// <summary>
	/// Validates the request for deleting a specific job operation.
	/// </summary>
	/// <param name="jobId">The identifier of the job to be validated.</param>
	/// <param name="jobAssemblyId">The identifier of the job assembly to be validated.</param>
	/// <param name="jobOperationId">The identifier of the job operation to be validated.</param>
	/// <returns>
	/// A task that represents the asynchronous validation operation. The task result contains an 
	/// <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> object with details of the validation result.
	/// </returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteJobOperation(string jobId, int jobAssemblyId, int jobOperationId);

	/// <summary>
	/// Processes the request to retrieve all JobOperations with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of JobOperations to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a BOM response message DTO with a list of JobOperations DTOs.</returns>
	Task<BOMResponseMessageDto<IList<BOMJobOperationDto>>> Process_GetAllJobOperations(int pageSize, int pageNumber);

	/// <summary>
	/// Processes and retrieves a job operation.
	/// </summary>
	/// <param name="jobId">The ID of the job.</param>
	/// <param name="jobAssemblyId">The ID of the job assembly.</param>
	/// <param name="jobOperationId">The ID of the job operation.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.BOM.BOMResponseMessageDto`1" /> object.</returns>
	Task<BOMResponseMessageDto<BOMJobOperationDto>> Process_GetJobOperation(string jobId, int jobAssemblyId, int jobOperationId);

	/// <summary>
	/// Processes the posting of JobOperation.
	/// </summary>
	/// <param name="jobOperation">The JobOperation data transfer object (DTO) containing the details of the jobOperation to be posted.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.BOM.BOMResponseMessageDto`1" /> with the response message and the jobOperation details.</returns>
	Task<BOMResponseMessageDto<BOMJobOperationDto>> Process_PostJobOperation(BOMJobOperationDto jobOperation);

	/// <summary>
	/// Processes the deletion of a specific job operation.
	/// </summary>
	/// <param name="jobId">The identifier of the job to be deleted.</param>
	/// <param name="jobAssemblyId">The identifier of the job assembly to be deleted.</param>
	/// <param name="jobOperationId">The identifier of the job operation to be deleted.</param>
	/// <returns>
	/// A task that represents the asynchronous deletion operation. The task result contains a 
	/// <see cref="T:M1.API.DTOs.BOM.BOMResponseMessageDto`1" /> object with details of the job operation deletion response.
	/// </returns>
	Task<BOMResponseMessageDto<BOMJobOperationDto>> Process_DeleteJobOperation(string jobId, int jobAssemblyId, int jobOperationId);
}
