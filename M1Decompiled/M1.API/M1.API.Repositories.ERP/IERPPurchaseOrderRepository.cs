using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPPurchaseOrderRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a PurchaseOrder with the specified Unique Id exists.
	/// </summary>
	/// <param name="purchaseOrderId">The Unique Id of the PurchaseOrder to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the PurchaseOrder exists or not.</returns>
	Task<bool> DoesPurchaseOrderExist(Guid purchaseOrderId);

	/// <summary>
	/// Retrieves all PurchaseOrders with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PurchaseOrders to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PurchaseOrders DTOs.</returns>
	Task<ICollection<ERPPurchaseOrderInformationDto>> GetAllPurchaseOrders(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific PurchaseOrder.
	/// </summary>
	/// <param name="purchaseOrderId">The Unique Id of the PurchaseOrder to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the PurchaseOrder DTO.</returns>
	Task<ERPPurchaseOrderInformationDto> GetPurchaseOrder(Guid purchaseOrderId);

	/// <summary>
	/// Saves the provided ERP purchaseOrder.
	/// </summary>
	/// <param name="purchaseOrder">The ERP purchaseOrder to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SavePurchaseOrder(ERPPurchaseOrderDto purchaseOrder);
}
