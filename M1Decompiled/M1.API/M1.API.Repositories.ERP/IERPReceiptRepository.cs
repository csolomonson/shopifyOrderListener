using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPReceiptRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a Receipt with the specified Unique Id exists.
	/// </summary>
	/// <param name="receiptId">The Unique Id of the Receipt to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the Receipt exists or not.</returns>
	Task<bool> DoesReceiptExist(Guid receiptId);

	/// <summary>
	/// Retrieves all Receipts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Receipts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of Receipts DTOs.</returns>
	Task<ICollection<ERPReceiptInformationDto>> GetAllReceipts(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific Receipt.
	/// </summary>
	/// <param name="receiptId">The Unique Id of the Receipt to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the Receipt DTO.</returns>
	Task<ERPReceiptInformationDto> GetReceipt(Guid receiptId);

	/// <summary>
	/// Saves the provided ERP receipt.
	/// </summary>
	/// <param name="receipt">The ERP receipt to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveReceipt(ERPReceiptDto receipt);
}
