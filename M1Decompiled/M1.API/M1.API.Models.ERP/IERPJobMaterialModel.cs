using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPJobMaterialModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all JobMaterials with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of JobMaterials to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllJobMaterials(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving JobMaterial information based on the specified JobMaterial Unique Id.
	/// </summary>
	/// <param name="jobMaterialId">The Unique Id of the JobMaterial.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetJobMaterial(Guid jobMaterialId);

	/// <summary>
	/// Validates the PUT request for creating or updating JobMaterial information based on the specified JobMaterial.
	/// </summary>
	/// <param name="jobMaterial">The JobMaterial details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutJobMaterial(ERPJobMaterialDto jobMaterial);

	/// <summary>
	/// Processes the request to retrieve all JobMaterials with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of JobMaterials to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of JobMaterials DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPJobMaterialDto>>> Process_GetAllJobMaterials(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific JobMaterial.
	/// </summary>
	/// <param name="jobMaterialId">The Unique Id of the JobMaterial to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the JobMaterial DTO.</returns>
	Task<ERPResponseMessageDto<ERPJobMaterialDto>> Process_GetJobMaterial(Guid jobMaterialId);

	/// <summary>
	/// Processes the creating or updating of a JobMaterial record.
	/// </summary>
	/// <param name="jobMaterial">The JobMaterial data transfer object (DTO) containing the details of the JobMaterial to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the JobMaterial details.</returns>
	Task<ERPResponseMessageDto<ERPJobMaterialDto>> Process_PutJobMaterial(ERPJobMaterialDto jobMaterial);

	/// <summary>
	/// Validates the request for deleting a JobMaterial record.
	/// </summary>
	/// <param name="jobMaterialId">The Unique Id of the JobMaterial.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteJobMaterial(Guid jobMaterialId);

	/// <summary>
	/// Processes the request to delete a JobMaterial record.
	/// </summary>
	/// <param name="jobMaterialId">The Unique Id of the JobMaterial.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPJobMaterialDto>> Process_DeleteJobMaterial(Guid jobMaterialId);
}
