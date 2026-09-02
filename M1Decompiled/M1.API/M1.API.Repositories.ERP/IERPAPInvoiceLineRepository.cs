using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPAPInvoiceLineRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a APInvoiceLine with the specified Unique Id exists.
	/// </summary>
	/// <param name="aPInvoiceLineId">The Unique Id of the APInvoiceLine to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the APInvoiceLine exists or not.</returns>
	Task<bool> DoesAPInvoiceLineExist(Guid aPInvoiceLineId);

	/// <summary>
	/// Retrieves all APInvoiceLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of APInvoiceLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of APInvoiceLines DTOs.</returns>
	Task<ICollection<ERPAPInvoiceLineInformationDto>> GetAllAPInvoiceLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific APInvoiceLine.
	/// </summary>
	/// <param name="aPInvoiceLineId">The Unique Id of the APInvoiceLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the APInvoiceLine DTO.</returns>
	Task<ERPAPInvoiceLineInformationDto> GetAPInvoiceLine(Guid aPInvoiceLineId);

	/// <summary>
	/// Saves the provided ERP aPInvoiceLine.
	/// </summary>
	/// <param name="aPInvoiceLine">The ERP aPInvoiceLine to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveAPInvoiceLine(ERPAPInvoiceLineDto aPInvoiceLine);
}
