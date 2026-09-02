using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPProjectTypeModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all ProjectTypes with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ProjectTypes to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllProjectTypes(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving ProjectType information based on the specified ProjectType Unique Id.
	/// </summary>
	/// <param name="projectTypeId">The Unique Id of the ProjectType.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetProjectType(Guid projectTypeId);

	/// <summary>
	/// Validates the PUT request for creating or updating ProjectType information based on the specified ProjectType.
	/// </summary>
	/// <param name="projectType">The ProjectType details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutProjectType(ERPProjectTypeDto projectType);

	/// <summary>
	/// Processes the request to retrieve all ProjectTypes with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ProjectTypes to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ProjectTypes DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPProjectTypeDto>>> Process_GetAllProjectTypes(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific ProjectType.
	/// </summary>
	/// <param name="projectTypeId">The Unique Id of the ProjectType to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the ProjectType DTO.</returns>
	Task<ERPResponseMessageDto<ERPProjectTypeDto>> Process_GetProjectType(Guid projectTypeId);

	/// <summary>
	/// Processes the creating or updating of a ProjectType record.
	/// </summary>
	/// <param name="projectType">The ProjectType data transfer object (DTO) containing the details of the ProjectType to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the ProjectType details.</returns>
	Task<ERPResponseMessageDto<ERPProjectTypeDto>> Process_PutProjectType(ERPProjectTypeDto projectType);

	/// <summary>
	/// Validates the request for deleting a ProjectType record.
	/// </summary>
	/// <param name="projectTypeId">The Unique Id of the ProjectType.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteProjectType(Guid projectTypeId);

	/// <summary>
	/// Processes the request to delete a ProjectType record.
	/// </summary>
	/// <param name="projectTypeId">The Unique Id of the ProjectType.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPProjectTypeDto>> Process_DeleteProjectType(Guid projectTypeId);
}
