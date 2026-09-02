using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.DTOs.BOM.Sales;
using M1.API.DTOs.Core;

namespace M1.API.Models.BOM;

public interface IBOMQuoteAssemblyModel : IBOMBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving QuoteAssemblies information Quote ID and QuoteLine ID if required.
	/// </summary>
	/// <param name="quoteId">The ID of the Quote.</param>
	/// <param name="quoteLineId">The ID of the Quote Line.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetQuoteAssembly(string quoteId, string quoteLineId = "");

	/// <summary>
	/// Validates the POST request for retrieving QuoteAssembly information based on the specified QuoteAssembly.
	/// </summary>
	/// <param name="quoteAssembly">The QuoteAssembly details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PostQuoteAssemblyAsync(BOMCreateQuoteAssemblyDto quoteAssembly);

	/// <summary>
	/// Processes the request to retrieve all QuoteAssemblies with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of QuoteAssemblies to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a BOM response message DTO with a list of QuoteAssemblies DTOs.</returns>
	Task<BOMResponseMessageDto<IList<BOMQuoteAssemblyDto>>> Process_GetAllQuoteAssemblies(int pageSize, int pageNumber);

	/// <summary>
	/// Processes the request to retrieve detailed information about a list QuoteAssemblies based on Quote and QuoteLine Id.
	/// </summary>
	/// <param name="quoteId">The ID of the Quote to retrieve information for.</param>
	/// <param name="quoteLineId">The ID of the QuoteLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a BOM response message DTO with a list of QuoteAssemblies DTOs.</returns>
	Task<BOMResponseMessageDto<IList<BOMQuoteAssemblyDto>>> Process_GetQuoteAssemblies(string quoteId, string quoteLineId = "");

	/// <summary>
	/// Processes the posting of QuoteAssembly.
	/// </summary>
	/// <param name="quoteAssembly">The QuoteAssembly data transfer object (DTO) containing the details of the quote assembly to be posted.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.BOM.BOMResponseMessageDto`1" /> with the response message and the quote assembly details.</returns>
	Task<BOMResponseMessageDto<BOMCreateQuoteAssemblyDto>> Process_PostQuoteAssemblyAsync(BOMCreateQuoteAssemblyDto quoteAssembly);
}
