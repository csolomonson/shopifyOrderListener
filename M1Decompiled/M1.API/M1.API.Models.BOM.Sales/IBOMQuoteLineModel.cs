using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.DTOs.BOM.Sales;
using M1.API.DTOs.Core;

namespace M1.API.Models.BOM.Sales;

public interface IBOMQuoteLineModel : IBOMBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving QuoteLine information based on the specified QuoteLine ID.
	/// </summary>
	/// <param name="quoteLineId">The ID of the QuoteLine.</param>
	/// <param name="quoteId">The ID of the Quote.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetQuoteLine(string quoteId, string quoteLineId);

	/// <summary>
	/// Validates the POST request for retrieving QuoteLine information based on the specified QuoteLine.
	/// </summary>
	/// <param name="quoteLine">The QuoteLine details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PostQuoteLineAsync(BOMCreateQuoteLineDto quoteLine);

	/// <summary>
	/// Processes the request to retrieve all QuoteLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of QuoteLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a BOM response message DTO with a list of QuoteLines DTOs.</returns>
	Task<BOMResponseMessageDto<IList<BOMQuoteLineDto>>> Process_GetAllQuoteLines(int pageSize, int pageNumber);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific QuoteLine.
	/// </summary>
	/// <param name="quoteId">The ID of the Quote to retrieve information for.</param>
	/// <param name="quoteLineId">The ID of the QuoteLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a BOM response message DTO with the QuoteLine DTO.</returns>
	Task<BOMResponseMessageDto<BOMQuoteLineDto>> Process_GetQuoteLine(string quoteId, string quoteLineId);

	/// <summary>
	/// Processes the posting of QuoteLine.
	/// </summary>
	/// <param name="quoteLine">The QuoteLine data transfer object (DTO) containing the details of the quote to be posted.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.BOM.BOMResponseMessageDto`1" /> with the response message and the quote line details.</returns>
	Task<BOMResponseMessageDto<BOMCreateQuoteLineDto>> Process_PostQuoteLineAsync(BOMCreateQuoteLineDto quoteLine);
}
