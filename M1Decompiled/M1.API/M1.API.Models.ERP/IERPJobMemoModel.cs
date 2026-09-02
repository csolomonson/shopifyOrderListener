using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPJobMemoModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all JobMemos with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of JobMemos to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllJobMemos(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving JobMemo information based on the specified JobMemo Unique Id.
	/// </summary>
	/// <param name="jobMemoId">The Unique Id of the JobMemo.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetJobMemo(Guid jobMemoId);

	/// <summary>
	/// Validates the PUT request for creating or updating JobMemo information based on the specified JobMemo.
	/// </summary>
	/// <param name="jobMemo">The JobMemo details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutJobMemo(ERPJobMemoDto jobMemo);

	/// <summary>
	/// Processes the request to retrieve all JobMemos with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of JobMemos to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of JobMemos DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPJobMemoDto>>> Process_GetAllJobMemos(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific JobMemo.
	/// </summary>
	/// <param name="jobMemoId">The Unique Id of the JobMemo to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the JobMemo DTO.</returns>
	Task<ERPResponseMessageDto<ERPJobMemoDto>> Process_GetJobMemo(Guid jobMemoId);

	/// <summary>
	/// Processes the creating or updating of a JobMemo record.
	/// </summary>
	/// <param name="jobMemo">The JobMemo data transfer object (DTO) containing the details of the JobMemo to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the JobMemo details.</returns>
	Task<ERPResponseMessageDto<ERPJobMemoDto>> Process_PutJobMemo(ERPJobMemoDto jobMemo);

	/// <summary>
	/// Validates the request for deleting a JobMemo record.
	/// </summary>
	/// <param name="jobMemoId">The Unique Id of the JobMemo.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteJobMemo(Guid jobMemoId);

	/// <summary>
	/// Processes the request to delete a JobMemo record.
	/// </summary>
	/// <param name="jobMemoId">The Unique Id of the JobMemo.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPJobMemoDto>> Process_DeleteJobMemo(Guid jobMemoId);
}
