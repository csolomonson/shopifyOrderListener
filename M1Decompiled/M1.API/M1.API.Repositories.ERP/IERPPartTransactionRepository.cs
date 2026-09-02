using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPPartTransactionRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a PartTransaction with the specified Unique Id exists.
	/// </summary>
	/// <param name="partTransactionId">The Unique Id of the PartTransaction to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the PartTransaction exists or not.</returns>
	Task<bool> DoesPartTransactionExist(Guid partTransactionId);

	/// <summary>
	/// Retrieves all PartTransactions with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartTransactions to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PartTransactions DTOs.</returns>
	Task<ICollection<ERPPartTransactionInformationDto>> GetAllPartTransactions(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific PartTransaction.
	/// </summary>
	/// <param name="partTransactionId">The Unique Id of the PartTransaction to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the PartTransaction DTO.</returns>
	Task<ERPPartTransactionInformationDto> GetPartTransaction(Guid partTransactionId);

	/// <summary>
	/// Saves the provided ERP partTransaction.
	/// </summary>
	/// <param name="partTransaction">The ERP partTransaction to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SavePartTransaction(ERPPartTransactionDto partTransaction);
}
