using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPOrganizationModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all Organizations with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Organizations to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllOrganizations(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving Organization information based on the specified Organization Unique Id.
	/// </summary>
	/// <param name="organizationId">The Unique Id of the Organization.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetOrganization(Guid organizationId);

	/// <summary>
	/// Validates the PUT request for creating or updating Organization information based on the specified Organization.
	/// </summary>
	/// <param name="organization">The Organization details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutOrganization(ERPOrganizationDto organization);

	/// <summary>
	/// Processes the request to retrieve all Organizations with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Organizations to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of Organizations DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPOrganizationDto>>> Process_GetAllOrganizations(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific Organization.
	/// </summary>
	/// <param name="organizationId">The Unique Id of the Organization to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the Organization DTO.</returns>
	Task<ERPResponseMessageDto<ERPOrganizationDto>> Process_GetOrganization(Guid organizationId);

	/// <summary>
	/// Processes the creating or updating of a Organization record.
	/// </summary>
	/// <param name="organization">The Organization data transfer object (DTO) containing the details of the Organization to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the Organization details.</returns>
	Task<ERPResponseMessageDto<ERPOrganizationDto>> Process_PutOrganization(ERPOrganizationDto organization);

	/// <summary>
	/// Validates the request for deleting a Organization record.
	/// </summary>
	/// <param name="organizationId">The Unique Id of the Organization.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteOrganization(Guid organizationId);

	/// <summary>
	/// Processes the request to delete a Organization record.
	/// </summary>
	/// <param name="organizationId">The Unique Id of the Organization.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPOrganizationDto>> Process_DeleteOrganization(Guid organizationId);
}
