using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPEmployeeQAApprovalRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a EmployeeQAApproval with the specified Unique Id exists.
	/// </summary>
	/// <param name="employeeQAApprovalId">The Unique Id of the EmployeeQAApproval to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the EmployeeQAApproval exists or not.</returns>
	Task<bool> DoesEmployeeQAApprovalExist(Guid employeeQAApprovalId);

	/// <summary>
	/// Retrieves all EmployeeQAApprovals with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of EmployeeQAApprovals to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of EmployeeQAApprovals DTOs.</returns>
	Task<ICollection<ERPEmployeeQAApprovalInformationDto>> GetAllEmployeeQAApprovals(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific EmployeeQAApproval.
	/// </summary>
	/// <param name="employeeQAApprovalId">The Unique Id of the EmployeeQAApproval to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the EmployeeQAApproval DTO.</returns>
	Task<ERPEmployeeQAApprovalInformationDto> GetEmployeeQAApproval(Guid employeeQAApprovalId);

	/// <summary>
	/// Saves the provided ERP employeeQAApproval.
	/// </summary>
	/// <param name="employeeQAApproval">The ERP employeeQAApproval to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveEmployeeQAApproval(ERPEmployeeQAApprovalDto employeeQAApproval);
}
