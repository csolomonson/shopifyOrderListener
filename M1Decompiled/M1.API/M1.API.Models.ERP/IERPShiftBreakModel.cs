using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPShiftBreakModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all ShiftBreaks with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ShiftBreaks to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllShiftBreaks(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving ShiftBreak information based on the specified ShiftBreak Unique Id.
	/// </summary>
	/// <param name="shiftBreakId">The Unique Id of the ShiftBreak.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetShiftBreak(Guid shiftBreakId);

	/// <summary>
	/// Processes the request to retrieve all ShiftBreaks with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ShiftBreaks to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ShiftBreaks DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPShiftBreakDto>>> Process_GetAllShiftBreaks(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific ShiftBreak.
	/// </summary>
	/// <param name="shiftBreakId">The Unique Id of the ShiftBreak to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the ShiftBreak DTO.</returns>
	Task<ERPResponseMessageDto<ERPShiftBreakDto>> Process_GetShiftBreak(Guid shiftBreakId);
}
