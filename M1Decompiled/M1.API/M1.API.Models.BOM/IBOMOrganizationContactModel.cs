using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.DTOs.Core;
using M1.API.DTOs.Custom;

namespace M1.API.Models.BOM;

public interface IBOMOrganizationContactModel : IBOMBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving OrganizationContact information based on the specified OrganizationContact ID.
	/// </summary>
	/// <param name="organizationId">The ID of the Organization.</param>
	/// <param name="locationId">The ID of the Location.</param>
	/// <param name="contactId">The ID of the OrganizationContact.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetOrganizationContact(string organizationId, string locationId, string contactId);

	/// <summary>
	/// Validates the POST request for retrieving OrganizationContact information based on the specified OrganizationContact.
	/// </summary>
	/// <param name="organizationContact">The OrganizationContact details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PostOrganizationContact(BOMOrganizationContactDto organizationContact);

	/// <summary>
	/// Processes the request to retrieve all OrganizationContacts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of OrganizationContacts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a BOM response message DTO with a list of OrganizationContacts DTOs.</returns>
	Task<BOMResponseMessageDto<IList<CTMOrganizationContactDto>>> Process_GetAllOrganizationContacts(int pageSize, int pageNumber);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific OrganizationContact.
	/// </summary>
	/// <param name="organizationId">The ID of the Organization to retrieve information for.</param>
	/// <param name="locationId">The ID of the Location to retrieve information for.</param>
	/// <param name="contactId">The ID of the Contact to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a BOM response message DTO with the OrganizationContact DTO.</returns>
	Task<BOMResponseMessageDto<CTMOrganizationContactDto>> Process_GetOrganizationContact(string organizationId, string locationId, string contactId);

	/// <summary>
	/// Processes the posting of OrganizationContact.
	/// </summary>
	/// <param name="organizationContact">The OrganizationContact data transfer object (DTO) containing the details of the organizationContact to be posted.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.BOM.BOMResponseMessageDto`1" /> with the response message and the organizationContact details.</returns>
	Task<BOMResponseMessageDto<BOMOrganizationContactDto>> Process_PostOrganizationContact(BOMOrganizationContactDto organizationContact);
}
