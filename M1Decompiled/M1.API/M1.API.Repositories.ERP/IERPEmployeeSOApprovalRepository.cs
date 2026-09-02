using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPEmployeeSOApprovalRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a EmployeeSOApproval with the specified Unique Id exists.
	/// </summary>
	/// <param name="employeeSOApprovalId">The Unique Id of the EmployeeSOApproval to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the EmployeeSOApproval exists or not.</returns>
	Task<bool> DoesEmployeeSOApprovalExist(Guid employeeSOApprovalId);

	/// <summary>
	/// Retrieves all EmployeeSOApprovals with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of EmployeeSOApprovals to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of EmployeeSOApprovals DTOs.</returns>
	Task<ICollection<ERPEmployeeSOApprovalInformationDto>> GetAllEmployeeSOApprovals(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific EmployeeSOApproval.
	/// </summary>
	/// <param name="employeeSOApprovalId">The Unique Id of the EmployeeSOApproval to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the EmployeeSOApproval DTO.</returns>
	Task<ERPEmployeeSOApprovalInformationDto> GetEmployeeSOApproval(Guid employeeSOApprovalId);

	/// <summary>
	/// Saves the provided ERP employeeSOApproval.
	/// </summary>
	/// <param name="employeeSOApproval">The ERP employeeSOApproval to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveEmployeeSOApproval(ERPEmployeeSOApprovalDto employeeSOApproval);
}
