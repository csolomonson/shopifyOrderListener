using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPProjectAreaModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all ProjectAreas with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ProjectAreas to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllProjectAreas(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving ProjectArea information based on the specified ProjectArea Unique Id.
	/// </summary>
	/// <param name="projectAreaId">The Unique Id of the ProjectArea.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetProjectArea(Guid projectAreaId);

	/// <summary>
	/// Validates the PUT request for creating or updating ProjectArea information based on the specified ProjectArea.
	/// </summary>
	/// <param name="projectArea">The ProjectArea details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutProjectArea(ERPProjectAreaDto projectArea);

	/// <summary>
	/// Processes the request to retrieve all ProjectAreas with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ProjectAreas to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ProjectAreas DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPProjectAreaDto>>> Process_GetAllProjectAreas(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific ProjectArea.
	/// </summary>
	/// <param name="projectAreaId">The Unique Id of the ProjectArea to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the ProjectArea DTO.</returns>
	Task<ERPResponseMessageDto<ERPProjectAreaDto>> Process_GetProjectArea(Guid projectAreaId);

	/// <summary>
	/// Processes the creating or updating of a ProjectArea record.
	/// </summary>
	/// <param name="projectArea">The ProjectArea data transfer object (DTO) containing the details of the ProjectArea to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the ProjectArea details.</returns>
	Task<ERPResponseMessageDto<ERPProjectAreaDto>> Process_PutProjectArea(ERPProjectAreaDto projectArea);

	/// <summary>
	/// Validates the request for deleting a ProjectArea record.
	/// </summary>
	/// <param name="projectAreaId">The Unique Id of the ProjectArea.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteProjectArea(Guid projectAreaId);

	/// <summary>
	/// Processes the request to delete a ProjectArea record.
	/// </summary>
	/// <param name="projectAreaId">The Unique Id of the ProjectArea.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPProjectAreaDto>> Process_DeleteProjectArea(Guid projectAreaId);
}
