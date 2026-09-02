using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPChangeRequestGroupLinkModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all ChangeRequestGroupLinks with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ChangeRequestGroupLinks to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllChangeRequestGroupLinks(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving ChangeRequestGroupLink information based on the specified ChangeRequestGroupLink Unique Id.
	/// </summary>
	/// <param name="changeRequestGroupLinkId">The Unique Id of the ChangeRequestGroupLink.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetChangeRequestGroupLink(Guid changeRequestGroupLinkId);

	/// <summary>
	/// Validates the PUT request for creating or updating ChangeRequestGroupLink information based on the specified ChangeRequestGroupLink.
	/// </summary>
	/// <param name="changeRequestGroupLink">The ChangeRequestGroupLink details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutChangeRequestGroupLink(ERPChangeRequestGroupLinkDto changeRequestGroupLink);

	/// <summary>
	/// Processes the request to retrieve all ChangeRequestGroupLinks with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ChangeRequestGroupLinks to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ChangeRequestGroupLinks DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPChangeRequestGroupLinkDto>>> Process_GetAllChangeRequestGroupLinks(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific ChangeRequestGroupLink.
	/// </summary>
	/// <param name="changeRequestGroupLinkId">The Unique Id of the ChangeRequestGroupLink to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the ChangeRequestGroupLink DTO.</returns>
	Task<ERPResponseMessageDto<ERPChangeRequestGroupLinkDto>> Process_GetChangeRequestGroupLink(Guid changeRequestGroupLinkId);

	/// <summary>
	/// Processes the creating or updating of a ChangeRequestGroupLink record.
	/// </summary>
	/// <param name="changeRequestGroupLink">The ChangeRequestGroupLink data transfer object (DTO) containing the details of the ChangeRequestGroupLink to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the ChangeRequestGroupLink details.</returns>
	Task<ERPResponseMessageDto<ERPChangeRequestGroupLinkDto>> Process_PutChangeRequestGroupLink(ERPChangeRequestGroupLinkDto changeRequestGroupLink);

	/// <summary>
	/// Validates the request for deleting a ChangeRequestGroupLink record.
	/// </summary>
	/// <param name="changeRequestGroupLinkId">The Unique Id of the ChangeRequestGroupLink.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteChangeRequestGroupLink(Guid changeRequestGroupLinkId);

	/// <summary>
	/// Processes the request to delete a ChangeRequestGroupLink record.
	/// </summary>
	/// <param name="changeRequestGroupLinkId">The Unique Id of the ChangeRequestGroupLink.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPChangeRequestGroupLinkDto>> Process_DeleteChangeRequestGroupLink(Guid changeRequestGroupLinkId);
}
