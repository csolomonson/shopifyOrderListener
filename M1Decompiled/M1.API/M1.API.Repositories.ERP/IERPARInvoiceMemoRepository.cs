using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPARInvoiceMemoRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a ARInvoiceMemo with the specified Unique Id exists.
	/// </summary>
	/// <param name="aRInvoiceMemoId">The Unique Id of the ARInvoiceMemo to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the ARInvoiceMemo exists or not.</returns>
	Task<bool> DoesARInvoiceMemoExist(Guid aRInvoiceMemoId);

	/// <summary>
	/// Retrieves all ARInvoiceMemos with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ARInvoiceMemos to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ARInvoiceMemos DTOs.</returns>
	Task<ICollection<ERPARInvoiceMemoInformationDto>> GetAllARInvoiceMemos(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific ARInvoiceMemo.
	/// </summary>
	/// <param name="aRInvoiceMemoId">The Unique Id of the ARInvoiceMemo to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the ARInvoiceMemo DTO.</returns>
	Task<ERPARInvoiceMemoInformationDto> GetARInvoiceMemo(Guid aRInvoiceMemoId);

	/// <summary>
	/// Saves the provided ERP aRInvoiceMemo.
	/// </summary>
	/// <param name="aRInvoiceMemo">The ERP aRInvoiceMemo to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveARInvoiceMemo(ERPARInvoiceMemoDto aRInvoiceMemo);
}
