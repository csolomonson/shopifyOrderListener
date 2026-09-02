using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPWorkCenterMemoModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all WorkCenterMemos with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of WorkCenterMemos to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllWorkCenterMemos(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving WorkCenterMemo information based on the specified WorkCenterMemo Unique Id.
	/// </summary>
	/// <param name="workCenterMemoId">The Unique Id of the WorkCenterMemo.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetWorkCenterMemo(Guid workCenterMemoId);

	/// <summary>
	/// Validates the PUT request for creating or updating WorkCenterMemo information based on the specified WorkCenterMemo.
	/// </summary>
	/// <param name="workCenterMemo">The WorkCenterMemo details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutWorkCenterMemo(ERPWorkCenterMemoDto workCenterMemo);

	/// <summary>
	/// Processes the request to retrieve all WorkCenterMemos with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of WorkCenterMemos to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of WorkCenterMemos DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPWorkCenterMemoDto>>> Process_GetAllWorkCenterMemos(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific WorkCenterMemo.
	/// </summary>
	/// <param name="workCenterMemoId">The Unique Id of the WorkCenterMemo to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the WorkCenterMemo DTO.</returns>
	Task<ERPResponseMessageDto<ERPWorkCenterMemoDto>> Process_GetWorkCenterMemo(Guid workCenterMemoId);

	/// <summary>
	/// Processes the creating or updating of a WorkCenterMemo record.
	/// </summary>
	/// <param name="workCenterMemo">The WorkCenterMemo data transfer object (DTO) containing the details of the WorkCenterMemo to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the WorkCenterMemo details.</returns>
	Task<ERPResponseMessageDto<ERPWorkCenterMemoDto>> Process_PutWorkCenterMemo(ERPWorkCenterMemoDto workCenterMemo);

	/// <summary>
	/// Validates the request for deleting a WorkCenterMemo record.
	/// </summary>
	/// <param name="workCenterMemoId">The Unique Id of the WorkCenterMemo.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteWorkCenterMemo(Guid workCenterMemoId);

	/// <summary>
	/// Processes the request to delete a WorkCenterMemo record.
	/// </summary>
	/// <param name="workCenterMemoId">The Unique Id of the WorkCenterMemo.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPWorkCenterMemoDto>> Process_DeleteWorkCenterMemo(Guid workCenterMemoId);
}
