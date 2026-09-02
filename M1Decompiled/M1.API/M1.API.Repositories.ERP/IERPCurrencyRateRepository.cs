using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPCurrencyRateRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a CurrencyRate with the specified Unique Id exists.
	/// </summary>
	/// <param name="currencyRateId">The Unique Id of the CurrencyRate to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the CurrencyRate exists or not.</returns>
	Task<bool> DoesCurrencyRateExist(Guid currencyRateId);

	/// <summary>
	/// Retrieves all CurrencyRates with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of CurrencyRates to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of CurrencyRates DTOs.</returns>
	Task<ICollection<ERPCurrencyRateInformationDto>> GetAllCurrencyRates(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific CurrencyRate.
	/// </summary>
	/// <param name="currencyRateId">The Unique Id of the CurrencyRate to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the CurrencyRate DTO.</returns>
	Task<ERPCurrencyRateInformationDto> GetCurrencyRate(Guid currencyRateId);

	/// <summary>
	/// Saves the provided ERP currencyRate.
	/// </summary>
	/// <param name="currencyRate">The ERP currencyRate to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveCurrencyRate(ERPCurrencyRateDto currencyRate);
}
