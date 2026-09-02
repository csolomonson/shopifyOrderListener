using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPFreightPackageLinkRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a FreightPackageLink with the specified Unique Id exists.
	/// </summary>
	/// <param name="freightPackageLinkId">The Unique Id of the FreightPackageLink to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the FreightPackageLink exists or not.</returns>
	Task<bool> DoesFreightPackageLinkExist(Guid freightPackageLinkId);

	/// <summary>
	/// Retrieves all FreightPackageLinks with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of FreightPackageLinks to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of FreightPackageLinks DTOs.</returns>
	Task<ICollection<ERPFreightPackageLinkInformationDto>> GetAllFreightPackageLinks(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific FreightPackageLink.
	/// </summary>
	/// <param name="freightPackageLinkId">The Unique Id of the FreightPackageLink to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the FreightPackageLink DTO.</returns>
	Task<ERPFreightPackageLinkInformationDto> GetFreightPackageLink(Guid freightPackageLinkId);

	/// <summary>
	/// Saves the provided ERP freightPackageLink.
	/// </summary>
	/// <param name="freightPackageLink">The ERP freightPackageLink to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveFreightPackageLink(ERPFreightPackageLinkDto freightPackageLink);
}
