using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPFreightPackageRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a FreightPackage with the specified Unique Id exists.
	/// </summary>
	/// <param name="freightPackageId">The Unique Id of the FreightPackage to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the FreightPackage exists or not.</returns>
	Task<bool> DoesFreightPackageExist(Guid freightPackageId);

	/// <summary>
	/// Retrieves all FreightPackages with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of FreightPackages to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of FreightPackages DTOs.</returns>
	Task<ICollection<ERPFreightPackageInformationDto>> GetAllFreightPackages(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific FreightPackage.
	/// </summary>
	/// <param name="freightPackageId">The Unique Id of the FreightPackage to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the FreightPackage DTO.</returns>
	Task<ERPFreightPackageInformationDto> GetFreightPackage(Guid freightPackageId);

	/// <summary>
	/// Saves the provided ERP freightPackage.
	/// </summary>
	/// <param name="freightPackage">The ERP freightPackage to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveFreightPackage(ERPFreightPackageDto freightPackage);
}
