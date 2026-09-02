using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.DTOs.BOM.Sales;
using M1.API.DTOs.Core;

namespace M1.API.Models.BOM;

public interface IBOMQuoteMaterialModel : IBOMBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving QuoteMaterial information based on Quote ID.
	/// </summary>
	/// <param name="quoteId">The ID of the Quote.</param>
	/// <param name="quoteLineId">The ID of the QuoteLine.</param>
	/// <param name="quoteAssemblyId">The ID of the QuoteAssembly.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetQuoteMaterialsAsync(string quoteId, string quoteLineId = "", string quoteAssemblyId = "");

	/// <summary>
	/// Validates the POST request for retrieving QuoteMaterial information based on the specified Quote/QuoteLine/QuoteAssembly.
	/// </summary>
	/// <param name="quoteMaterial">The QuoteMaterial details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PostQuoteMaterialAsync(BOMCreateQuoteMaterialDto quoteMaterial);

	/// <summary>
	/// Processes the request to retrieve all QuoteMaterials with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of QuoteMaterials to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a BOM response message DTO with a list of QuoteMaterials DTOs.</returns>
	Task<BOMResponseMessageDto<IList<BOMQuoteMaterialDto>>> Process_GetAllQuoteMaterials(int pageSize, int pageNumber);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific Quote.
	/// </summary>
	/// <param name="quoteId">The ID of the Quote to retrieve information for.</param>
	/// <param name="quoteLineId">The ID of the QuoteLine to retrieve information for.</param>
	/// <param name="quoteAssemblyId">The ID of the QuoteAssembly to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a BOM response message DTO with a list of QuoteMaterials DTOs.</returns>
	Task<BOMResponseMessageDto<IList<BOMQuoteMaterialDto>>> Process_GetQuoteMaterialsAsync(string quoteId, string quoteLineId = "", string quoteAssemblyId = "");

	/// <summary>
	/// Processes the posting of QuoteMaterial.
	/// </summary>
	/// <param name="quoteMaterial">The QuoteMaterial data transfer object (DTO) containing the details of the quote material to be posted.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.BOM.BOMResponseMessageDto`1" /> with the response message and the quote material details.</returns>
	Task<BOMResponseMessageDto<BOMCreateQuoteMaterialDto>> Process_PostQuoteMaterialAsync(BOMCreateQuoteMaterialDto quoteMaterial);
}
