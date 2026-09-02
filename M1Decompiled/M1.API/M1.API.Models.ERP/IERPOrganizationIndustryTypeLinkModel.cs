using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPOrganizationIndustryTypeLinkModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all OrganizationIndustryTypeLinks with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of OrganizationIndustryTypeLinks to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllOrganizationIndustryTypeLinks(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving OrganizationIndustryTypeLink information based on the specified OrganizationIndustryTypeLink Unique Id.
	/// </summary>
	/// <param name="organizationIndustryTypeLinkId">The Unique Id of the OrganizationIndustryTypeLink.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetOrganizationIndustryTypeLink(Guid organizationIndustryTypeLinkId);

	/// <summary>
	/// Validates the PUT request for creating or updating OrganizationIndustryTypeLink information based on the specified OrganizationIndustryTypeLink.
	/// </summary>
	/// <param name="organizationIndustryTypeLink">The OrganizationIndustryTypeLink details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutOrganizationIndustryTypeLink(ERPOrganizationIndustryTypeLinkDto organizationIndustryTypeLink);

	/// <summary>
	/// Processes the request to retrieve all OrganizationIndustryTypeLinks with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of OrganizationIndustryTypeLinks to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of OrganizationIndustryTypeLinks DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPOrganizationIndustryTypeLinkDto>>> Process_GetAllOrganizationIndustryTypeLinks(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific OrganizationIndustryTypeLink.
	/// </summary>
	/// <param name="organizationIndustryTypeLinkId">The Unique Id of the OrganizationIndustryTypeLink to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the OrganizationIndustryTypeLink DTO.</returns>
	Task<ERPResponseMessageDto<ERPOrganizationIndustryTypeLinkDto>> Process_GetOrganizationIndustryTypeLink(Guid organizationIndustryTypeLinkId);

	/// <summary>
	/// Processes the creating or updating of a OrganizationIndustryTypeLink record.
	/// </summary>
	/// <param name="organizationIndustryTypeLink">The OrganizationIndustryTypeLink data transfer object (DTO) containing the details of the OrganizationIndustryTypeLink to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the OrganizationIndustryTypeLink details.</returns>
	Task<ERPResponseMessageDto<ERPOrganizationIndustryTypeLinkDto>> Process_PutOrganizationIndustryTypeLink(ERPOrganizationIndustryTypeLinkDto organizationIndustryTypeLink);

	/// <summary>
	/// Validates the request for deleting a OrganizationIndustryTypeLink record.
	/// </summary>
	/// <param name="organizationIndustryTypeLinkId">The Unique Id of the OrganizationIndustryTypeLink.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteOrganizationIndustryTypeLink(Guid organizationIndustryTypeLinkId);

	/// <summary>
	/// Processes the request to delete a OrganizationIndustryTypeLink record.
	/// </summary>
	/// <param name="organizationIndustryTypeLinkId">The Unique Id of the OrganizationIndustryTypeLink.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPOrganizationIndustryTypeLinkDto>> Process_DeleteOrganizationIndustryTypeLink(Guid organizationIndustryTypeLinkId);
}
