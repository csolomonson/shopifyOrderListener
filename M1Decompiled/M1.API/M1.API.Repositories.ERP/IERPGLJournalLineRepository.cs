using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPGLJournalLineRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a GLJournalLine with the specified Unique Id exists.
	/// </summary>
	/// <param name="gLJournalLineId">The Unique Id of the GLJournalLine to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the GLJournalLine exists or not.</returns>
	Task<bool> DoesGLJournalLineExist(Guid gLJournalLineId);

	/// <summary>
	/// Retrieves all GLJournalLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of GLJournalLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of GLJournalLines DTOs.</returns>
	Task<ICollection<ERPGLJournalLineInformationDto>> GetAllGLJournalLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific GLJournalLine.
	/// </summary>
	/// <param name="gLJournalLineId">The Unique Id of the GLJournalLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the GLJournalLine DTO.</returns>
	Task<ERPGLJournalLineInformationDto> GetGLJournalLine(Guid gLJournalLineId);

	/// <summary>
	/// Saves the provided ERP gLJournalLine.
	/// </summary>
	/// <param name="gLJournalLine">The ERP gLJournalLine to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveGLJournalLine(ERPGLJournalLineDto gLJournalLine);
}
