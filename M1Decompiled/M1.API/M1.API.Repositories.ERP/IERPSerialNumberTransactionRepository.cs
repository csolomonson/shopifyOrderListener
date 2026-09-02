using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPSerialNumberTransactionRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a SerialNumberTransaction with the specified Unique Id exists.
	/// </summary>
	/// <param name="serialNumberTransactionId">The Unique Id of the SerialNumberTransaction to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the SerialNumberTransaction exists or not.</returns>
	Task<bool> DoesSerialNumberTransactionExist(Guid serialNumberTransactionId);

	/// <summary>
	/// Retrieves all SerialNumberTransactions with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of SerialNumberTransactions to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of SerialNumberTransactions DTOs.</returns>
	Task<ICollection<ERPSerialNumberTransactionInformationDto>> GetAllSerialNumberTransactions(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific SerialNumberTransaction.
	/// </summary>
	/// <param name="serialNumberTransactionId">The Unique Id of the SerialNumberTransaction to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the SerialNumberTransaction DTO.</returns>
	Task<ERPSerialNumberTransactionInformationDto> GetSerialNumberTransaction(Guid serialNumberTransactionId);

	/// <summary>
	/// Saves the provided ERP serialNumberTransaction.
	/// </summary>
	/// <param name="serialNumberTransaction">The ERP serialNumberTransaction to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveSerialNumberTransaction(ERPSerialNumberTransactionDto serialNumberTransaction);
}
