using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPPurchasePlannerOrderDetailRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a PurchasePlannerOrderDetail with the specified Unique Id exists.
	/// </summary>
	/// <param name="purchasePlannerOrderDetailId">The Unique Id of the PurchasePlannerOrderDetail to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the PurchasePlannerOrderDetail exists or not.</returns>
	Task<bool> DoesPurchasePlannerOrderDetailExist(Guid purchasePlannerOrderDetailId);

	/// <summary>
	/// Retrieves all PurchasePlannerOrderDetails with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PurchasePlannerOrderDetails to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PurchasePlannerOrderDetails DTOs.</returns>
	Task<ICollection<ERPPurchasePlannerOrderDetailInformationDto>> GetAllPurchasePlannerOrderDetails(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific PurchasePlannerOrderDetail.
	/// </summary>
	/// <param name="purchasePlannerOrderDetailId">The Unique Id of the PurchasePlannerOrderDetail to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the PurchasePlannerOrderDetail DTO.</returns>
	Task<ERPPurchasePlannerOrderDetailInformationDto> GetPurchasePlannerOrderDetail(Guid purchasePlannerOrderDetailId);

	/// <summary>
	/// Saves the provided ERP purchasePlannerOrderDetail.
	/// </summary>
	/// <param name="purchasePlannerOrderDetail">The ERP purchasePlannerOrderDetail to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SavePurchasePlannerOrderDetail(ERPPurchasePlannerOrderDetailDto purchasePlannerOrderDetail);
}
