using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPRMAReceiptRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a RMAReceipt with the specified Unique Id exists.
	/// </summary>
	/// <param name="rMAReceiptId">The Unique Id of the RMAReceipt to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the RMAReceipt exists or not.</returns>
	Task<bool> DoesRMAReceiptExist(Guid rMAReceiptId);

	/// <summary>
	/// Retrieves all RMAReceipts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of RMAReceipts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of RMAReceipts DTOs.</returns>
	Task<ICollection<ERPRMAReceiptInformationDto>> GetAllRMAReceipts(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific RMAReceipt.
	/// </summary>
	/// <param name="rMAReceiptId">The Unique Id of the RMAReceipt to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the RMAReceipt DTO.</returns>
	Task<ERPRMAReceiptInformationDto> GetRMAReceipt(Guid rMAReceiptId);

	/// <summary>
	/// Saves the provided ERP rMAReceipt.
	/// </summary>
	/// <param name="rMAReceipt">The ERP rMAReceipt to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveRMAReceipt(ERPRMAReceiptDto rMAReceipt);
}
