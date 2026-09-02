using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.DTOs.BOM.Job;
using M1.API.DTOs.Core;

namespace M1.API.Models.BOM.Job;

public interface IBOMTimecardModel : IBOMBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving Timecard information based on the specified Timecard ID.
	/// </summary>
	/// <param name="timecardId">The ID of the Timecard.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetTimecard(string timecardId);

	/// <summary>
	/// Processes the request to retrieve all Timecards with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Timecards to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a BOM response message DTO with a list of Timecards DTOs.</returns>
	Task<BOMResponseMessageDto<IList<BOMTimecardDto>>> Process_GetAllTimecards(int pageSize, int pageNumber);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific Timecard.
	/// </summary>
	/// <param name="timecardId">The ID of the Timecard to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a BOM response message DTO with the Timecard DTO.</returns>
	Task<BOMResponseMessageDto<BOMTimecardDto>> Process_GetTimecard(string timecardId);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific Timecard.
	/// </summary>
	/// <param name="timecardId">The ID of the Timecard to retrieve information for.</param>
	/// <param name="employeeId">The ID of Employee to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a BOM response message DTO with the Timecard DTO.</returns>
	Task<BOMResponseMessageDto<BOMTimecardDto>> Process_GetTimecard(string timecardId, string employeeId);
}
