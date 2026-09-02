using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPCurrencyRateLineRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a CurrencyRateLine with the specified Unique Id exists.
	/// </summary>
	/// <param name="currencyRateLineId">The Unique Id of the CurrencyRateLine to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the CurrencyRateLine exists or not.</returns>
	Task<bool> DoesCurrencyRateLineExist(Guid currencyRateLineId);

	/// <summary>
	/// Retrieves all CurrencyRateLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of CurrencyRateLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of CurrencyRateLines DTOs.</returns>
	Task<ICollection<ERPCurrencyRateLineInformationDto>> GetAllCurrencyRateLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific CurrencyRateLine.
	/// </summary>
	/// <param name="currencyRateLineId">The Unique Id of the CurrencyRateLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the CurrencyRateLine DTO.</returns>
	Task<ERPCurrencyRateLineInformationDto> GetCurrencyRateLine(Guid currencyRateLineId);

	/// <summary>
	/// Saves the provided ERP currencyRateLine.
	/// </summary>
	/// <param name="currencyRateLine">The ERP currencyRateLine to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveCurrencyRateLine(ERPCurrencyRateLineDto currencyRateLine);
}
