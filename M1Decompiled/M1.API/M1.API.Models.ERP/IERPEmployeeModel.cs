using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPEmployeeModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all Employees with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Employees to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllEmployees(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving Employee information based on the specified Employee Unique Id.
	/// </summary>
	/// <param name="employeeId">The Unique Id of the Employee.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetEmployee(Guid employeeId);

	/// <summary>
	/// Validates the PUT request for creating or updating Employee information based on the specified Employee.
	/// </summary>
	/// <param name="employee">The Employee details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutEmployee(ERPEmployeeDto employee);

	/// <summary>
	/// Processes the request to retrieve all Employees with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Employees to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of Employees DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPEmployeeDto>>> Process_GetAllEmployees(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific Employee.
	/// </summary>
	/// <param name="employeeId">The Unique Id of the Employee to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the Employee DTO.</returns>
	Task<ERPResponseMessageDto<ERPEmployeeDto>> Process_GetEmployee(Guid employeeId);

	/// <summary>
	/// Processes the creating or updating of a Employee record.
	/// </summary>
	/// <param name="employee">The Employee data transfer object (DTO) containing the details of the Employee to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the Employee details.</returns>
	Task<ERPResponseMessageDto<ERPEmployeeDto>> Process_PutEmployee(ERPEmployeeDto employee);

	/// <summary>
	/// Validates the request for deleting a Employee record.
	/// </summary>
	/// <param name="employeeId">The Unique Id of the Employee.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteEmployee(Guid employeeId);

	/// <summary>
	/// Processes the request to delete a Employee record.
	/// </summary>
	/// <param name="employeeId">The Unique Id of the Employee.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPEmployeeDto>> Process_DeleteEmployee(Guid employeeId);
}
