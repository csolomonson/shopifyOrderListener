using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPPartMemoModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all PartMemos with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartMemos to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllPartMemos(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving PartMemo information based on the specified PartMemo Unique Id.
	/// </summary>
	/// <param name="partMemoId">The Unique Id of the PartMemo.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetPartMemo(Guid partMemoId);

	/// <summary>
	/// Validates the PUT request for creating or updating PartMemo information based on the specified PartMemo.
	/// </summary>
	/// <param name="partMemo">The PartMemo details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutPartMemo(ERPPartMemoDto partMemo);

	/// <summary>
	/// Processes the request to retrieve all PartMemos with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartMemos to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PartMemos DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPPartMemoDto>>> Process_GetAllPartMemos(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific PartMemo.
	/// </summary>
	/// <param name="partMemoId">The Unique Id of the PartMemo to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the PartMemo DTO.</returns>
	Task<ERPResponseMessageDto<ERPPartMemoDto>> Process_GetPartMemo(Guid partMemoId);

	/// <summary>
	/// Processes the creating or updating of a PartMemo record.
	/// </summary>
	/// <param name="partMemo">The PartMemo data transfer object (DTO) containing the details of the PartMemo to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the PartMemo details.</returns>
	Task<ERPResponseMessageDto<ERPPartMemoDto>> Process_PutPartMemo(ERPPartMemoDto partMemo);

	/// <summary>
	/// Validates the request for deleting a PartMemo record.
	/// </summary>
	/// <param name="partMemoId">The Unique Id of the PartMemo.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeletePartMemo(Guid partMemoId);

	/// <summary>
	/// Processes the request to delete a PartMemo record.
	/// </summary>
	/// <param name="partMemoId">The Unique Id of the PartMemo.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPPartMemoDto>> Process_DeletePartMemo(Guid partMemoId);
}
