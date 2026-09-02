using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Custom;

namespace M1.API.Repositories.Core.Transaction;

/// <summary>
/// Interface for interacting with the Receipt repository.
/// </summary>
public interface IReceiptRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a receipt with the specified ID exists.
	/// </summary>
	/// <param name="receiptId">The ID of the receipt to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the receipt exists or not.</returns>
	Task<bool> DoesReceiptExists(string receiptId);

	/// <summary>
	/// Retrieves all receipts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of receipts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a collection of receipt information DTOs.</returns>
	Task<ICollection<ReceiptInformationDto>> GetAllReceipts(int? pageSize = null, int? pageNumber = null);

	/// <summary>
	/// Retrieves detailed information about a specific receipt.
	/// </summary>
	/// <param name="receiptId">The ID of the receipt to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the receipt information DTO.</returns>
	Task<ReceiptInformationDto> GetReceiptInfo(string receiptId);

	/// <summary>
	/// Retrieves detailed line information about a specific receipt.
	/// </summary>
	/// <param name="receiptId">The ID of the receipt to retrieve line information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a list of receipt line information DTOs.</returns>
	Task<IList<ReceiptLineInformationDto>> GetReceiptLineInfo(string receiptId);
}
