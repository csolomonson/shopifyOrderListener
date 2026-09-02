using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPChangeLogModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all ChangeLog with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ChangeLog to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllChangeLog(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving ChangeLog information based on the specified ChangeLog Unique Id.
	/// </summary>
	/// <param name="changeLogId">The Unique Id of the ChangeLog.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetChangeLog(Guid changeLogId);

	/// <summary>
	/// Processes the request to retrieve all ChangeLog with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ChangeLog to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ChangeLog DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPChangeLogDto>>> Process_GetAllChangeLog(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific ChangeLog.
	/// </summary>
	/// <param name="changeLogId">The Unique Id of the ChangeLog to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the ChangeLog DTO.</returns>
	Task<ERPResponseMessageDto<ERPChangeLogDto>> Process_GetChangeLog(Guid changeLogId);
}
