using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPProjectModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all Projects with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Projects to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllProjects(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving Project information based on the specified Project Unique Id.
	/// </summary>
	/// <param name="projectId">The Unique Id of the Project.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetProject(Guid projectId);

	/// <summary>
	/// Validates the PUT request for creating or updating Project information based on the specified Project.
	/// </summary>
	/// <param name="project">The Project details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutProject(ERPProjectDto project);

	/// <summary>
	/// Processes the request to retrieve all Projects with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Projects to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of Projects DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPProjectDto>>> Process_GetAllProjects(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific Project.
	/// </summary>
	/// <param name="projectId">The Unique Id of the Project to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the Project DTO.</returns>
	Task<ERPResponseMessageDto<ERPProjectDto>> Process_GetProject(Guid projectId);

	/// <summary>
	/// Processes the creating or updating of a Project record.
	/// </summary>
	/// <param name="project">The Project data transfer object (DTO) containing the details of the Project to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the Project details.</returns>
	Task<ERPResponseMessageDto<ERPProjectDto>> Process_PutProject(ERPProjectDto project);

	/// <summary>
	/// Validates the request for deleting a Project record.
	/// </summary>
	/// <param name="projectId">The Unique Id of the Project.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteProject(Guid projectId);

	/// <summary>
	/// Processes the request to delete a Project record.
	/// </summary>
	/// <param name="projectId">The Unique Id of the Project.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPProjectDto>> Process_DeleteProject(Guid projectId);
}
