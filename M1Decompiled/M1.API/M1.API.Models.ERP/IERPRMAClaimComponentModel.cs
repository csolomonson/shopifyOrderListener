using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPRMAClaimComponentModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all RMAClaimComponents with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of RMAClaimComponents to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllRMAClaimComponents(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving RMAClaimComponent information based on the specified RMAClaimComponent Unique Id.
	/// </summary>
	/// <param name="rMAClaimComponentId">The Unique Id of the RMAClaimComponent.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetRMAClaimComponent(Guid rMAClaimComponentId);

	/// <summary>
	/// Validates the PUT request for creating or updating RMAClaimComponent information based on the specified RMAClaimComponent.
	/// </summary>
	/// <param name="rMAClaimComponent">The RMAClaimComponent details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutRMAClaimComponent(ERPRMAClaimComponentDto rMAClaimComponent);

	/// <summary>
	/// Processes the request to retrieve all RMAClaimComponents with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of RMAClaimComponents to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of RMAClaimComponents DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPRMAClaimComponentDto>>> Process_GetAllRMAClaimComponents(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific RMAClaimComponent.
	/// </summary>
	/// <param name="rMAClaimComponentId">The Unique Id of the RMAClaimComponent to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the RMAClaimComponent DTO.</returns>
	Task<ERPResponseMessageDto<ERPRMAClaimComponentDto>> Process_GetRMAClaimComponent(Guid rMAClaimComponentId);

	/// <summary>
	/// Processes the creating or updating of a RMAClaimComponent record.
	/// </summary>
	/// <param name="rMAClaimComponent">The RMAClaimComponent data transfer object (DTO) containing the details of the RMAClaimComponent to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the RMAClaimComponent details.</returns>
	Task<ERPResponseMessageDto<ERPRMAClaimComponentDto>> Process_PutRMAClaimComponent(ERPRMAClaimComponentDto rMAClaimComponent);

	/// <summary>
	/// Validates the request for deleting a RMAClaimComponent record.
	/// </summary>
	/// <param name="rMAClaimComponentId">The Unique Id of the RMAClaimComponent.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteRMAClaimComponent(Guid rMAClaimComponentId);

	/// <summary>
	/// Processes the request to delete a RMAClaimComponent record.
	/// </summary>
	/// <param name="rMAClaimComponentId">The Unique Id of the RMAClaimComponent.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPRMAClaimComponentDto>> Process_DeleteRMAClaimComponent(Guid rMAClaimComponentId);
}
