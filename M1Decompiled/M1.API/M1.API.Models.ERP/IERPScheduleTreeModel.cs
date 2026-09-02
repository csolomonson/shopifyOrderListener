using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPScheduleTreeModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all ScheduleTrees with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ScheduleTrees to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllScheduleTrees(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving ScheduleTree information based on the specified ScheduleTree Unique Id.
	/// </summary>
	/// <param name="scheduleTreeId">The Unique Id of the ScheduleTree.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetScheduleTree(Guid scheduleTreeId);

	/// <summary>
	/// Processes the request to retrieve all ScheduleTrees with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ScheduleTrees to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ScheduleTrees DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPScheduleTreeDto>>> Process_GetAllScheduleTrees(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific ScheduleTree.
	/// </summary>
	/// <param name="scheduleTreeId">The Unique Id of the ScheduleTree to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the ScheduleTree DTO.</returns>
	Task<ERPResponseMessageDto<ERPScheduleTreeDto>> Process_GetScheduleTree(Guid scheduleTreeId);
}
