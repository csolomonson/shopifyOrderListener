using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPOrganizationRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a Organization with the specified Unique Id exists.
	/// </summary>
	/// <param name="organizationId">The Unique Id of the Organization to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the Organization exists or not.</returns>
	Task<bool> DoesOrganizationExist(Guid organizationId);

	/// <summary>
	/// Retrieves all Organizations with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Organizations to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of Organizations DTOs.</returns>
	Task<ICollection<ERPOrganizationInformationDto>> GetAllOrganizations(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific Organization.
	/// </summary>
	/// <param name="organizationId">The Unique Id of the Organization to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the Organization DTO.</returns>
	Task<ERPOrganizationInformationDto> GetOrganization(Guid organizationId);

	/// <summary>
	/// Saves the provided ERP organization.
	/// </summary>
	/// <param name="organization">The ERP organization to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveOrganization(ERPOrganizationDto organization);
}
