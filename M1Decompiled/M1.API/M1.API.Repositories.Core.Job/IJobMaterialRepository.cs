using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.BOM.Job;
using M1.API.DTOs.Core;

namespace M1.API.Repositories.Core.Job;

public interface IJobMaterialRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a JobMaterial with the specified ID exists.
	/// </summary>
	/// <param name="jobId">The ID of the Job to check.</param>
	/// <param name="jobAssemblyId">The ID of the JobAssembly to check.</param>
	/// <param name="jobMaterialId">The ID of the JobMaterial to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the JobMaterial exists or not.</returns>
	Task<bool> DoesJobMaterialExists(string jobId, int jobAssemblyId, int jobMaterialId);

	/// <summary>
	/// Retrieves all JobMaterial with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of JobMaterials to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a BOM response message DTO with a list of JobMaterials DTOs.</returns>
	Task<ICollection<BOMJobMaterialDto>> GetAllJobMaterials(int? pageSize = null, int? pageNumber = null);

	/// <summary>
	/// Retrieves the information of a specific job material.
	/// </summary>
	/// <param name="jobId">The ID of the job.</param>
	/// <param name="jobAssemblyId">The ID of the job assembly.</param>
	/// <param name="jobMaterialId">The ID of the job material whose information is to be retrieved.</param>
	/// <returns>
	/// A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.BOM.Job.BOMJobMaterialDto" /> 
	/// which includes the information about the specified job material.
	/// </returns>
	Task<BOMJobMaterialDto> GetJobMaterialInfo(string jobId, int jobAssemblyId, int jobMaterialId);

	/// <summary>
	/// Saves the provided BOM jobMaterial.
	/// </summary>
	/// <param name="jobMaterial">The BOM jobMaterial to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveJobMaterial(BOMJobMaterialDto jobMaterial);

	/// <summary>
	/// Deletes a specific job material.
	/// </summary>
	/// <param name="jobId">The ID of the job.</param>
	/// <param name="jobAssemblyId">The ID of the job assembly.</param>
	/// <param name="jobMaterialId">The ID of the job material to be deleted.</param>
	/// <returns>
	/// A task that represents the asynchronous operation. The task result contains the API validation information.
	/// </returns>
	Task<APIValidationInfoDto> DeleteJobMaterial(string jobId, int jobAssemblyId, int jobMaterialId);
}
