using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPPlantDepartmentModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all PlantDepartments with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PlantDepartments to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllPlantDepartments(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving PlantDepartment information based on the specified PlantDepartment Unique Id.
	/// </summary>
	/// <param name="plantDepartmentId">The Unique Id of the PlantDepartment.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetPlantDepartment(Guid plantDepartmentId);

	/// <summary>
	/// Validates the PUT request for creating or updating PlantDepartment information based on the specified PlantDepartment.
	/// </summary>
	/// <param name="plantDepartment">The PlantDepartment details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutPlantDepartment(ERPPlantDepartmentDto plantDepartment);

	/// <summary>
	/// Processes the request to retrieve all PlantDepartments with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PlantDepartments to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PlantDepartments DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPPlantDepartmentDto>>> Process_GetAllPlantDepartments(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific PlantDepartment.
	/// </summary>
	/// <param name="plantDepartmentId">The Unique Id of the PlantDepartment to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the PlantDepartment DTO.</returns>
	Task<ERPResponseMessageDto<ERPPlantDepartmentDto>> Process_GetPlantDepartment(Guid plantDepartmentId);

	/// <summary>
	/// Processes the creating or updating of a PlantDepartment record.
	/// </summary>
	/// <param name="plantDepartment">The PlantDepartment data transfer object (DTO) containing the details of the PlantDepartment to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the PlantDepartment details.</returns>
	Task<ERPResponseMessageDto<ERPPlantDepartmentDto>> Process_PutPlantDepartment(ERPPlantDepartmentDto plantDepartment);

	/// <summary>
	/// Validates the request for deleting a PlantDepartment record.
	/// </summary>
	/// <param name="plantDepartmentId">The Unique Id of the PlantDepartment.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeletePlantDepartment(Guid plantDepartmentId);

	/// <summary>
	/// Processes the request to delete a PlantDepartment record.
	/// </summary>
	/// <param name="plantDepartmentId">The Unique Id of the PlantDepartment.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPPlantDepartmentDto>> Process_DeletePlantDepartment(Guid plantDepartmentId);
}
