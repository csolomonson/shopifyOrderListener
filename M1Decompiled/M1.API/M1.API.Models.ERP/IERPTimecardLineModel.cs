using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPTimecardLineModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all TimecardLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of TimecardLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllTimecardLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving TimecardLine information based on the specified TimecardLine Unique Id.
	/// </summary>
	/// <param name="timecardLineId">The Unique Id of the TimecardLine.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetTimecardLine(Guid timecardLineId);

	/// <summary>
	/// Validates the PUT request for creating or updating TimecardLine information based on the specified TimecardLine.
	/// </summary>
	/// <param name="timecardLine">The TimecardLine details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutTimecardLine(ERPTimecardLineDto timecardLine);

	/// <summary>
	/// Processes the request to retrieve all TimecardLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of TimecardLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of TimecardLines DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPTimecardLineDto>>> Process_GetAllTimecardLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific TimecardLine.
	/// </summary>
	/// <param name="timecardLineId">The Unique Id of the TimecardLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the TimecardLine DTO.</returns>
	Task<ERPResponseMessageDto<ERPTimecardLineDto>> Process_GetTimecardLine(Guid timecardLineId);

	/// <summary>
	/// Processes the creating or updating of a TimecardLine record.
	/// </summary>
	/// <param name="timecardLine">The TimecardLine data transfer object (DTO) containing the details of the TimecardLine to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the TimecardLine details.</returns>
	Task<ERPResponseMessageDto<ERPTimecardLineDto>> Process_PutTimecardLine(ERPTimecardLineDto timecardLine);

	/// <summary>
	/// Validates the request for deleting a TimecardLine record.
	/// </summary>
	/// <param name="timecardLineId">The Unique Id of the TimecardLine.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteTimecardLine(Guid timecardLineId);

	/// <summary>
	/// Processes the request to delete a TimecardLine record.
	/// </summary>
	/// <param name="timecardLineId">The Unique Id of the TimecardLine.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPTimecardLineDto>> Process_DeleteTimecardLine(Guid timecardLineId);
}
