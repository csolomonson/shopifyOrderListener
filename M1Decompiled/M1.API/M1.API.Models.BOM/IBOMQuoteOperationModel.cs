using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.DTOs.Core;

namespace M1.API.Models.BOM;

public interface IBOMQuoteOperationModel : IBOMBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving QuoteOperation information based on the specified QuoteOperation ID.
	/// </summary>
	/// <param name="quoteId">The unique identifier for the quote.</param>
	/// <param name="quoteLineId">Optional. The unique identifier for the quote line associated with the quote.</param>
	/// <param name="quoteAssemblyId">Optional. The unique identifier for the quote assembly associated with the quote.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetQuoteOperation(string quoteId, string quoteLineId = "", string quoteAssemblyId = "");

	/// <summary>
	/// Processes the request to retrieve all QuoteOperations with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of QuoteOperations to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a BOM response message DTO with a list of QuoteOperations DTOs.</returns>
	Task<BOMResponseMessageDto<IList<BOMQuoteOperationDto>>> Process_GetAllQuoteOperations(int pageSize, int pageNumber);

	/// <summary>
	/// Retrieves a list of <see cref="T:M1.API.DTOs.BOM.BOMQuoteOperationDto" /> objects related to a specific quote.
	/// </summary>
	/// <param name="quoteId">The unique identifier for the quote.</param>
	/// <param name="quoteLineId">Optional. The unique identifier for the quote line associated with the quote.</param>
	/// <param name="quoteAssemblyId">Optional. The unique identifier for the quote assembly associated with the quote.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a BOM response message DTO with a list of QuoteOperations DTOs.</returns>
	Task<BOMResponseMessageDto<IList<BOMQuoteOperationDto>>> Process_GetQuoteOperations(string quoteId, string quoteLineId = "", string quoteAssemblyId = "");
}
