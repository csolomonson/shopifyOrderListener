using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPDMRClaimComponentModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all DMRClaimComponents with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of DMRClaimComponents to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllDMRClaimComponents(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving DMRClaimComponent information based on the specified DMRClaimComponent Unique Id.
	/// </summary>
	/// <param name="dMRClaimComponentId">The Unique Id of the DMRClaimComponent.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetDMRClaimComponent(Guid dMRClaimComponentId);

	/// <summary>
	/// Validates the PUT request for creating or updating DMRClaimComponent information based on the specified DMRClaimComponent.
	/// </summary>
	/// <param name="dMRClaimComponent">The DMRClaimComponent details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutDMRClaimComponent(ERPDMRClaimComponentDto dMRClaimComponent);

	/// <summary>
	/// Processes the request to retrieve all DMRClaimComponents with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of DMRClaimComponents to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of DMRClaimComponents DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPDMRClaimComponentDto>>> Process_GetAllDMRClaimComponents(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific DMRClaimComponent.
	/// </summary>
	/// <param name="dMRClaimComponentId">The Unique Id of the DMRClaimComponent to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the DMRClaimComponent DTO.</returns>
	Task<ERPResponseMessageDto<ERPDMRClaimComponentDto>> Process_GetDMRClaimComponent(Guid dMRClaimComponentId);

	/// <summary>
	/// Processes the creating or updating of a DMRClaimComponent record.
	/// </summary>
	/// <param name="dMRClaimComponent">The DMRClaimComponent data transfer object (DTO) containing the details of the DMRClaimComponent to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the DMRClaimComponent details.</returns>
	Task<ERPResponseMessageDto<ERPDMRClaimComponentDto>> Process_PutDMRClaimComponent(ERPDMRClaimComponentDto dMRClaimComponent);

	/// <summary>
	/// Validates the request for deleting a DMRClaimComponent record.
	/// </summary>
	/// <param name="dMRClaimComponentId">The Unique Id of the DMRClaimComponent.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteDMRClaimComponent(Guid dMRClaimComponentId);

	/// <summary>
	/// Processes the request to delete a DMRClaimComponent record.
	/// </summary>
	/// <param name="dMRClaimComponentId">The Unique Id of the DMRClaimComponent.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPDMRClaimComponentDto>> Process_DeleteDMRClaimComponent(Guid dMRClaimComponentId);
}
