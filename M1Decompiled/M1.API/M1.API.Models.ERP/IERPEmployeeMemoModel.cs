using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPEmployeeMemoModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all EmployeeMemos with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of EmployeeMemos to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllEmployeeMemos(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving EmployeeMemo information based on the specified EmployeeMemo Unique Id.
	/// </summary>
	/// <param name="employeeMemoId">The Unique Id of the EmployeeMemo.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetEmployeeMemo(Guid employeeMemoId);

	/// <summary>
	/// Validates the PUT request for creating or updating EmployeeMemo information based on the specified EmployeeMemo.
	/// </summary>
	/// <param name="employeeMemo">The EmployeeMemo details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutEmployeeMemo(ERPEmployeeMemoDto employeeMemo);

	/// <summary>
	/// Processes the request to retrieve all EmployeeMemos with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of EmployeeMemos to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of EmployeeMemos DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPEmployeeMemoDto>>> Process_GetAllEmployeeMemos(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific EmployeeMemo.
	/// </summary>
	/// <param name="employeeMemoId">The Unique Id of the EmployeeMemo to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the EmployeeMemo DTO.</returns>
	Task<ERPResponseMessageDto<ERPEmployeeMemoDto>> Process_GetEmployeeMemo(Guid employeeMemoId);

	/// <summary>
	/// Processes the creating or updating of a EmployeeMemo record.
	/// </summary>
	/// <param name="employeeMemo">The EmployeeMemo data transfer object (DTO) containing the details of the EmployeeMemo to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the EmployeeMemo details.</returns>
	Task<ERPResponseMessageDto<ERPEmployeeMemoDto>> Process_PutEmployeeMemo(ERPEmployeeMemoDto employeeMemo);

	/// <summary>
	/// Validates the request for deleting a EmployeeMemo record.
	/// </summary>
	/// <param name="employeeMemoId">The Unique Id of the EmployeeMemo.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteEmployeeMemo(Guid employeeMemoId);

	/// <summary>
	/// Processes the request to delete a EmployeeMemo record.
	/// </summary>
	/// <param name="employeeMemoId">The Unique Id of the EmployeeMemo.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPEmployeeMemoDto>> Process_DeleteEmployeeMemo(Guid employeeMemoId);
}
