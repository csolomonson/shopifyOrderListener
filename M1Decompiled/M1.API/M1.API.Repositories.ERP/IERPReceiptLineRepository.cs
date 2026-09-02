using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPReceiptLineRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a ReceiptLine with the specified Unique Id exists.
	/// </summary>
	/// <param name="receiptLineId">The Unique Id of the ReceiptLine to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the ReceiptLine exists or not.</returns>
	Task<bool> DoesReceiptLineExist(Guid receiptLineId);

	/// <summary>
	/// Retrieves all ReceiptLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ReceiptLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ReceiptLines DTOs.</returns>
	Task<ICollection<ERPReceiptLineInformationDto>> GetAllReceiptLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific ReceiptLine.
	/// </summary>
	/// <param name="receiptLineId">The Unique Id of the ReceiptLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the ReceiptLine DTO.</returns>
	Task<ERPReceiptLineInformationDto> GetReceiptLine(Guid receiptLineId);

	/// <summary>
	/// Saves the provided ERP receiptLine.
	/// </summary>
	/// <param name="receiptLine">The ERP receiptLine to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveReceiptLine(ERPReceiptLineDto receiptLine);
}
