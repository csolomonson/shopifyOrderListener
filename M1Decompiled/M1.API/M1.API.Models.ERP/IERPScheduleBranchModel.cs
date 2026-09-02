using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPScheduleBranchModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all ScheduleBranches with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ScheduleBranches to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllScheduleBranches(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving ScheduleBranch information based on the specified ScheduleBranch Unique Id.
	/// </summary>
	/// <param name="scheduleBranchId">The Unique Id of the ScheduleBranch.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetScheduleBranch(Guid scheduleBranchId);

	/// <summary>
	/// Processes the request to retrieve all ScheduleBranches with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ScheduleBranches to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ScheduleBranches DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPScheduleBranchDto>>> Process_GetAllScheduleBranches(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific ScheduleBranch.
	/// </summary>
	/// <param name="scheduleBranchId">The Unique Id of the ScheduleBranch to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the ScheduleBranch DTO.</returns>
	Task<ERPResponseMessageDto<ERPScheduleBranchDto>> Process_GetScheduleBranch(Guid scheduleBranchId);
}
