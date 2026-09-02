using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPAPInvoiceRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a APInvoice with the specified Unique Id exists.
	/// </summary>
	/// <param name="aPInvoiceId">The Unique Id of the APInvoice to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the APInvoice exists or not.</returns>
	Task<bool> DoesAPInvoiceExist(Guid aPInvoiceId);

	/// <summary>
	/// Retrieves all APInvoices with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of APInvoices to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of APInvoices DTOs.</returns>
	Task<ICollection<ERPAPInvoiceInformationDto>> GetAllAPInvoices(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific APInvoice.
	/// </summary>
	/// <param name="aPInvoiceId">The Unique Id of the APInvoice to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the APInvoice DTO.</returns>
	Task<ERPAPInvoiceInformationDto> GetAPInvoice(Guid aPInvoiceId);

	/// <summary>
	/// Saves the provided ERP aPInvoice.
	/// </summary>
	/// <param name="aPInvoice">The ERP aPInvoice to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveAPInvoice(ERPAPInvoiceDto aPInvoice);
}
