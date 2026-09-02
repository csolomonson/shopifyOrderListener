using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPGLJournalMemoModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all GLJournalMemos with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of GLJournalMemos to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllGLJournalMemos(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving GLJournalMemo information based on the specified GLJournalMemo Unique Id.
	/// </summary>
	/// <param name="gLJournalMemoId">The Unique Id of the GLJournalMemo.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetGLJournalMemo(Guid gLJournalMemoId);

	/// <summary>
	/// Validates the PUT request for creating or updating GLJournalMemo information based on the specified GLJournalMemo.
	/// </summary>
	/// <param name="gLJournalMemo">The GLJournalMemo details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutGLJournalMemo(ERPGLJournalMemoDto gLJournalMemo);

	/// <summary>
	/// Processes the request to retrieve all GLJournalMemos with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of GLJournalMemos to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of GLJournalMemos DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPGLJournalMemoDto>>> Process_GetAllGLJournalMemos(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific GLJournalMemo.
	/// </summary>
	/// <param name="gLJournalMemoId">The Unique Id of the GLJournalMemo to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the GLJournalMemo DTO.</returns>
	Task<ERPResponseMessageDto<ERPGLJournalMemoDto>> Process_GetGLJournalMemo(Guid gLJournalMemoId);

	/// <summary>
	/// Processes the creating or updating of a GLJournalMemo record.
	/// </summary>
	/// <param name="gLJournalMemo">The GLJournalMemo data transfer object (DTO) containing the details of the GLJournalMemo to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the GLJournalMemo details.</returns>
	Task<ERPResponseMessageDto<ERPGLJournalMemoDto>> Process_PutGLJournalMemo(ERPGLJournalMemoDto gLJournalMemo);

	/// <summary>
	/// Validates the request for deleting a GLJournalMemo record.
	/// </summary>
	/// <param name="gLJournalMemoId">The Unique Id of the GLJournalMemo.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteGLJournalMemo(Guid gLJournalMemoId);

	/// <summary>
	/// Processes the request to delete a GLJournalMemo record.
	/// </summary>
	/// <param name="gLJournalMemoId">The Unique Id of the GLJournalMemo.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPGLJournalMemoDto>> Process_DeleteGLJournalMemo(Guid gLJournalMemoId);
}
