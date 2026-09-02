using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPGLJournalModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all GLJournals with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of GLJournals to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllGLJournals(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving GLJournal information based on the specified GLJournal Unique Id.
	/// </summary>
	/// <param name="gLJournalId">The Unique Id of the GLJournal.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetGLJournal(Guid gLJournalId);

	/// <summary>
	/// Validates the PUT request for creating or updating GLJournal information based on the specified GLJournal.
	/// </summary>
	/// <param name="gLJournal">The GLJournal details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutGLJournal(ERPGLJournalDto gLJournal);

	/// <summary>
	/// Processes the request to retrieve all GLJournals with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of GLJournals to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of GLJournals DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPGLJournalDto>>> Process_GetAllGLJournals(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific GLJournal.
	/// </summary>
	/// <param name="gLJournalId">The Unique Id of the GLJournal to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the GLJournal DTO.</returns>
	Task<ERPResponseMessageDto<ERPGLJournalDto>> Process_GetGLJournal(Guid gLJournalId);

	/// <summary>
	/// Processes the creating or updating of a GLJournal record.
	/// </summary>
	/// <param name="gLJournal">The GLJournal data transfer object (DTO) containing the details of the GLJournal to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the GLJournal details.</returns>
	Task<ERPResponseMessageDto<ERPGLJournalDto>> Process_PutGLJournal(ERPGLJournalDto gLJournal);

	/// <summary>
	/// Validates the request for deleting a GLJournal record.
	/// </summary>
	/// <param name="gLJournalId">The Unique Id of the GLJournal.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteGLJournal(Guid gLJournalId);

	/// <summary>
	/// Processes the request to delete a GLJournal record.
	/// </summary>
	/// <param name="gLJournalId">The Unique Id of the GLJournal.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPGLJournalDto>> Process_DeleteGLJournal(Guid gLJournalId);
}
