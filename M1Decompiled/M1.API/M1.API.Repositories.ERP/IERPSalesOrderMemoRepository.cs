using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPSalesOrderMemoRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a SalesOrderMemo with the specified Unique Id exists.
	/// </summary>
	/// <param name="salesOrderMemoId">The Unique Id of the SalesOrderMemo to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the SalesOrderMemo exists or not.</returns>
	Task<bool> DoesSalesOrderMemoExist(Guid salesOrderMemoId);

	/// <summary>
	/// Retrieves all SalesOrderMemos with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of SalesOrderMemos to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of SalesOrderMemos DTOs.</returns>
	Task<ICollection<ERPSalesOrderMemoInformationDto>> GetAllSalesOrderMemos(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific SalesOrderMemo.
	/// </summary>
	/// <param name="salesOrderMemoId">The Unique Id of the SalesOrderMemo to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the SalesOrderMemo DTO.</returns>
	Task<ERPSalesOrderMemoInformationDto> GetSalesOrderMemo(Guid salesOrderMemoId);

	/// <summary>
	/// Saves the provided ERP salesOrderMemo.
	/// </summary>
	/// <param name="salesOrderMemo">The ERP salesOrderMemo to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveSalesOrderMemo(ERPSalesOrderMemoDto salesOrderMemo);
}
