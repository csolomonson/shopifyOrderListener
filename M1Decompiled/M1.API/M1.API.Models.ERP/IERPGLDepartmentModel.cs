using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPGLDepartmentModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all GLDepartments with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of GLDepartments to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllGLDepartments(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving GLDepartment information based on the specified GLDepartment Unique Id.
	/// </summary>
	/// <param name="gLDepartmentId">The Unique Id of the GLDepartment.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetGLDepartment(Guid gLDepartmentId);

	/// <summary>
	/// Validates the PUT request for creating or updating GLDepartment information based on the specified GLDepartment.
	/// </summary>
	/// <param name="gLDepartment">The GLDepartment details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutGLDepartment(ERPGLDepartmentDto gLDepartment);

	/// <summary>
	/// Processes the request to retrieve all GLDepartments with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of GLDepartments to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of GLDepartments DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPGLDepartmentDto>>> Process_GetAllGLDepartments(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific GLDepartment.
	/// </summary>
	/// <param name="gLDepartmentId">The Unique Id of the GLDepartment to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the GLDepartment DTO.</returns>
	Task<ERPResponseMessageDto<ERPGLDepartmentDto>> Process_GetGLDepartment(Guid gLDepartmentId);

	/// <summary>
	/// Processes the creating or updating of a GLDepartment record.
	/// </summary>
	/// <param name="gLDepartment">The GLDepartment data transfer object (DTO) containing the details of the GLDepartment to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the GLDepartment details.</returns>
	Task<ERPResponseMessageDto<ERPGLDepartmentDto>> Process_PutGLDepartment(ERPGLDepartmentDto gLDepartment);

	/// <summary>
	/// Validates the request for deleting a GLDepartment record.
	/// </summary>
	/// <param name="gLDepartmentId">The Unique Id of the GLDepartment.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteGLDepartment(Guid gLDepartmentId);

	/// <summary>
	/// Processes the request to delete a GLDepartment record.
	/// </summary>
	/// <param name="gLDepartmentId">The Unique Id of the GLDepartment.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPGLDepartmentDto>> Process_DeleteGLDepartment(Guid gLDepartmentId);
}
