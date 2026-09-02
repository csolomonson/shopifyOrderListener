using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.DTOs.Core;
using M1.API.DTOs.Custom;

namespace M1.API.Repositories.Core;

public interface IOrganizationContactRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a OrganizationContact with the specified ID exists.
	/// </summary>
	/// <param name="organizationId">The ID of the Organization to check.</param>
	/// <param name="locationId">The ID of the Location to check.</param>
	/// <param name="contactId">The ID of the Contact to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the OrganizationContact exists or not.</returns>
	Task<bool> DoesOrganizationContactExists(string organizationId, string locationId, string contactId);

	/// <summary>
	/// Retrieves all OrganizationContact with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of OrganizationContacts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a BOM response message DTO with a list of OrganizationContacts DTOs.</returns>
	Task<ICollection<OrganizationContactInformationDto>> GetAllOrganizationContacts(int? pageSize = null, int? pageNumber = null);

	/// <summary>
	/// Retrieves detailed information about a specific OrganizationContact.
	/// </summary>
	/// <param name="organizationId">The ID of the Organization to retrieve information for.</param>
	/// <param name="locationId">The ID of the Location to retrieve information for.</param>
	/// <param name="contactId">The ID of the Contact to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the OrganizationContact DTO.</returns>
	Task<OrganizationContactInformationDto> GetOrganizationContact(string organizationId, string locationId, string contactId);

	/// <summary>
	/// Saves the provided BOM organizationContact.
	/// </summary>
	/// <param name="organizationContact">The BOM organizationContact to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveOrganizationContact(BOMOrganizationContactDto organizationContact);
}
