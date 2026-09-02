using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPOrganizationLocationModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all OrganizationLocations with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of OrganizationLocations to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllOrganizationLocations(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving OrganizationLocation information based on the specified OrganizationLocation Unique Id.
	/// </summary>
	/// <param name="organizationLocationId">The Unique Id of the OrganizationLocation.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetOrganizationLocation(Guid organizationLocationId);

	/// <summary>
	/// Validates the PUT request for creating or updating OrganizationLocation information based on the specified OrganizationLocation.
	/// </summary>
	/// <param name="organizationLocation">The OrganizationLocation details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutOrganizationLocation(ERPOrganizationLocationDto organizationLocation);

	/// <summary>
	/// Processes the request to retrieve all OrganizationLocations with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of OrganizationLocations to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of OrganizationLocations DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPOrganizationLocationDto>>> Process_GetAllOrganizationLocations(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific OrganizationLocation.
	/// </summary>
	/// <param name="organizationLocationId">The Unique Id of the OrganizationLocation to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the OrganizationLocation DTO.</returns>
	Task<ERPResponseMessageDto<ERPOrganizationLocationDto>> Process_GetOrganizationLocation(Guid organizationLocationId);

	/// <summary>
	/// Processes the creating or updating of a OrganizationLocation record.
	/// </summary>
	/// <param name="organizationLocation">The OrganizationLocation data transfer object (DTO) containing the details of the OrganizationLocation to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the OrganizationLocation details.</returns>
	Task<ERPResponseMessageDto<ERPOrganizationLocationDto>> Process_PutOrganizationLocation(ERPOrganizationLocationDto organizationLocation);

	/// <summary>
	/// Validates the request for deleting a OrganizationLocation record.
	/// </summary>
	/// <param name="organizationLocationId">The Unique Id of the OrganizationLocation.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteOrganizationLocation(Guid organizationLocationId);

	/// <summary>
	/// Processes the request to delete a OrganizationLocation record.
	/// </summary>
	/// <param name="organizationLocationId">The Unique Id of the OrganizationLocation.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPOrganizationLocationDto>> Process_DeleteOrganizationLocation(Guid organizationLocationId);
}
