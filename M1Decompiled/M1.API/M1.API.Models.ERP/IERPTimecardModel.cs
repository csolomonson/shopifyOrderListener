using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPTimecardModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all Timecards with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Timecards to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllTimecards(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving Timecard information based on the specified Timecard Unique Id.
	/// </summary>
	/// <param name="timecardId">The Unique Id of the Timecard.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetTimecard(Guid timecardId);

	/// <summary>
	/// Validates the PUT request for creating or updating Timecard information based on the specified Timecard.
	/// </summary>
	/// <param name="timecard">The Timecard details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutTimecard(ERPTimecardDto timecard);

	/// <summary>
	/// Processes the request to retrieve all Timecards with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Timecards to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of Timecards DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPTimecardDto>>> Process_GetAllTimecards(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific Timecard.
	/// </summary>
	/// <param name="timecardId">The Unique Id of the Timecard to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the Timecard DTO.</returns>
	Task<ERPResponseMessageDto<ERPTimecardDto>> Process_GetTimecard(Guid timecardId);

	/// <summary>
	/// Processes the creating or updating of a Timecard record.
	/// </summary>
	/// <param name="timecard">The Timecard data transfer object (DTO) containing the details of the Timecard to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the Timecard details.</returns>
	Task<ERPResponseMessageDto<ERPTimecardDto>> Process_PutTimecard(ERPTimecardDto timecard);

	/// <summary>
	/// Validates the request for deleting a Timecard record.
	/// </summary>
	/// <param name="timecardId">The Unique Id of the Timecard.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteTimecard(Guid timecardId);

	/// <summary>
	/// Processes the request to delete a Timecard record.
	/// </summary>
	/// <param name="timecardId">The Unique Id of the Timecard.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPTimecardDto>> Process_DeleteTimecard(Guid timecardId);
}
