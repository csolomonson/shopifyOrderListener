using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using M1.API.DTOs.BOM.Job;
using M1.API.DTOs.Core;
using M1.API.DTOs.Custom;

namespace M1.API.Repositories.Core.Job;

public interface IJobRepository : IAPIBaseRepository, IDisposable
{
	Task<bool> DoesJobAssemblyExists(string jobId, int jobAssemblyId);

	Task<bool> DoesJobExists(string jobId);

	Task<bool> DoesJobOperationExists(string jobId, int jobAssemblyId, int jobOperationId);

	Task<BOMJobGuidsDto> GetJobGuidsInfo(string jobId, string partId);

	Task<string> GetJobIdFromGuid(string jobIdString);

	Task<BOMJobDto> GetJobHeaderInfo(string jobId);

	Task<DataTable> GetJobMethodAsDataTable(string jobId);

	/// <summary>
	/// Retrieves all Job with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Jobs to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a BOM response message DTO with a list of Jobs DTOs.</returns>
	Task<ICollection<BOMJobDto>> GetAllJobs(int? pageSize = null, int? pageNumber = null);

	/// <summary>
	/// Retrieves detailed information about a specific Job.
	/// </summary>
	/// <param name="jobId">The ID of the Job to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the BOM Job DTO.</returns>
	Task<BOMJobDto> GetJob(string jobId);

	/// <summary>
	/// Saves the provided CTM job.
	/// </summary>
	/// <param name="job">The CTM job to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveJob(CTMJobDto job);
}
