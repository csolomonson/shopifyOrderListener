using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPEmployeePOApprovalRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a EmployeePOApproval with the specified Unique Id exists.
	/// </summary>
	/// <param name="employeePOApprovalId">The Unique Id of the EmployeePOApproval to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the EmployeePOApproval exists or not.</returns>
	Task<bool> DoesEmployeePOApprovalExist(Guid employeePOApprovalId);

	/// <summary>
	/// Retrieves all EmployeePOApprovals with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of EmployeePOApprovals to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of EmployeePOApprovals DTOs.</returns>
	Task<ICollection<ERPEmployeePOApprovalInformationDto>> GetAllEmployeePOApprovals(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific EmployeePOApproval.
	/// </summary>
	/// <param name="employeePOApprovalId">The Unique Id of the EmployeePOApproval to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the EmployeePOApproval DTO.</returns>
	Task<ERPEmployeePOApprovalInformationDto> GetEmployeePOApproval(Guid employeePOApprovalId);

	/// <summary>
	/// Saves the provided ERP employeePOApproval.
	/// </summary>
	/// <param name="employeePOApproval">The ERP employeePOApproval to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveEmployeePOApproval(ERPEmployeePOApprovalDto employeePOApproval);
}
