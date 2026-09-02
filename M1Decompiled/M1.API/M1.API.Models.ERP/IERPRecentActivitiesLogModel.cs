using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPRecentActivitiesLogModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all RecentActivitiesLog with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of RecentActivitiesLog to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllRecentActivitiesLog(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving RecentActivitiesLog information based on the specified RecentActivitiesLog Unique Id.
	/// </summary>
	/// <param name="recentActivitiesLogId">The Unique Id of the RecentActivitiesLog.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetRecentActivitiesLog(Guid recentActivitiesLogId);

	/// <summary>
	/// Processes the request to retrieve all RecentActivitiesLog with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of RecentActivitiesLog to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of RecentActivitiesLog DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPRecentActivitiesLogDto>>> Process_GetAllRecentActivitiesLog(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific RecentActivitiesLog.
	/// </summary>
	/// <param name="recentActivitiesLogId">The Unique Id of the RecentActivitiesLog to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the RecentActivitiesLog DTO.</returns>
	Task<ERPResponseMessageDto<ERPRecentActivitiesLogDto>> Process_GetRecentActivitiesLog(Guid recentActivitiesLogId);
}
