using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPPurchasePlannerSessionRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a PurchasePlannerSession with the specified Unique Id exists.
	/// </summary>
	/// <param name="purchasePlannerSessionId">The Unique Id of the PurchasePlannerSession to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the PurchasePlannerSession exists or not.</returns>
	Task<bool> DoesPurchasePlannerSessionExist(Guid purchasePlannerSessionId);

	/// <summary>
	/// Retrieves all PurchasePlannerSessions with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PurchasePlannerSessions to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PurchasePlannerSessions DTOs.</returns>
	Task<ICollection<ERPPurchasePlannerSessionInformationDto>> GetAllPurchasePlannerSessions(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific PurchasePlannerSession.
	/// </summary>
	/// <param name="purchasePlannerSessionId">The Unique Id of the PurchasePlannerSession to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the PurchasePlannerSession DTO.</returns>
	Task<ERPPurchasePlannerSessionInformationDto> GetPurchasePlannerSession(Guid purchasePlannerSessionId);

	/// <summary>
	/// Saves the provided ERP purchasePlannerSession.
	/// </summary>
	/// <param name="purchasePlannerSession">The ERP purchasePlannerSession to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SavePurchasePlannerSession(ERPPurchasePlannerSessionDto purchasePlannerSession);
}
