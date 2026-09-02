using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPRFQMemoModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all RFQMemos with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of RFQMemos to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllRFQMemos(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving RFQMemo information based on the specified RFQMemo Unique Id.
	/// </summary>
	/// <param name="rFQMemoId">The Unique Id of the RFQMemo.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetRFQMemo(Guid rFQMemoId);

	/// <summary>
	/// Validates the PUT request for creating or updating RFQMemo information based on the specified RFQMemo.
	/// </summary>
	/// <param name="rFQMemo">The RFQMemo details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutRFQMemo(ERPRFQMemoDto rFQMemo);

	/// <summary>
	/// Processes the request to retrieve all RFQMemos with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of RFQMemos to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of RFQMemos DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPRFQMemoDto>>> Process_GetAllRFQMemos(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific RFQMemo.
	/// </summary>
	/// <param name="rFQMemoId">The Unique Id of the RFQMemo to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the RFQMemo DTO.</returns>
	Task<ERPResponseMessageDto<ERPRFQMemoDto>> Process_GetRFQMemo(Guid rFQMemoId);

	/// <summary>
	/// Processes the creating or updating of a RFQMemo record.
	/// </summary>
	/// <param name="rFQMemo">The RFQMemo data transfer object (DTO) containing the details of the RFQMemo to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the RFQMemo details.</returns>
	Task<ERPResponseMessageDto<ERPRFQMemoDto>> Process_PutRFQMemo(ERPRFQMemoDto rFQMemo);

	/// <summary>
	/// Validates the request for deleting a RFQMemo record.
	/// </summary>
	/// <param name="rFQMemoId">The Unique Id of the RFQMemo.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteRFQMemo(Guid rFQMemoId);

	/// <summary>
	/// Processes the request to delete a RFQMemo record.
	/// </summary>
	/// <param name="rFQMemoId">The Unique Id of the RFQMemo.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPRFQMemoDto>> Process_DeleteRFQMemo(Guid rFQMemoId);
}
