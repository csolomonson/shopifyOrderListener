using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.DTOs.BOM.Sales;
using M1.API.DTOs.Core;

namespace M1.API.Repositories.Core;

public interface IQuoteAssemblyRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a QuoteAssembly with the specified ID exists.
	/// </summary>
	/// <param name="quoteAssemblyId">The ID of the QuoteAssembly to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the QuoteAssembly exists or not.</returns>
	Task<bool> DoesQuoteAssemblyExist(string quoteAssemblyId);

	/// <summary>
	/// Checks if a QuoteAssembly with the specified QuoteId, QuoteLineId and QuoteAssemblyId exists.
	/// </summary>
	/// <param name="quoteId">The ID of the Quote to check.</param>
	/// <param name="quoteLineId">The ID of the QuoteLine to check.</param>
	/// <param name="quoteAssemblyId">The ID of the QuoteAssembly to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the QuoteAssembly exists or not.</returns>
	Task<bool> DoesQuoteAssemblyExist(string quoteId, string quoteLineId, string quoteAssemblyId);

	/// <summary>
	/// Retrieves all QuoteAssembly with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of QuoteAssemblies to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a BOM response message DTO with a list of QuoteAssemblies DTOs.</returns>
	Task<ICollection<BOMQuoteAssemblyDto>> GetAllQuoteAssemblies(int? pageSize = null, int? pageNumber = null);

	/// <summary>
	/// Retrieves a list of QuoteAssemblies based on Quote Id.
	/// </summary>
	/// <param name="quoteId">The ID of the Quote to retrieve all the quote assemblies for.</param>
	/// <param name="quoteLineId">The ID of the QuoteLine to retrieve all the quote assemblies for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a BOM response message DTO with a list of QuoteAssemblies DTOs for a giving Quote and QuoteLine Id.</returns>
	Task<IList<BOMQuoteAssemblyDto>> GetQuoteAssemblies(string quoteId, string quoteLineId);

	/// <summary>
	/// Saves the provided BOM quoteAssembly.
	/// </summary>
	/// <param name="quoteAssembly">The BOM quote assembly to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveQuoteAssemblyAsync(BOMCreateQuoteAssemblyDto quoteAssembly);
}
