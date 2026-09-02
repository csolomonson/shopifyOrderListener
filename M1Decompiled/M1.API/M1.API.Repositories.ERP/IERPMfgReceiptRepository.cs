using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPMfgReceiptRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a MfgReceipt with the specified Unique Id exists.
	/// </summary>
	/// <param name="mfgReceiptId">The Unique Id of the MfgReceipt to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the MfgReceipt exists or not.</returns>
	Task<bool> DoesMfgReceiptExist(Guid mfgReceiptId);

	/// <summary>
	/// Retrieves all MfgReceipts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of MfgReceipts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of MfgReceipts DTOs.</returns>
	Task<ICollection<ERPMfgReceiptInformationDto>> GetAllMfgReceipts(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific MfgReceipt.
	/// </summary>
	/// <param name="mfgReceiptId">The Unique Id of the MfgReceipt to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the MfgReceipt DTO.</returns>
	Task<ERPMfgReceiptInformationDto> GetMfgReceipt(Guid mfgReceiptId);

	/// <summary>
	/// Saves the provided ERP mfgReceipt.
	/// </summary>
	/// <param name="mfgReceipt">The ERP mfgReceipt to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveMfgReceipt(ERPMfgReceiptDto mfgReceipt);
}
