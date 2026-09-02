using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPPurchaseOrderComponentRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a PurchaseOrderComponent with the specified Unique Id exists.
	/// </summary>
	/// <param name="purchaseOrderComponentId">The Unique Id of the PurchaseOrderComponent to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the PurchaseOrderComponent exists or not.</returns>
	Task<bool> DoesPurchaseOrderComponentExist(Guid purchaseOrderComponentId);

	/// <summary>
	/// Retrieves all PurchaseOrderComponents with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PurchaseOrderComponents to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PurchaseOrderComponents DTOs.</returns>
	Task<ICollection<ERPPurchaseOrderComponentInformationDto>> GetAllPurchaseOrderComponents(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific PurchaseOrderComponent.
	/// </summary>
	/// <param name="purchaseOrderComponentId">The Unique Id of the PurchaseOrderComponent to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the PurchaseOrderComponent DTO.</returns>
	Task<ERPPurchaseOrderComponentInformationDto> GetPurchaseOrderComponent(Guid purchaseOrderComponentId);

	/// <summary>
	/// Saves the provided ERP purchaseOrderComponent.
	/// </summary>
	/// <param name="purchaseOrderComponent">The ERP purchaseOrderComponent to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SavePurchaseOrderComponent(ERPPurchaseOrderComponentDto purchaseOrderComponent);
}
