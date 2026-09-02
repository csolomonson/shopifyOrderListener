using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPReasonModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all Reasons with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Reasons to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllReasons(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving Reason information based on the specified Reason Unique Id.
	/// </summary>
	/// <param name="reasonId">The Unique Id of the Reason.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetReason(Guid reasonId);

	/// <summary>
	/// Processes the request to retrieve all Reasons with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Reasons to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of Reasons DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPReasonDto>>> Process_GetAllReasons(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific Reason.
	/// </summary>
	/// <param name="reasonId">The Unique Id of the Reason to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the Reason DTO.</returns>
	Task<ERPResponseMessageDto<ERPReasonDto>> Process_GetReason(Guid reasonId);
}
