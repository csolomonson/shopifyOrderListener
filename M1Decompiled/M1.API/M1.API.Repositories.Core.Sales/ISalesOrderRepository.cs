using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.BOM.Sales;

namespace M1.API.Repositories.Core.Sales;

public interface ISalesOrderRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a SalesOrder with the specified ID exists.
	/// </summary>
	/// <param name="salesOrderId">The ID of the SalesOrder to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the SalesOrder exists or not.</returns>
	Task<bool> DoesSalesOrderExists(string salesOrderId);

	/// <summary>
	/// Retrieves all SalesOrder with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of SalesOrders to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a BOM response message DTO with a list of SalesOrders DTOs.</returns>
	Task<ICollection<BOMSalesOrderDto>> GetAllSalesOrders(int? pageSize = null, int? pageNumber = null);

	/// <summary>
	/// Retrieves detailed information about a specific SalesOrder.
	/// </summary>
	/// <param name="salesOrderId">The ID of the SalesOrder to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the SalesOrder DTO.</returns>
	Task<BOMSalesOrderDto> GetSalesOrder(string salesOrderId);
}
