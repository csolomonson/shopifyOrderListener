using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPShiftModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all Shifts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Shifts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllShifts(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving Shift information based on the specified Shift Unique Id.
	/// </summary>
	/// <param name="shiftId">The Unique Id of the Shift.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetShift(Guid shiftId);

	/// <summary>
	/// Processes the request to retrieve all Shifts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Shifts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of Shifts DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPShiftDto>>> Process_GetAllShifts(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific Shift.
	/// </summary>
	/// <param name="shiftId">The Unique Id of the Shift to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the Shift DTO.</returns>
	Task<ERPResponseMessageDto<ERPShiftDto>> Process_GetShift(Guid shiftId);
}
