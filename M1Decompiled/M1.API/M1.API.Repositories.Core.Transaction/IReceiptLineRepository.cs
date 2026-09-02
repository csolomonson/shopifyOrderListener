using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Custom;

namespace M1.API.Repositories.Core.Transaction;

public interface IReceiptLineRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Retrieves detailed information about a specific receipt line associated with the given part ID.
	/// </summary>
	/// <param name="partId">The ID of the part associated with the receipt line.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the receipt line DTO.</returns>
	Task<ReceiptLineDto> GetReceiptLine(string partId);

	/// <summary>
	/// Retrieves all receipt lines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of receipt lines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a collection of receipt line DTOs.</returns>
	Task<ICollection<ReceiptLineDto>> GetAllReceiptLines(int? pageSize = null, int? pageNumber = null);
}
