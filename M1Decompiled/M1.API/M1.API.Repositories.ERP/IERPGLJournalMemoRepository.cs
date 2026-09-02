using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPGLJournalMemoRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a GLJournalMemo with the specified Unique Id exists.
	/// </summary>
	/// <param name="gLJournalMemoId">The Unique Id of the GLJournalMemo to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the GLJournalMemo exists or not.</returns>
	Task<bool> DoesGLJournalMemoExist(Guid gLJournalMemoId);

	/// <summary>
	/// Retrieves all GLJournalMemos with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of GLJournalMemos to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of GLJournalMemos DTOs.</returns>
	Task<ICollection<ERPGLJournalMemoInformationDto>> GetAllGLJournalMemos(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific GLJournalMemo.
	/// </summary>
	/// <param name="gLJournalMemoId">The Unique Id of the GLJournalMemo to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the GLJournalMemo DTO.</returns>
	Task<ERPGLJournalMemoInformationDto> GetGLJournalMemo(Guid gLJournalMemoId);

	/// <summary>
	/// Saves the provided ERP gLJournalMemo.
	/// </summary>
	/// <param name="gLJournalMemo">The ERP gLJournalMemo to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveGLJournalMemo(ERPGLJournalMemoDto gLJournalMemo);
}
