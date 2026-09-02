using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPQuoteModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all Quotes with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Quotes to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllQuotes(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving Quote information based on the specified Quote Unique Id.
	/// </summary>
	/// <param name="quoteId">The Unique Id of the Quote.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetQuote(Guid quoteId);

	/// <summary>
	/// Validates the PUT request for creating or updating Quote information based on the specified Quote.
	/// </summary>
	/// <param name="quote">The Quote details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutQuote(ERPQuoteDto quote);

	/// <summary>
	/// Processes the request to retrieve all Quotes with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Quotes to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of Quotes DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPQuoteDto>>> Process_GetAllQuotes(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific Quote.
	/// </summary>
	/// <param name="quoteId">The Unique Id of the Quote to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the Quote DTO.</returns>
	Task<ERPResponseMessageDto<ERPQuoteDto>> Process_GetQuote(Guid quoteId);

	/// <summary>
	/// Processes the creating or updating of a Quote record.
	/// </summary>
	/// <param name="quote">The Quote data transfer object (DTO) containing the details of the Quote to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the Quote details.</returns>
	Task<ERPResponseMessageDto<ERPQuoteDto>> Process_PutQuote(ERPQuoteDto quote);

	/// <summary>
	/// Validates the request for deleting a Quote record.
	/// </summary>
	/// <param name="quoteId">The Unique Id of the Quote.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteQuote(Guid quoteId);

	/// <summary>
	/// Processes the request to delete a Quote record.
	/// </summary>
	/// <param name="quoteId">The Unique Id of the Quote.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPQuoteDto>> Process_DeleteQuote(Guid quoteId);
}
