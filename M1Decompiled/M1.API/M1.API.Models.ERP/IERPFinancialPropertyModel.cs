using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPFinancialPropertyModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all FinancialProperties with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of FinancialProperties to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllFinancialProperties(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving FinancialProperty information based on the specified FinancialProperty Unique Id.
	/// </summary>
	/// <param name="financialPropertyId">The Unique Id of the FinancialProperty.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetFinancialProperty(Guid financialPropertyId);

	/// <summary>
	/// Processes the request to retrieve all FinancialProperties with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of FinancialProperties to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of FinancialProperties DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPFinancialPropertyDto>>> Process_GetAllFinancialProperties(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific FinancialProperty.
	/// </summary>
	/// <param name="financialPropertyId">The Unique Id of the FinancialProperty to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the FinancialProperty DTO.</returns>
	Task<ERPResponseMessageDto<ERPFinancialPropertyDto>> Process_GetFinancialProperty(Guid financialPropertyId);
}
