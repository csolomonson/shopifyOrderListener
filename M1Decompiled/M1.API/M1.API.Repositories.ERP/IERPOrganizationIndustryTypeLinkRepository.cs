using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPOrganizationIndustryTypeLinkRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a OrganizationIndustryTypeLink with the specified Unique Id exists.
	/// </summary>
	/// <param name="organizationIndustryTypeLinkId">The Unique Id of the OrganizationIndustryTypeLink to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the OrganizationIndustryTypeLink exists or not.</returns>
	Task<bool> DoesOrganizationIndustryTypeLinkExist(Guid organizationIndustryTypeLinkId);

	/// <summary>
	/// Retrieves all OrganizationIndustryTypeLinks with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of OrganizationIndustryTypeLinks to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of OrganizationIndustryTypeLinks DTOs.</returns>
	Task<ICollection<ERPOrganizationIndustryTypeLinkInformationDto>> GetAllOrganizationIndustryTypeLinks(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific OrganizationIndustryTypeLink.
	/// </summary>
	/// <param name="organizationIndustryTypeLinkId">The Unique Id of the OrganizationIndustryTypeLink to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the OrganizationIndustryTypeLink DTO.</returns>
	Task<ERPOrganizationIndustryTypeLinkInformationDto> GetOrganizationIndustryTypeLink(Guid organizationIndustryTypeLinkId);

	/// <summary>
	/// Saves the provided ERP organizationIndustryTypeLink.
	/// </summary>
	/// <param name="organizationIndustryTypeLink">The ERP organizationIndustryTypeLink to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveOrganizationIndustryTypeLink(ERPOrganizationIndustryTypeLinkDto organizationIndustryTypeLink);
}
