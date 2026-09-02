using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPOrganizationContactRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a OrganizationContact with the specified Unique Id exists.
	/// </summary>
	/// <param name="organizationContactId">The Unique Id of the OrganizationContact to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the OrganizationContact exists or not.</returns>
	Task<bool> DoesOrganizationContactExist(Guid organizationContactId);

	/// <summary>
	/// Retrieves all OrganizationContacts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of OrganizationContacts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of OrganizationContacts DTOs.</returns>
	Task<ICollection<ERPOrganizationContactInformationDto>> GetAllOrganizationContacts(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific OrganizationContact.
	/// </summary>
	/// <param name="organizationContactId">The Unique Id of the OrganizationContact to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the OrganizationContact DTO.</returns>
	Task<ERPOrganizationContactInformationDto> GetOrganizationContact(Guid organizationContactId);

	/// <summary>
	/// Saves the provided ERP organizationContact.
	/// </summary>
	/// <param name="organizationContact">The ERP organizationContact to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveOrganizationContact(ERPOrganizationContactDto organizationContact);
}
