using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPJobMaterialComponentModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all JobMaterialComponents with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of JobMaterialComponents to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllJobMaterialComponents(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving JobMaterialComponent information based on the specified JobMaterialComponent Unique Id.
	/// </summary>
	/// <param name="jobMaterialComponentId">The Unique Id of the JobMaterialComponent.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetJobMaterialComponent(Guid jobMaterialComponentId);

	/// <summary>
	/// Validates the PUT request for creating or updating JobMaterialComponent information based on the specified JobMaterialComponent.
	/// </summary>
	/// <param name="jobMaterialComponent">The JobMaterialComponent details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutJobMaterialComponent(ERPJobMaterialComponentDto jobMaterialComponent);

	/// <summary>
	/// Processes the request to retrieve all JobMaterialComponents with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of JobMaterialComponents to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of JobMaterialComponents DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPJobMaterialComponentDto>>> Process_GetAllJobMaterialComponents(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific JobMaterialComponent.
	/// </summary>
	/// <param name="jobMaterialComponentId">The Unique Id of the JobMaterialComponent to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the JobMaterialComponent DTO.</returns>
	Task<ERPResponseMessageDto<ERPJobMaterialComponentDto>> Process_GetJobMaterialComponent(Guid jobMaterialComponentId);

	/// <summary>
	/// Processes the creating or updating of a JobMaterialComponent record.
	/// </summary>
	/// <param name="jobMaterialComponent">The JobMaterialComponent data transfer object (DTO) containing the details of the JobMaterialComponent to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the JobMaterialComponent details.</returns>
	Task<ERPResponseMessageDto<ERPJobMaterialComponentDto>> Process_PutJobMaterialComponent(ERPJobMaterialComponentDto jobMaterialComponent);

	/// <summary>
	/// Validates the request for deleting a JobMaterialComponent record.
	/// </summary>
	/// <param name="jobMaterialComponentId">The Unique Id of the JobMaterialComponent.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteJobMaterialComponent(Guid jobMaterialComponentId);

	/// <summary>
	/// Processes the request to delete a JobMaterialComponent record.
	/// </summary>
	/// <param name="jobMaterialComponentId">The Unique Id of the JobMaterialComponent.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPJobMaterialComponentDto>> Process_DeleteJobMaterialComponent(Guid jobMaterialComponentId);
}
