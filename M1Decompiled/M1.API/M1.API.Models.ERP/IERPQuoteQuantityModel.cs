using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPQuoteQuantityModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all QuoteQuantities with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of QuoteQuantities to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllQuoteQuantities(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving QuoteQuantity information based on the specified QuoteQuantity Unique Id.
	/// </summary>
	/// <param name="quoteQuantityId">The Unique Id of the QuoteQuantity.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetQuoteQuantity(Guid quoteQuantityId);

	/// <summary>
	/// Validates the PUT request for creating or updating QuoteQuantity information based on the specified QuoteQuantity.
	/// </summary>
	/// <param name="quoteQuantity">The QuoteQuantity details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutQuoteQuantity(ERPQuoteQuantityDto quoteQuantity);

	/// <summary>
	/// Processes the request to retrieve all QuoteQuantities with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of QuoteQuantities to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of QuoteQuantities DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPQuoteQuantityDto>>> Process_GetAllQuoteQuantities(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific QuoteQuantity.
	/// </summary>
	/// <param name="quoteQuantityId">The Unique Id of the QuoteQuantity to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the QuoteQuantity DTO.</returns>
	Task<ERPResponseMessageDto<ERPQuoteQuantityDto>> Process_GetQuoteQuantity(Guid quoteQuantityId);

	/// <summary>
	/// Processes the creating or updating of a QuoteQuantity record.
	/// </summary>
	/// <param name="quoteQuantity">The QuoteQuantity data transfer object (DTO) containing the details of the QuoteQuantity to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the QuoteQuantity details.</returns>
	Task<ERPResponseMessageDto<ERPQuoteQuantityDto>> Process_PutQuoteQuantity(ERPQuoteQuantityDto quoteQuantity);

	/// <summary>
	/// Validates the request for deleting a QuoteQuantity record.
	/// </summary>
	/// <param name="quoteQuantityId">The Unique Id of the QuoteQuantity.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteQuoteQuantity(Guid quoteQuantityId);

	/// <summary>
	/// Processes the request to delete a QuoteQuantity record.
	/// </summary>
	/// <param name="quoteQuantityId">The Unique Id of the QuoteQuantity.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPQuoteQuantityDto>> Process_DeleteQuoteQuantity(Guid quoteQuantityId);
}
