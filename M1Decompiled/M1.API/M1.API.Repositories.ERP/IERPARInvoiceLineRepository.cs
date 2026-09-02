using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPARInvoiceLineRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a ARInvoiceLine with the specified Unique Id exists.
	/// </summary>
	/// <param name="aRInvoiceLineId">The Unique Id of the ARInvoiceLine to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the ARInvoiceLine exists or not.</returns>
	Task<bool> DoesARInvoiceLineExist(Guid aRInvoiceLineId);

	/// <summary>
	/// Retrieves all ARInvoiceLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ARInvoiceLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ARInvoiceLines DTOs.</returns>
	Task<ICollection<ERPARInvoiceLineInformationDto>> GetAllARInvoiceLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific ARInvoiceLine.
	/// </summary>
	/// <param name="aRInvoiceLineId">The Unique Id of the ARInvoiceLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the ARInvoiceLine DTO.</returns>
	Task<ERPARInvoiceLineInformationDto> GetARInvoiceLine(Guid aRInvoiceLineId);

	/// <summary>
	/// Saves the provided ERP aRInvoiceLine.
	/// </summary>
	/// <param name="aRInvoiceLine">The ERP aRInvoiceLine to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveARInvoiceLine(ERPARInvoiceLineDto aRInvoiceLine);
}
