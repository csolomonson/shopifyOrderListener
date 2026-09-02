using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPPartTransactionCostRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a PartTransactionCost with the specified Unique Id exists.
	/// </summary>
	/// <param name="partTransactionCostId">The Unique Id of the PartTransactionCost to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the PartTransactionCost exists or not.</returns>
	Task<bool> DoesPartTransactionCostExist(Guid partTransactionCostId);

	/// <summary>
	/// Retrieves all PartTransactionCosts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartTransactionCosts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PartTransactionCosts DTOs.</returns>
	Task<ICollection<ERPPartTransactionCostInformationDto>> GetAllPartTransactionCosts(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific PartTransactionCost.
	/// </summary>
	/// <param name="partTransactionCostId">The Unique Id of the PartTransactionCost to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the PartTransactionCost DTO.</returns>
	Task<ERPPartTransactionCostInformationDto> GetPartTransactionCost(Guid partTransactionCostId);

	/// <summary>
	/// Saves the provided ERP partTransactionCost.
	/// </summary>
	/// <param name="partTransactionCost">The ERP partTransactionCost to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SavePartTransactionCost(ERPPartTransactionCostDto partTransactionCost);
}
