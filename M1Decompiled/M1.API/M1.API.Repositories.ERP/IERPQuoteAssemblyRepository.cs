using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPQuoteAssemblyRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a QuoteAssembly with the specified Unique Id exists.
	/// </summary>
	/// <param name="quoteAssemblyId">The Unique Id of the QuoteAssembly to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the QuoteAssembly exists or not.</returns>
	Task<bool> DoesQuoteAssemblyExist(Guid quoteAssemblyId);

	/// <summary>
	/// Retrieves all QuoteAssemblies with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of QuoteAssemblies to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of QuoteAssemblies DTOs.</returns>
	Task<ICollection<ERPQuoteAssemblyInformationDto>> GetAllQuoteAssemblies(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific QuoteAssembly.
	/// </summary>
	/// <param name="quoteAssemblyId">The Unique Id of the QuoteAssembly to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the QuoteAssembly DTO.</returns>
	Task<ERPQuoteAssemblyInformationDto> GetQuoteAssembly(Guid quoteAssemblyId);

	/// <summary>
	/// Saves the provided ERP quoteAssembly.
	/// </summary>
	/// <param name="quoteAssembly">The ERP quoteAssembly to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveQuoteAssembly(ERPQuoteAssemblyDto quoteAssembly);
}
