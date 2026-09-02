using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPFreightPackageRateRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a FreightPackageRate with the specified Unique Id exists.
	/// </summary>
	/// <param name="freightPackageRateId">The Unique Id of the FreightPackageRate to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the FreightPackageRate exists or not.</returns>
	Task<bool> DoesFreightPackageRateExist(Guid freightPackageRateId);

	/// <summary>
	/// Retrieves all FreightPackageRates with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of FreightPackageRates to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of FreightPackageRates DTOs.</returns>
	Task<ICollection<ERPFreightPackageRateInformationDto>> GetAllFreightPackageRates(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific FreightPackageRate.
	/// </summary>
	/// <param name="freightPackageRateId">The Unique Id of the FreightPackageRate to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the FreightPackageRate DTO.</returns>
	Task<ERPFreightPackageRateInformationDto> GetFreightPackageRate(Guid freightPackageRateId);

	/// <summary>
	/// Saves the provided ERP freightPackageRate.
	/// </summary>
	/// <param name="freightPackageRate">The ERP freightPackageRate to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveFreightPackageRate(ERPFreightPackageRateDto freightPackageRate);
}
