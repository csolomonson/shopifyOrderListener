using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.DTOs.BOM.Job;
using M1.API.DTOs.Core;

namespace M1.API.Models.BOM.Job;

public interface IBOMJobMaterialModel : IBOMBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving a job material.
	/// </summary>
	/// <param name="jobId">The ID of the job.</param>
	/// <param name="jobAssemblyId">The ID of the job assembly.</param>
	/// <param name="jobMaterialId">The ID of the job material to be retrieved.</param>
	/// <returns>
	/// A task that represents the asynchronous operation. The task result contains the API validation information.
	/// </returns>
	Task<APIValidationInfoDto> ValidateRequest_GetJobMaterial(string jobId, int jobAssemblyId, int jobMaterialId);

	/// <summary>
	/// Validates the POST request for retrieving JobMaterial information based on the specified JobMaterial.
	/// </summary>
	/// <param name="jobMaterial">The JobMaterial details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PostJobMaterial(BOMJobMaterialDto jobMaterial);

	/// <summary>
	/// Validates the request for deleting a job material.
	/// </summary>
	/// <param name="jobId">The ID of the job.</param>
	/// <param name="jobAssemblyId">The ID of the job assembly.</param>
	/// <param name="jobMaterialId">The ID of the job material to be deleted.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the API validation information.</returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteJobMaterial(string jobId, int jobAssemblyId, int jobMaterialId);

	/// <summary>
	/// Processes the request to retrieve all JobMaterials with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of JobMaterials to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a BOM response message DTO with a list of JobMaterials DTOs.</returns>
	Task<BOMResponseMessageDto<IList<BOMJobMaterialDto>>> Process_GetAllJobMaterials(int pageSize, int pageNumber);

	/// <summary>
	/// Processes the retrieval of a job material.
	/// </summary>
	/// <param name="jobId">The ID of the job.</param>
	/// <param name="jobAssemblyId">The ID of the job assembly.</param>
	/// <param name="jobMaterialId">The ID of the job material to be retrieved.</param>
	/// <returns>
	/// A task that represents the asynchronous operation. The task result contains a 
	/// <see cref="T:M1.API.DTOs.BOM.BOMResponseMessageDto`1" /> which includes information about the retrieved job material.
	/// </returns>
	Task<BOMResponseMessageDto<BOMJobMaterialDto>> Process_GetJobMaterial(string jobId, int jobAssemblyId, int jobMaterialId);

	/// <summary>
	/// Processes the posting of JobMaterial.
	/// </summary>
	/// <param name="jobMaterial">The JobMaterial data transfer object (DTO) containing the details of the jobMaterial to be posted.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.BOM.BOMResponseMessageDto`1" /> with the response message and the jobMaterial details.</returns>
	Task<BOMResponseMessageDto<BOMJobMaterialDto>> Process_PostJobMaterial(BOMJobMaterialDto jobMaterial);

	/// <summary>
	/// Processes the deletion of a job material.
	/// </summary>
	/// <param name="jobId">The ID of the job.</param>
	/// <param name="jobAssemblyId">The ID of the job assembly.</param>
	/// <param name="jobMaterialId">The ID of the job material to be deleted.</param>
	/// <returns>
	/// A task that represents the asynchronous operation. The task result contains a 
	/// <see cref="T:M1.API.DTOs.BOM.BOMResponseMessageDto`1" /> which includes information about the deleted job material.
	/// </returns>
	Task<BOMResponseMessageDto<BOMJobMaterialDto>> Process_DeleteJobMaterial(string jobId, int jobAssemblyId, int jobMaterialId);
}
