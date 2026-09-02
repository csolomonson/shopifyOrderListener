using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPLotNumberTransactionRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a LotNumberTransaction with the specified Unique Id exists.
	/// </summary>
	/// <param name="lotNumberTransactionId">The Unique Id of the LotNumberTransaction to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the LotNumberTransaction exists or not.</returns>
	Task<bool> DoesLotNumberTransactionExist(Guid lotNumberTransactionId);

	/// <summary>
	/// Retrieves all LotNumberTransactions with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of LotNumberTransactions to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of LotNumberTransactions DTOs.</returns>
	Task<ICollection<ERPLotNumberTransactionInformationDto>> GetAllLotNumberTransactions(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific LotNumberTransaction.
	/// </summary>
	/// <param name="lotNumberTransactionId">The Unique Id of the LotNumberTransaction to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the LotNumberTransaction DTO.</returns>
	Task<ERPLotNumberTransactionInformationDto> GetLotNumberTransaction(Guid lotNumberTransactionId);

	/// <summary>
	/// Saves the provided ERP lotNumberTransaction.
	/// </summary>
	/// <param name="lotNumberTransaction">The ERP lotNumberTransaction to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveLotNumberTransaction(ERPLotNumberTransactionDto lotNumberTransaction);
}
