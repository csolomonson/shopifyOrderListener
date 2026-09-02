using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPARInvoiceSalesPersonRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a ARInvoiceSalesPerson with the specified Unique Id exists.
	/// </summary>
	/// <param name="aRInvoiceSalesPersonId">The Unique Id of the ARInvoiceSalesPerson to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the ARInvoiceSalesPerson exists or not.</returns>
	Task<bool> DoesARInvoiceSalesPersonExist(Guid aRInvoiceSalesPersonId);

	/// <summary>
	/// Retrieves all ARInvoiceSalesPeople with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ARInvoiceSalesPeople to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ARInvoiceSalesPeople DTOs.</returns>
	Task<ICollection<ERPARInvoiceSalesPersonInformationDto>> GetAllARInvoiceSalesPeople(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific ARInvoiceSalesPerson.
	/// </summary>
	/// <param name="aRInvoiceSalesPersonId">The Unique Id of the ARInvoiceSalesPerson to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the ARInvoiceSalesPerson DTO.</returns>
	Task<ERPARInvoiceSalesPersonInformationDto> GetARInvoiceSalesPerson(Guid aRInvoiceSalesPersonId);

	/// <summary>
	/// Saves the provided ERP aRInvoiceSalesPerson.
	/// </summary>
	/// <param name="aRInvoiceSalesPerson">The ERP aRInvoiceSalesPerson to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveARInvoiceSalesPerson(ERPARInvoiceSalesPersonDto aRInvoiceSalesPerson);
}
