using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPMaterialIssueComponentModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all MaterialIssueComponents with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of MaterialIssueComponents to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllMaterialIssueComponents(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving MaterialIssueComponent information based on the specified MaterialIssueComponent Unique Id.
	/// </summary>
	/// <param name="materialIssueComponentId">The Unique Id of the MaterialIssueComponent.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetMaterialIssueComponent(Guid materialIssueComponentId);

	/// <summary>
	/// Validates the PUT request for creating or updating MaterialIssueComponent information based on the specified MaterialIssueComponent.
	/// </summary>
	/// <param name="materialIssueComponent">The MaterialIssueComponent details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutMaterialIssueComponent(ERPMaterialIssueComponentDto materialIssueComponent);

	/// <summary>
	/// Processes the request to retrieve all MaterialIssueComponents with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of MaterialIssueComponents to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of MaterialIssueComponents DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPMaterialIssueComponentDto>>> Process_GetAllMaterialIssueComponents(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific MaterialIssueComponent.
	/// </summary>
	/// <param name="materialIssueComponentId">The Unique Id of the MaterialIssueComponent to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the MaterialIssueComponent DTO.</returns>
	Task<ERPResponseMessageDto<ERPMaterialIssueComponentDto>> Process_GetMaterialIssueComponent(Guid materialIssueComponentId);

	/// <summary>
	/// Processes the creating or updating of a MaterialIssueComponent record.
	/// </summary>
	/// <param name="materialIssueComponent">The MaterialIssueComponent data transfer object (DTO) containing the details of the MaterialIssueComponent to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the MaterialIssueComponent details.</returns>
	Task<ERPResponseMessageDto<ERPMaterialIssueComponentDto>> Process_PutMaterialIssueComponent(ERPMaterialIssueComponentDto materialIssueComponent);

	/// <summary>
	/// Validates the request for deleting a MaterialIssueComponent record.
	/// </summary>
	/// <param name="materialIssueComponentId">The Unique Id of the MaterialIssueComponent.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteMaterialIssueComponent(Guid materialIssueComponentId);

	/// <summary>
	/// Processes the request to delete a MaterialIssueComponent record.
	/// </summary>
	/// <param name="materialIssueComponentId">The Unique Id of the MaterialIssueComponent.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPMaterialIssueComponentDto>> Process_DeleteMaterialIssueComponent(Guid materialIssueComponentId);
}
