using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPOrganizationContactGroupLinkModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all OrganizationContactGroupLinks with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of OrganizationContactGroupLinks to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllOrganizationContactGroupLinks(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving OrganizationContactGroupLink information based on the specified OrganizationContactGroupLink Unique Id.
	/// </summary>
	/// <param name="organizationContactGroupLinkId">The Unique Id of the OrganizationContactGroupLink.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetOrganizationContactGroupLink(Guid organizationContactGroupLinkId);

	/// <summary>
	/// Validates the PUT request for creating or updating OrganizationContactGroupLink information based on the specified OrganizationContactGroupLink.
	/// </summary>
	/// <param name="organizationContactGroupLink">The OrganizationContactGroupLink details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutOrganizationContactGroupLink(ERPOrganizationContactGroupLinkDto organizationContactGroupLink);

	/// <summary>
	/// Processes the request to retrieve all OrganizationContactGroupLinks with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of OrganizationContactGroupLinks to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of OrganizationContactGroupLinks DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPOrganizationContactGroupLinkDto>>> Process_GetAllOrganizationContactGroupLinks(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific OrganizationContactGroupLink.
	/// </summary>
	/// <param name="organizationContactGroupLinkId">The Unique Id of the OrganizationContactGroupLink to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the OrganizationContactGroupLink DTO.</returns>
	Task<ERPResponseMessageDto<ERPOrganizationContactGroupLinkDto>> Process_GetOrganizationContactGroupLink(Guid organizationContactGroupLinkId);

	/// <summary>
	/// Processes the creating or updating of a OrganizationContactGroupLink record.
	/// </summary>
	/// <param name="organizationContactGroupLink">The OrganizationContactGroupLink data transfer object (DTO) containing the details of the OrganizationContactGroupLink to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the OrganizationContactGroupLink details.</returns>
	Task<ERPResponseMessageDto<ERPOrganizationContactGroupLinkDto>> Process_PutOrganizationContactGroupLink(ERPOrganizationContactGroupLinkDto organizationContactGroupLink);

	/// <summary>
	/// Validates the request for deleting a OrganizationContactGroupLink record.
	/// </summary>
	/// <param name="organizationContactGroupLinkId">The Unique Id of the OrganizationContactGroupLink.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteOrganizationContactGroupLink(Guid organizationContactGroupLinkId);

	/// <summary>
	/// Processes the request to delete a OrganizationContactGroupLink record.
	/// </summary>
	/// <param name="organizationContactGroupLinkId">The Unique Id of the OrganizationContactGroupLink.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPOrganizationContactGroupLinkDto>> Process_DeleteOrganizationContactGroupLink(Guid organizationContactGroupLinkId);
}
