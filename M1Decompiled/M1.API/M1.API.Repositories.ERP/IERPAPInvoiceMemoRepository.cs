using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPAPInvoiceMemoRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a APInvoiceMemo with the specified Unique Id exists.
	/// </summary>
	/// <param name="aPInvoiceMemoId">The Unique Id of the APInvoiceMemo to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the APInvoiceMemo exists or not.</returns>
	Task<bool> DoesAPInvoiceMemoExist(Guid aPInvoiceMemoId);

	/// <summary>
	/// Retrieves all APInvoiceMemos with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of APInvoiceMemos to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of APInvoiceMemos DTOs.</returns>
	Task<ICollection<ERPAPInvoiceMemoInformationDto>> GetAllAPInvoiceMemos(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific APInvoiceMemo.
	/// </summary>
	/// <param name="aPInvoiceMemoId">The Unique Id of the APInvoiceMemo to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the APInvoiceMemo DTO.</returns>
	Task<ERPAPInvoiceMemoInformationDto> GetAPInvoiceMemo(Guid aPInvoiceMemoId);

	/// <summary>
	/// Saves the provided ERP aPInvoiceMemo.
	/// </summary>
	/// <param name="aPInvoiceMemo">The ERP aPInvoiceMemo to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveAPInvoiceMemo(ERPAPInvoiceMemoDto aPInvoiceMemo);
}
