using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPPurchaseOrderAccountRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a PurchaseOrderAccount with the specified Unique Id exists.
	/// </summary>
	/// <param name="purchaseOrderAccountId">The Unique Id of the PurchaseOrderAccount to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the PurchaseOrderAccount exists or not.</returns>
	Task<bool> DoesPurchaseOrderAccountExist(Guid purchaseOrderAccountId);

	/// <summary>
	/// Retrieves all PurchaseOrderAccounts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PurchaseOrderAccounts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PurchaseOrderAccounts DTOs.</returns>
	Task<ICollection<ERPPurchaseOrderAccountInformationDto>> GetAllPurchaseOrderAccounts(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific PurchaseOrderAccount.
	/// </summary>
	/// <param name="purchaseOrderAccountId">The Unique Id of the PurchaseOrderAccount to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the PurchaseOrderAccount DTO.</returns>
	Task<ERPPurchaseOrderAccountInformationDto> GetPurchaseOrderAccount(Guid purchaseOrderAccountId);

	/// <summary>
	/// Saves the provided ERP purchaseOrderAccount.
	/// </summary>
	/// <param name="purchaseOrderAccount">The ERP purchaseOrderAccount to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SavePurchaseOrderAccount(ERPPurchaseOrderAccountDto purchaseOrderAccount);
}
