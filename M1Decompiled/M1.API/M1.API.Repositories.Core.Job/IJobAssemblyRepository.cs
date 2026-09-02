using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.BOM.Job;
using M1.API.DTOs.Core;

namespace M1.API.Repositories.Core.Job;

public interface IJobAssemblyRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Retrieves information about all job assemblies for a given job ID.
	/// </summary>
	/// <param name="jobId">The unique identifier for the job.</param>
	/// <returns>A task that represents the asynchronous operation. 
	/// The task result contains a list of <see cref="T:M1.API.DTOs.BOM.Job.BOMJobAssemblyDto" /> objects representing the job assemblies information.</returns>
	Task<IList<BOMJobAssemblyDto>> GetJobAssembliesInfo(string jobId);

	/// <summary>
	/// Retrieves information about a specific job assembly for a given job ID and job assembly ID.
	/// </summary>
	/// <param name="jobId">The unique identifier for the job.</param>
	/// <param name="jobAssemblyId">The unique identifier for the job assembly.</param>
	/// <returns>A task that represents the asynchronous operation. 
	/// The task result contains a <see cref="T:M1.API.DTOs.BOM.Job.BOMJobAssemblyDto" /> object representing the job assembly information.</returns>
	Task<BOMJobAssemblyDto> GetJobAssemblyInfo(string jobId, int jobAssemblyId);

	/// <summary>
	/// Checks if a JobAssembly with the specified ID exists.
	/// </summary>
	/// <param name="jobAssemblyId">The ID of the JobAssembly to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the JobAssembly exists or not.</returns>
	Task<bool> DoesJobAssemblyExists(string jobAssemblyId);

	/// <summary>
	/// Retrieves all JobAssembly with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of JobAssemblies to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a BOM response message DTO with a list of JobAssemblies DTOs.</returns>
	Task<ICollection<BOMJobAssemblyDto>> GetAllJobAssemblies(int? pageSize = null, int? pageNumber = null);

	/// <summary>
	/// Retrieves detailed information about a specific JobAssembly.
	/// </summary>
	/// <param name="jobAssemblyId">The ID of the JobAssembly to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the JobAssembly DTO.</returns>
	Task<BOMJobAssemblyDto> GetJobAssembly(string jobAssemblyId);

	Task<APIValidationInfoDto> SaveJobAssembly(BOMJobAssemblyDto jobAssembly);
}
