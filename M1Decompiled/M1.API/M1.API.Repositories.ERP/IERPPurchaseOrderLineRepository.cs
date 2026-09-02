using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPPurchaseOrderLineRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a PurchaseOrderLine with the specified Unique Id exists.
	/// </summary>
	/// <param name="purchaseOrderLineId">The Unique Id of the PurchaseOrderLine to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the PurchaseOrderLine exists or not.</returns>
	Task<bool> DoesPurchaseOrderLineExist(Guid purchaseOrderLineId);

	/// <summary>
	/// Retrieves all PurchaseOrderLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PurchaseOrderLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PurchaseOrderLines DTOs.</returns>
	Task<ICollection<ERPPurchaseOrderLineInformationDto>> GetAllPurchaseOrderLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific PurchaseOrderLine.
	/// </summary>
	/// <param name="purchaseOrderLineId">The Unique Id of the PurchaseOrderLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the PurchaseOrderLine DTO.</returns>
	Task<ERPPurchaseOrderLineInformationDto> GetPurchaseOrderLine(Guid purchaseOrderLineId);

	/// <summary>
	/// Saves the provided ERP purchaseOrderLine.
	/// </summary>
	/// <param name="purchaseOrderLine">The ERP purchaseOrderLine to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SavePurchaseOrderLine(ERPPurchaseOrderLineDto purchaseOrderLine);
}
