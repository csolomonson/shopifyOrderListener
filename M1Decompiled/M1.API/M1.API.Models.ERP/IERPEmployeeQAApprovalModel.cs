using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPEmployeeQAApprovalModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all EmployeeQAApprovals with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of EmployeeQAApprovals to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllEmployeeQAApprovals(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving EmployeeQAApproval information based on the specified EmployeeQAApproval Unique Id.
	/// </summary>
	/// <param name="employeeQAApprovalId">The Unique Id of the EmployeeQAApproval.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetEmployeeQAApproval(Guid employeeQAApprovalId);

	/// <summary>
	/// Validates the PUT request for creating or updating EmployeeQAApproval information based on the specified EmployeeQAApproval.
	/// </summary>
	/// <param name="employeeQAApproval">The EmployeeQAApproval details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutEmployeeQAApproval(ERPEmployeeQAApprovalDto employeeQAApproval);

	/// <summary>
	/// Processes the request to retrieve all EmployeeQAApprovals with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of EmployeeQAApprovals to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of EmployeeQAApprovals DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPEmployeeQAApprovalDto>>> Process_GetAllEmployeeQAApprovals(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific EmployeeQAApproval.
	/// </summary>
	/// <param name="employeeQAApprovalId">The Unique Id of the EmployeeQAApproval to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the EmployeeQAApproval DTO.</returns>
	Task<ERPResponseMessageDto<ERPEmployeeQAApprovalDto>> Process_GetEmployeeQAApproval(Guid employeeQAApprovalId);

	/// <summary>
	/// Processes the creating or updating of a EmployeeQAApproval record.
	/// </summary>
	/// <param name="employeeQAApproval">The EmployeeQAApproval data transfer object (DTO) containing the details of the EmployeeQAApproval to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the EmployeeQAApproval details.</returns>
	Task<ERPResponseMessageDto<ERPEmployeeQAApprovalDto>> Process_PutEmployeeQAApproval(ERPEmployeeQAApprovalDto employeeQAApproval);

	/// <summary>
	/// Validates the request for deleting a EmployeeQAApproval record.
	/// </summary>
	/// <param name="employeeQAApprovalId">The Unique Id of the EmployeeQAApproval.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteEmployeeQAApproval(Guid employeeQAApprovalId);

	/// <summary>
	/// Processes the request to delete a EmployeeQAApproval record.
	/// </summary>
	/// <param name="employeeQAApprovalId">The Unique Id of the EmployeeQAApproval.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPEmployeeQAApprovalDto>> Process_DeleteEmployeeQAApproval(Guid employeeQAApprovalId);
}
