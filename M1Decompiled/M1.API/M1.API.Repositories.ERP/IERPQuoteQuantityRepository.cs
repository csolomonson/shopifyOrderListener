using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPQuoteQuantityRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a QuoteQuantity with the specified Unique Id exists.
	/// </summary>
	/// <param name="quoteQuantityId">The Unique Id of the QuoteQuantity to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the QuoteQuantity exists or not.</returns>
	Task<bool> DoesQuoteQuantityExist(Guid quoteQuantityId);

	/// <summary>
	/// Retrieves all QuoteQuantities with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of QuoteQuantities to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of QuoteQuantities DTOs.</returns>
	Task<ICollection<ERPQuoteQuantityInformationDto>> GetAllQuoteQuantities(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific QuoteQuantity.
	/// </summary>
	/// <param name="quoteQuantityId">The Unique Id of the QuoteQuantity to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the QuoteQuantity DTO.</returns>
	Task<ERPQuoteQuantityInformationDto> GetQuoteQuantity(Guid quoteQuantityId);

	/// <summary>
	/// Saves the provided ERP quoteQuantity.
	/// </summary>
	/// <param name="quoteQuantity">The ERP quoteQuantity to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveQuoteQuantity(ERPQuoteQuantityDto quoteQuantity);
}
