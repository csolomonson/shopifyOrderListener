using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.DTOs.BOM.Sales;
using M1.API.DTOs.Core;
using M1.API.DTOs.Custom.Sales;

namespace M1.API.Models.BOM.Sales;

public interface IBOMQuoteModel : IBOMBaseModel, IAPIBaseModel, IDisposable
{
	IDictionary<string, object> QuoteKeyDictionary { get; set; }

	/// <summary>
	/// Validates the request for retrieving Quote information based on the specified Quote ID.
	/// </summary>
	/// <param name="quoteId">The ID of the Quote.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetQuoteAsync(string quoteId);

	/// <summary>
	/// Validates the POST request for retrieving Quote information based on the specified Quote.
	/// </summary>
	/// <param name="quote">The Quote details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PostQuoteAsync(BOMCreateQuoteDto quote);

	/// <summary>
	/// Processes the request to retrieve all Quotes with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Quotes to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a BOM response message DTO with a list of Quotes DTOs.</returns>
	Task<BOMResponseMessageDto<IList<BOMQuoteDto>>> Process_GetAllQuotesAsync(int pageSize, int pageNumber);

	/// <summary>
	/// Processes the retrieval of quote lines based on the provided quote ID.
	/// </summary>
	/// <param name="quoteId">The ID of the quote for which to retrieve lines.</param>
	/// <returns>
	/// A task that represents the asynchronous operation. The task result contains a 
	/// <see cref="T:M1.API.DTOs.BOM.BOMResponseMessageDto`1" /> object.
	/// </returns>
	Task<BOMResponseMessageDto<CTMBOMQuoteLineDto>> Process_GetQuoteLinesAsync(string quoteId);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific Quote.
	/// </summary>
	/// <param name="quoteId">The ID of the Quote to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a BOM response message DTO with the Quote DTO.</returns>
	Task<BOMResponseMessageDto<BOMQuoteDto>> Process_GetQuoteAsync(string quoteId);

	/// <summary>
	/// Processes the posting of Quote.
	/// </summary>
	/// <param name="quote">The Quote data transfer object (DTO) containing the details of the quote to be posted.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.BOM.BOMResponseMessageDto`1" /> with the response message and the quote details.</returns>
	Task<BOMResponseMessageDto<BOMCreateQuoteDto>> Process_PostQuoteAsync(BOMCreateQuoteDto quote);
}
