using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPSalesOrderLineRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a SalesOrderLine with the specified Unique Id exists.
	/// </summary>
	/// <param name="salesOrderLineId">The Unique Id of the SalesOrderLine to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the SalesOrderLine exists or not.</returns>
	Task<bool> DoesSalesOrderLineExist(Guid salesOrderLineId);

	/// <summary>
	/// Retrieves all SalesOrderLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of SalesOrderLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of SalesOrderLines DTOs.</returns>
	Task<ICollection<ERPSalesOrderLineInformationDto>> GetAllSalesOrderLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific SalesOrderLine.
	/// </summary>
	/// <param name="salesOrderLineId">The Unique Id of the SalesOrderLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the SalesOrderLine DTO.</returns>
	Task<ERPSalesOrderLineInformationDto> GetSalesOrderLine(Guid salesOrderLineId);

	/// <summary>
	/// Saves the provided ERP salesOrderLine.
	/// </summary>
	/// <param name="salesOrderLine">The ERP salesOrderLine to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveSalesOrderLine(ERPSalesOrderLineDto salesOrderLine);
}
