using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPOrganizationLocationRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a OrganizationLocation with the specified Unique Id exists.
	/// </summary>
	/// <param name="organizationLocationId">The Unique Id of the OrganizationLocation to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the OrganizationLocation exists or not.</returns>
	Task<bool> DoesOrganizationLocationExist(Guid organizationLocationId);

	/// <summary>
	/// Retrieves all OrganizationLocations with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of OrganizationLocations to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of OrganizationLocations DTOs.</returns>
	Task<ICollection<ERPOrganizationLocationInformationDto>> GetAllOrganizationLocations(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific OrganizationLocation.
	/// </summary>
	/// <param name="organizationLocationId">The Unique Id of the OrganizationLocation to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the OrganizationLocation DTO.</returns>
	Task<ERPOrganizationLocationInformationDto> GetOrganizationLocation(Guid organizationLocationId);

	/// <summary>
	/// Saves the provided ERP organizationLocation.
	/// </summary>
	/// <param name="organizationLocation">The ERP organizationLocation to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveOrganizationLocation(ERPOrganizationLocationDto organizationLocation);
}
