using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPEmployeePOApprovalModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all EmployeePOApprovals with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of EmployeePOApprovals to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllEmployeePOApprovals(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving EmployeePOApproval information based on the specified EmployeePOApproval Unique Id.
	/// </summary>
	/// <param name="employeePOApprovalId">The Unique Id of the EmployeePOApproval.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetEmployeePOApproval(Guid employeePOApprovalId);

	/// <summary>
	/// Validates the PUT request for creating or updating EmployeePOApproval information based on the specified EmployeePOApproval.
	/// </summary>
	/// <param name="employeePOApproval">The EmployeePOApproval details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutEmployeePOApproval(ERPEmployeePOApprovalDto employeePOApproval);

	/// <summary>
	/// Processes the request to retrieve all EmployeePOApprovals with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of EmployeePOApprovals to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of EmployeePOApprovals DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPEmployeePOApprovalDto>>> Process_GetAllEmployeePOApprovals(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific EmployeePOApproval.
	/// </summary>
	/// <param name="employeePOApprovalId">The Unique Id of the EmployeePOApproval to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the EmployeePOApproval DTO.</returns>
	Task<ERPResponseMessageDto<ERPEmployeePOApprovalDto>> Process_GetEmployeePOApproval(Guid employeePOApprovalId);

	/// <summary>
	/// Processes the creating or updating of a EmployeePOApproval record.
	/// </summary>
	/// <param name="employeePOApproval">The EmployeePOApproval data transfer object (DTO) containing the details of the EmployeePOApproval to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the EmployeePOApproval details.</returns>
	Task<ERPResponseMessageDto<ERPEmployeePOApprovalDto>> Process_PutEmployeePOApproval(ERPEmployeePOApprovalDto employeePOApproval);

	/// <summary>
	/// Validates the request for deleting a EmployeePOApproval record.
	/// </summary>
	/// <param name="employeePOApprovalId">The Unique Id of the EmployeePOApproval.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteEmployeePOApproval(Guid employeePOApprovalId);

	/// <summary>
	/// Processes the request to delete a EmployeePOApproval record.
	/// </summary>
	/// <param name="employeePOApprovalId">The Unique Id of the EmployeePOApproval.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPEmployeePOApprovalDto>> Process_DeleteEmployeePOApproval(Guid employeePOApprovalId);
}
