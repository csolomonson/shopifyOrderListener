using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPJobMemoRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a JobMemo with the specified Unique Id exists.
	/// </summary>
	/// <param name="jobMemoId">The Unique Id of the JobMemo to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the JobMemo exists or not.</returns>
	Task<bool> DoesJobMemoExist(Guid jobMemoId);

	/// <summary>
	/// Retrieves all JobMemos with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of JobMemos to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of JobMemos DTOs.</returns>
	Task<ICollection<ERPJobMemoInformationDto>> GetAllJobMemos(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific JobMemo.
	/// </summary>
	/// <param name="jobMemoId">The Unique Id of the JobMemo to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the JobMemo DTO.</returns>
	Task<ERPJobMemoInformationDto> GetJobMemo(Guid jobMemoId);

	/// <summary>
	/// Saves the provided ERP jobMemo.
	/// </summary>
	/// <param name="jobMemo">The ERP jobMemo to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveJobMemo(ERPJobMemoDto jobMemo);
}
