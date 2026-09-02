using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPMaterialIssueLineModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all MaterialIssueLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of MaterialIssueLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllMaterialIssueLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving MaterialIssueLine information based on the specified MaterialIssueLine Unique Id.
	/// </summary>
	/// <param name="materialIssueLineId">The Unique Id of the MaterialIssueLine.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetMaterialIssueLine(Guid materialIssueLineId);

	/// <summary>
	/// Validates the PUT request for creating or updating MaterialIssueLine information based on the specified MaterialIssueLine.
	/// </summary>
	/// <param name="materialIssueLine">The MaterialIssueLine details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutMaterialIssueLine(ERPMaterialIssueLineDto materialIssueLine);

	/// <summary>
	/// Processes the request to retrieve all MaterialIssueLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of MaterialIssueLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of MaterialIssueLines DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPMaterialIssueLineDto>>> Process_GetAllMaterialIssueLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific MaterialIssueLine.
	/// </summary>
	/// <param name="materialIssueLineId">The Unique Id of the MaterialIssueLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the MaterialIssueLine DTO.</returns>
	Task<ERPResponseMessageDto<ERPMaterialIssueLineDto>> Process_GetMaterialIssueLine(Guid materialIssueLineId);

	/// <summary>
	/// Processes the creating or updating of a MaterialIssueLine record.
	/// </summary>
	/// <param name="materialIssueLine">The MaterialIssueLine data transfer object (DTO) containing the details of the MaterialIssueLine to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the MaterialIssueLine details.</returns>
	Task<ERPResponseMessageDto<ERPMaterialIssueLineDto>> Process_PutMaterialIssueLine(ERPMaterialIssueLineDto materialIssueLine);

	/// <summary>
	/// Validates the request for deleting a MaterialIssueLine record.
	/// </summary>
	/// <param name="materialIssueLineId">The Unique Id of the MaterialIssueLine.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteMaterialIssueLine(Guid materialIssueLineId);

	/// <summary>
	/// Processes the request to delete a MaterialIssueLine record.
	/// </summary>
	/// <param name="materialIssueLineId">The Unique Id of the MaterialIssueLine.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPMaterialIssueLineDto>> Process_DeleteMaterialIssueLine(Guid materialIssueLineId);
}
