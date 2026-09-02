using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPSalesOrderApprovalRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a SalesOrderApproval with the specified Unique Id exists.
	/// </summary>
	/// <param name="salesOrderApprovalId">The Unique Id of the SalesOrderApproval to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the SalesOrderApproval exists or not.</returns>
	Task<bool> DoesSalesOrderApprovalExist(Guid salesOrderApprovalId);

	/// <summary>
	/// Retrieves all SalesOrderApprovals with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of SalesOrderApprovals to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of SalesOrderApprovals DTOs.</returns>
	Task<ICollection<ERPSalesOrderApprovalInformationDto>> GetAllSalesOrderApprovals(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific SalesOrderApproval.
	/// </summary>
	/// <param name="salesOrderApprovalId">The Unique Id of the SalesOrderApproval to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the SalesOrderApproval DTO.</returns>
	Task<ERPSalesOrderApprovalInformationDto> GetSalesOrderApproval(Guid salesOrderApprovalId);

	/// <summary>
	/// Saves the provided ERP salesOrderApproval.
	/// </summary>
	/// <param name="salesOrderApproval">The ERP salesOrderApproval to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveSalesOrderApproval(ERPSalesOrderApprovalDto salesOrderApproval);
}
