using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPTopActivitiesLogModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all TopActivitiesLog with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of TopActivitiesLog to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllTopActivitiesLog(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving TopActivitiesLog information based on the specified TopActivitiesLog Unique Id.
	/// </summary>
	/// <param name="topActivitiesLogId">The Unique Id of the TopActivitiesLog.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetTopActivitiesLog(Guid topActivitiesLogId);

	/// <summary>
	/// Processes the request to retrieve all TopActivitiesLog with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of TopActivitiesLog to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of TopActivitiesLog DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPTopActivitiesLogDto>>> Process_GetAllTopActivitiesLog(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific TopActivitiesLog.
	/// </summary>
	/// <param name="topActivitiesLogId">The Unique Id of the TopActivitiesLog to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the TopActivitiesLog DTO.</returns>
	Task<ERPResponseMessageDto<ERPTopActivitiesLogDto>> Process_GetTopActivitiesLog(Guid topActivitiesLogId);
}
