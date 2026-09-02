using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.DTOs.Core;

namespace M1.API.Models.BOM;

public interface IBOMTimecardLineModel : IBOMBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving TimecardLine information based on the specified Timecard ID and TimecardLine ID.
	/// </summary>
	/// <param name="timecardId">The ID of the Timecard.</param>
	/// <param name="timecardLineId">The ID of the TimecardLine.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetTimecardLine(string timecardId, string timecardLineId);

	/// <summary>
	/// Processes the request to retrieve all TimecardLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of TimecardLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a BOM response message DTO with a list of TimecardLines DTOs.</returns>
	Task<BOMResponseMessageDto<IList<BOMTimecardLineDto>>> Process_GetAllTimecardLines(int pageSize, int pageNumber);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific TimecardLine.
	/// </summary>
	/// <param name="timecardId">The ID of the Timecard to retrieve information for.</param>
	/// <param name="timecardLineId">The ID of the TimecardLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a BOM response message DTO with the TimecardLine DTO.</returns>
	Task<BOMResponseMessageDto<BOMTimecardLineDto>> Process_GetTimecardLine(string timecardId, string timecardLineId);
}
