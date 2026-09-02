using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPAssetPoolTransactionRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a AssetPoolTransaction with the specified Unique Id exists.
	/// </summary>
	/// <param name="assetPoolTransactionId">The Unique Id of the AssetPoolTransaction to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the AssetPoolTransaction exists or not.</returns>
	Task<bool> DoesAssetPoolTransactionExist(Guid assetPoolTransactionId);

	/// <summary>
	/// Retrieves all AssetPoolTransactions with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of AssetPoolTransactions to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of AssetPoolTransactions DTOs.</returns>
	Task<ICollection<ERPAssetPoolTransactionInformationDto>> GetAllAssetPoolTransactions(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific AssetPoolTransaction.
	/// </summary>
	/// <param name="assetPoolTransactionId">The Unique Id of the AssetPoolTransaction to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the AssetPoolTransaction DTO.</returns>
	Task<ERPAssetPoolTransactionInformationDto> GetAssetPoolTransaction(Guid assetPoolTransactionId);

	/// <summary>
	/// Saves the provided ERP assetPoolTransaction.
	/// </summary>
	/// <param name="assetPoolTransaction">The ERP assetPoolTransaction to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveAssetPoolTransaction(ERPAssetPoolTransactionDto assetPoolTransaction);
}
