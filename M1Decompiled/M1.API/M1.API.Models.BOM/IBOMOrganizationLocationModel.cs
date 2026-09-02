using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.DTOs.Core;
using M1.API.DTOs.Custom;

namespace M1.API.Models.BOM;

public interface IBOMOrganizationLocationModel : IBOMBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving OrganizationLocation information based on the specified OrganizationLocation ID.
	/// </summary>
	/// <param name="organizationId">The ID of the Organization.</param>
	/// <param name="organizationLocationId">The ID of the OrganizationLocation.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetOrganizationLocation(string organizationId, string organizationLocationId);

	/// <summary>
	/// Validates the POST request for retrieving OrganizationLocation information based on the specified OrganizationLocation.
	/// </summary>
	/// <param name="organizationLocation">The OrganizationLocation details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PostOrganizationLocation(BOMOrganizationLocationDto organizationLocation);

	/// <summary>
	/// Processes the request to retrieve all OrganizationLocations with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of OrganizationLocations to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a BOM response message DTO with a list of OrganizationLocations DTOs.</returns>
	Task<BOMResponseMessageDto<IList<CTMOrganizationLocationDto>>> Process_GetAllOrganizationLocations(int pageSize, int pageNumber);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific OrganizationLocation.
	/// </summary>
	/// <param name="organizationId">The ID of the Organization to retrieve information for.</param>
	/// <param name="organizationLocationId">The ID of the OrganizationLocation to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a BOM response message DTO with the OrganizationLocation DTO.</returns>
	Task<BOMResponseMessageDto<CTMOrganizationLocationDto>> Process_GetOrganizationLocation(string organizationId, string organizationLocationId);

	/// <summary>
	/// Processes the posting of OrganizationLocation.
	/// </summary>
	/// <param name="organizationLocation">The OrganizationLocation data transfer object (DTO) containing the details of the organizationLocation to be posted.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.BOM.BOMResponseMessageDto`1" /> with the response message and the organizationLocation details.</returns>
	Task<BOMResponseMessageDto<BOMOrganizationLocationDto>> Process_PostOrganizationLocation(BOMOrganizationLocationDto organizationLocation);
}
