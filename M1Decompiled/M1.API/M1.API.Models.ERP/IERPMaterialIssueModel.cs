using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPMaterialIssueModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all MaterialIssues with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of MaterialIssues to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllMaterialIssues(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving MaterialIssue information based on the specified MaterialIssue Unique Id.
	/// </summary>
	/// <param name="materialIssueId">The Unique Id of the MaterialIssue.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetMaterialIssue(Guid materialIssueId);

	/// <summary>
	/// Validates the PUT request for creating or updating MaterialIssue information based on the specified MaterialIssue.
	/// </summary>
	/// <param name="materialIssue">The MaterialIssue details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutMaterialIssue(ERPMaterialIssueDto materialIssue);

	/// <summary>
	/// Processes the request to retrieve all MaterialIssues with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of MaterialIssues to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of MaterialIssues DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPMaterialIssueDto>>> Process_GetAllMaterialIssues(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific MaterialIssue.
	/// </summary>
	/// <param name="materialIssueId">The Unique Id of the MaterialIssue to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the MaterialIssue DTO.</returns>
	Task<ERPResponseMessageDto<ERPMaterialIssueDto>> Process_GetMaterialIssue(Guid materialIssueId);

	/// <summary>
	/// Processes the creating or updating of a MaterialIssue record.
	/// </summary>
	/// <param name="materialIssue">The MaterialIssue data transfer object (DTO) containing the details of the MaterialIssue to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the MaterialIssue details.</returns>
	Task<ERPResponseMessageDto<ERPMaterialIssueDto>> Process_PutMaterialIssue(ERPMaterialIssueDto materialIssue);

	/// <summary>
	/// Validates the request for deleting a MaterialIssue record.
	/// </summary>
	/// <param name="materialIssueId">The Unique Id of the MaterialIssue.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteMaterialIssue(Guid materialIssueId);

	/// <summary>
	/// Processes the request to delete a MaterialIssue record.
	/// </summary>
	/// <param name="materialIssueId">The Unique Id of the MaterialIssue.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPMaterialIssueDto>> Process_DeleteMaterialIssue(Guid materialIssueId);
}
