using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.BOM.Job;
using M1.API.DTOs.Core;

namespace M1.API.Repositories.Core.Job;

public interface IJobOperationRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a JobOperation with the specified ID exists.
	/// </summary>
	/// <param name="jobOperationId">The ID of the JobOperation to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the JobOperation exists or not.</returns>
	Task<bool> DoesJobOperationExists(string jobOperationId);

	/// <summary>
	/// Retrieves all JobOperation with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of JobOperations to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a BOM response message DTO with a list of JobOperations DTOs.</returns>
	Task<ICollection<BOMJobOperationDto>> GetAllJobOperations(int? pageSize = null, int? pageNumber = null);

	/// <summary>
	/// Retrieves information about a specific job operation.
	/// </summary>
	/// <param name="jobId">The identifier of the job.</param>
	/// <param name="jobAssemblyId">The identifier of the job assembly.</param>
	/// <param name="jobOperationId">The identifier of the job operation.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.BOM.Job.BOMJobOperationDto" /> object with details of the job operation.</returns>
	Task<BOMJobOperationDto> GetJobOperationInfo(string jobId, int jobAssemblyId, int jobOperationId);

	/// <summary>
	/// Saves the provided BOM jobOperation.
	/// </summary>
	/// <param name="jobOperation">The BOM jobOperation to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveJobOperationAsync(BOMJobOperationDto jobOperation);

	/// <summary>
	/// Deletes a specific job operation.
	/// </summary>
	/// <param name="jobId">The ID of the job.</param>
	/// <param name="jobAssemblyId">The ID of the job assembly.</param>
	/// <param name="jobOperationId">The ID of the job operation to be deleted.</param>
	/// <returns>
	/// A task that represents the asynchronous operation. The task result contains the API validation information.
	/// </returns>
	Task<APIValidationInfoDto> DeleteJobOperation(string jobId, int jobAssemblyId, int jobOperationId);
}
