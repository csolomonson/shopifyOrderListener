using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.DTOs.BOM.Job;
using M1.API.DTOs.Core;
using M1.API.DTOs.Custom;

namespace M1.API.Models.BOM;

public interface IBOMJobAssemblyModel : IBOMBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving JobAssemblies information based on the specified Job ID.
	/// </summary>
	/// <param name="jobId">The ID of the Job.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetJobId(string jobId);

	Task<APIValidationInfoDto> ValidateRequest_GetJobAssembly(string jobId, int jobAssemblyId);

	/// <summary>
	/// Validates the POST request for retrieving JobAssembly information based on the specified JobAssembly.
	/// </summary>
	/// <param name="jobAssembly">The JobAssembly.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PostJobAssembly(BOMJobAssemblyDto jobAssembly);

	/// <summary>
	/// Validates the request for deleting a job assembly.
	/// </summary>
	/// <param name="jobId">The ID of the job.</param>
	/// <param name="jobAssemblyId">The ID of the job assembly to be deleted.</param>
	/// <returns>
	/// A task that represents the asynchronous operation. The task result contains the API validation information.
	/// </returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteJobAssembly(string jobId, int jobAssemblyId);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific JobAssembly.
	/// </summary>
	/// <param name="jobAssemblyId">The ID of the JobAssembly to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a CTM BOM response message DTO with the JobAssembly DTO.</returns>
	Task<BOMResponseMessageDto<CTMBOMJobAssemblyDto>> Process_GetJobAssembly(string jobAssemblyId);

	/// <summary>
	/// Processes the retrieval of a job assembly.
	/// </summary>
	/// <param name="jobId">The ID of the job.</param>
	/// <param name="jobAssemblyId">The ID of the job assembly to be retrieved.</param>
	/// <returns>
	/// A task that represents the asynchronous operation. The task result contains a 
	/// <see cref="T:M1.API.DTOs.BOM.BOMResponseMessageDto`1" /> which includes information about the retrieved job assembly.
	/// </returns>
	Task<BOMResponseMessageDto<BOMJobAssemblyDto>> Process_GetJobAssembly(string jobId, int jobAssemblyId);

	/// <summary>
	/// Processes the request to retrieve all JobAssemblies with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of JobAssemblies to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a BOM response message DTO with a list of JobAssemblies DTOs.</returns>
	Task<BOMResponseMessageDto<IList<BOMJobAssemblyDto>>> Process_GetAllJobAssemblies(int pageSize, int pageNumber);

	/// <summary>
	/// Processes the creation of a new job assembly.
	/// </summary>
	/// <param name="jobAssembly">The job assembly data to be created.</param>
	/// <returns>
	/// A task that represents the asynchronous operation. The task result contains a 
	/// <see cref="T:M1.API.DTOs.BOM.BOMResponseMessageDto`1" /> which includes information about the created job assembly.
	/// </returns>
	Task<BOMResponseMessageDto<BOMJobAssemblyDto>> Process_PostJobAssembly(BOMJobAssemblyDto jobAssembly);

	/// <summary>
	/// Processes the deletion of a job assembly.
	/// </summary>
	/// <param name="jobId">The ID of the job.</param>
	/// <param name="jobAssemblyId">The ID of the job assembly to be deleted.</param>
	/// <returns>
	/// A task that represents the asynchronous operation. The task result contains a 
	/// <see cref="T:M1.API.DTOs.BOM.BOMResponseMessageDto`1" /> which includes information about the deleted job assembly.
	/// </returns>
	Task<BOMResponseMessageDto<BOMJobAssemblyDto>> Process_DeleteJobAssembly(string jobId, int jobAssemblyId);
}
