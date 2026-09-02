using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPOrganizationContactModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all OrganizationContacts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of OrganizationContacts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllOrganizationContacts(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving OrganizationContact information based on the specified OrganizationContact Unique Id.
	/// </summary>
	/// <param name="organizationContactId">The Unique Id of the OrganizationContact.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetOrganizationContact(Guid organizationContactId);

	/// <summary>
	/// Validates the PUT request for creating or updating OrganizationContact information based on the specified OrganizationContact.
	/// </summary>
	/// <param name="organizationContact">The OrganizationContact details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutOrganizationContact(ERPOrganizationContactDto organizationContact);

	/// <summary>
	/// Processes the request to retrieve all OrganizationContacts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of OrganizationContacts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of OrganizationContacts DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPOrganizationContactDto>>> Process_GetAllOrganizationContacts(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific OrganizationContact.
	/// </summary>
	/// <param name="organizationContactId">The Unique Id of the OrganizationContact to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the OrganizationContact DTO.</returns>
	Task<ERPResponseMessageDto<ERPOrganizationContactDto>> Process_GetOrganizationContact(Guid organizationContactId);

	/// <summary>
	/// Processes the creating or updating of a OrganizationContact record.
	/// </summary>
	/// <param name="organizationContact">The OrganizationContact data transfer object (DTO) containing the details of the OrganizationContact to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the OrganizationContact details.</returns>
	Task<ERPResponseMessageDto<ERPOrganizationContactDto>> Process_PutOrganizationContact(ERPOrganizationContactDto organizationContact);

	/// <summary>
	/// Validates the request for deleting a OrganizationContact record.
	/// </summary>
	/// <param name="organizationContactId">The Unique Id of the OrganizationContact.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteOrganizationContact(Guid organizationContactId);

	/// <summary>
	/// Processes the request to delete a OrganizationContact record.
	/// </summary>
	/// <param name="organizationContactId">The Unique Id of the OrganizationContact.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPOrganizationContactDto>> Process_DeleteOrganizationContact(Guid organizationContactId);
}
