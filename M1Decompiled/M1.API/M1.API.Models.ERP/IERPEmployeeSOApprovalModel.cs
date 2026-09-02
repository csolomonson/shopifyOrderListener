using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPEmployeeSOApprovalModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all EmployeeSOApprovals with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of EmployeeSOApprovals to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllEmployeeSOApprovals(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving EmployeeSOApproval information based on the specified EmployeeSOApproval Unique Id.
	/// </summary>
	/// <param name="employeeSOApprovalId">The Unique Id of the EmployeeSOApproval.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetEmployeeSOApproval(Guid employeeSOApprovalId);

	/// <summary>
	/// Validates the PUT request for creating or updating EmployeeSOApproval information based on the specified EmployeeSOApproval.
	/// </summary>
	/// <param name="employeeSOApproval">The EmployeeSOApproval details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutEmployeeSOApproval(ERPEmployeeSOApprovalDto employeeSOApproval);

	/// <summary>
	/// Processes the request to retrieve all EmployeeSOApprovals with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of EmployeeSOApprovals to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of EmployeeSOApprovals DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPEmployeeSOApprovalDto>>> Process_GetAllEmployeeSOApprovals(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific EmployeeSOApproval.
	/// </summary>
	/// <param name="employeeSOApprovalId">The Unique Id of the EmployeeSOApproval to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the EmployeeSOApproval DTO.</returns>
	Task<ERPResponseMessageDto<ERPEmployeeSOApprovalDto>> Process_GetEmployeeSOApproval(Guid employeeSOApprovalId);

	/// <summary>
	/// Processes the creating or updating of a EmployeeSOApproval record.
	/// </summary>
	/// <param name="employeeSOApproval">The EmployeeSOApproval data transfer object (DTO) containing the details of the EmployeeSOApproval to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the EmployeeSOApproval details.</returns>
	Task<ERPResponseMessageDto<ERPEmployeeSOApprovalDto>> Process_PutEmployeeSOApproval(ERPEmployeeSOApprovalDto employeeSOApproval);

	/// <summary>
	/// Validates the request for deleting a EmployeeSOApproval record.
	/// </summary>
	/// <param name="employeeSOApprovalId">The Unique Id of the EmployeeSOApproval.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteEmployeeSOApproval(Guid employeeSOApprovalId);

	/// <summary>
	/// Processes the request to delete a EmployeeSOApproval record.
	/// </summary>
	/// <param name="employeeSOApprovalId">The Unique Id of the EmployeeSOApproval.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPEmployeeSOApprovalDto>> Process_DeleteEmployeeSOApproval(Guid employeeSOApprovalId);
}
