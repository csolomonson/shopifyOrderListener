using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPGLFiscalYearOpeningBalanceModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all GLFiscalYearOpeningBalances with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of GLFiscalYearOpeningBalances to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllGLFiscalYearOpeningBalances(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving GLFiscalYearOpeningBalance information based on the specified GLFiscalYearOpeningBalance Unique Id.
	/// </summary>
	/// <param name="gLFiscalYearOpeningBalanceId">The Unique Id of the GLFiscalYearOpeningBalance.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetGLFiscalYearOpeningBalance(Guid gLFiscalYearOpeningBalanceId);

	/// <summary>
	/// Processes the request to retrieve all GLFiscalYearOpeningBalances with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of GLFiscalYearOpeningBalances to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of GLFiscalYearOpeningBalances DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPGLFiscalYearOpeningBalanceDto>>> Process_GetAllGLFiscalYearOpeningBalances(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific GLFiscalYearOpeningBalance.
	/// </summary>
	/// <param name="gLFiscalYearOpeningBalanceId">The Unique Id of the GLFiscalYearOpeningBalance to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the GLFiscalYearOpeningBalance DTO.</returns>
	Task<ERPResponseMessageDto<ERPGLFiscalYearOpeningBalanceDto>> Process_GetGLFiscalYearOpeningBalance(Guid gLFiscalYearOpeningBalanceId);
}
