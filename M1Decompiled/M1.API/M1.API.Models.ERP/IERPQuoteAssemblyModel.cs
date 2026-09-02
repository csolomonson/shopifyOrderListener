using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPQuoteAssemblyModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all QuoteAssemblies with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of QuoteAssemblies to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllQuoteAssemblies(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving QuoteAssembly information based on the specified QuoteAssembly Unique Id.
	/// </summary>
	/// <param name="quoteAssemblyId">The Unique Id of the QuoteAssembly.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetQuoteAssembly(Guid quoteAssemblyId);

	/// <summary>
	/// Validates the PUT request for creating or updating QuoteAssembly information based on the specified QuoteAssembly.
	/// </summary>
	/// <param name="quoteAssembly">The QuoteAssembly details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutQuoteAssembly(ERPQuoteAssemblyDto quoteAssembly);

	/// <summary>
	/// Processes the request to retrieve all QuoteAssemblies with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of QuoteAssemblies to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of QuoteAssemblies DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPQuoteAssemblyDto>>> Process_GetAllQuoteAssemblies(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific QuoteAssembly.
	/// </summary>
	/// <param name="quoteAssemblyId">The Unique Id of the QuoteAssembly to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the QuoteAssembly DTO.</returns>
	Task<ERPResponseMessageDto<ERPQuoteAssemblyDto>> Process_GetQuoteAssembly(Guid quoteAssemblyId);

	/// <summary>
	/// Processes the creating or updating of a QuoteAssembly record.
	/// </summary>
	/// <param name="quoteAssembly">The QuoteAssembly data transfer object (DTO) containing the details of the QuoteAssembly to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the QuoteAssembly details.</returns>
	Task<ERPResponseMessageDto<ERPQuoteAssemblyDto>> Process_PutQuoteAssembly(ERPQuoteAssemblyDto quoteAssembly);

	/// <summary>
	/// Validates the request for deleting a QuoteAssembly record.
	/// </summary>
	/// <param name="quoteAssemblyId">The Unique Id of the QuoteAssembly.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteQuoteAssembly(Guid quoteAssemblyId);

	/// <summary>
	/// Processes the request to delete a QuoteAssembly record.
	/// </summary>
	/// <param name="quoteAssemblyId">The Unique Id of the QuoteAssembly.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPQuoteAssemblyDto>> Process_DeleteQuoteAssembly(Guid quoteAssemblyId);
}
