using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core.Transaction;
using M1.API.DTOs.Custom;

namespace M1.API.Repositories.Core.Transaction;

public interface IMfgReceiptRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a mfg receipt with the specified ID exists.
	/// </summary>
	/// <param name="mfgReceiptId">The ID of the mfg receipt to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the receipt exists or not.</returns>
	Task<bool> DoesMfgReceiptExists(string mfgReceiptId);

	/// <summary>
	/// Retrieves all manufacturing receipts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of manufacturing receipts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a collection of manufacturing receipt information DTOs.</returns>
	Task<ICollection<MfgReceiptInformationDto>> GetAllMfgReceipts(int? pageSize = null, int? pageNumber = null);

	/// <summary>
	/// Retrieves detailed information about a specific manufacturing receipt.
	/// </summary>
	/// <param name="mfgReceiptId">The ID of the manufacturing receipt to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the manufacturing receipt DTO.</returns>
	Task<MfgReceiptDto> GetMfgReceipt(string mfgReceiptId);
}
