using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPPurchaseOrderDeliveryRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a PurchaseOrderDelivery with the specified Unique Id exists.
	/// </summary>
	/// <param name="purchaseOrderDeliveryId">The Unique Id of the PurchaseOrderDelivery to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the PurchaseOrderDelivery exists or not.</returns>
	Task<bool> DoesPurchaseOrderDeliveryExist(Guid purchaseOrderDeliveryId);

	/// <summary>
	/// Retrieves all PurchaseOrderDeliveries with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PurchaseOrderDeliveries to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PurchaseOrderDeliveries DTOs.</returns>
	Task<ICollection<ERPPurchaseOrderDeliveryInformationDto>> GetAllPurchaseOrderDeliveries(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific PurchaseOrderDelivery.
	/// </summary>
	/// <param name="purchaseOrderDeliveryId">The Unique Id of the PurchaseOrderDelivery to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the PurchaseOrderDelivery DTO.</returns>
	Task<ERPPurchaseOrderDeliveryInformationDto> GetPurchaseOrderDelivery(Guid purchaseOrderDeliveryId);

	/// <summary>
	/// Saves the provided ERP purchaseOrderDelivery.
	/// </summary>
	/// <param name="purchaseOrderDelivery">The ERP purchaseOrderDelivery to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SavePurchaseOrderDelivery(ERPPurchaseOrderDeliveryDto purchaseOrderDelivery);
}
