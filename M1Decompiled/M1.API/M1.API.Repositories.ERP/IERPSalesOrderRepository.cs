using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPSalesOrderRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a SalesOrder with the specified Unique Id exists.
	/// </summary>
	/// <param name="salesOrderId">The Unique Id of the SalesOrder to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the SalesOrder exists or not.</returns>
	Task<bool> DoesSalesOrderExist(Guid salesOrderId);

	/// <summary>
	/// Retrieves all SalesOrders with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of SalesOrders to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of SalesOrders DTOs.</returns>
	Task<ICollection<ERPSalesOrderInformationDto>> GetAllSalesOrders(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific SalesOrder.
	/// </summary>
	/// <param name="salesOrderId">The Unique Id of the SalesOrder to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the SalesOrder DTO.</returns>
	Task<ERPSalesOrderInformationDto> GetSalesOrder(Guid salesOrderId);

	/// <summary>
	/// Saves the provided ERP salesOrder.
	/// </summary>
	/// <param name="salesOrder">The ERP salesOrder to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveSalesOrder(ERPSalesOrderDto salesOrder);
}
