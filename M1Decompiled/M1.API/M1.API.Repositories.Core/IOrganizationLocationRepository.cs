using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.DTOs.Core;
using M1.API.DTOs.Custom;

namespace M1.API.Repositories.Core;

public interface IOrganizationLocationRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a OrganizationLocation with the specified ID exists.
	/// </summary>
	/// <param name="organizationId">The ID of the Organization to check.</param>
	/// <param name="organizationLocationId">The ID of the OrganizationLocation to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the OrganizationLocation exists or not.</returns>
	Task<bool> DoesOrganizationLocationExists(string organizationId, string organizationLocationId);

	/// <summary>
	/// Retrieves all OrganizationLocation with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of OrganizationLocations to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a BOM response message DTO with a list of OrganizationLocations DTOs.</returns>
	Task<ICollection<OrganizationLocationInformationDto>> GetAllOrganizationLocations(int? pageSize = null, int? pageNumber = null);

	/// <summary>
	/// Retrieves detailed information about a specific OrganizationLocation.
	/// </summary>
	/// <param name="organizationId">The ID of the Organization to retrieve information for.</param>
	/// <param name="organizationLocationId">The ID of the OrganizationLocation to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the OrganizationLocation DTO.</returns>
	Task<OrganizationLocationInformationDto> GetOrganizationLocation(string organizationId, string organizationLocationId);

	/// <summary>
	/// Saves the provided BOM organizationLocation.
	/// </summary>
	/// <param name="organizationLocation">The BOM organizationLocation to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveOrganizationLocation(BOMOrganizationLocationDto organizationLocation);
}
