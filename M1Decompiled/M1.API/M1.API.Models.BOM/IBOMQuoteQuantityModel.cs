using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.DTOs.Core;

namespace M1.API.Models.BOM;

public interface IBOMQuoteQuantityModel : IBOMBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving QuoteQuantity information based on the specified Quote ID and QuoteLine ID if required.
	/// </summary>
	/// <param name="quoteId">The ID of the Quote.</param>
	/// <param name="quoteLineId">The ID of the Quote Line.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetQuoteQuantity(string quoteId, string quoteLineId = "");

	/// <summary>
	/// Processes the request to retrieve all QuoteQuantities with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of QuoteQuantities to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a BOM response message DTO with a list of QuoteQuantities DTOs.</returns>
	Task<BOMResponseMessageDto<IList<BOMQuoteQuantityDto>>> Process_GetAllQuoteQuantities(int pageSize, int pageNumber);

	/// <summary>
	/// Processes the request to retrieve detailed information about all QuoteQuantities based on specific Quote and QuoteLine Id.
	/// </summary>
	/// <param name="quoteId">The ID of the Quote to retrieve information for.</param>
	/// <param name="quoteLineId">The ID of the QuoteLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a BOM response message DTO with a list of QuoteQuantities DTOs.</returns>
	Task<BOMResponseMessageDto<IList<BOMQuoteQuantityDto>>> Process_GetQuoteQuantities(string quoteId, string quoteLineId = "");
}
