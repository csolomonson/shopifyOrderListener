using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPTimecardLineRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a TimecardLine with the specified Unique Id exists.
	/// </summary>
	/// <param name="timecardLineId">The Unique Id of the TimecardLine to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the TimecardLine exists or not.</returns>
	Task<bool> DoesTimecardLineExist(Guid timecardLineId);

	/// <summary>
	/// Retrieves all TimecardLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of TimecardLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of TimecardLines DTOs.</returns>
	Task<ICollection<ERPTimecardLineInformationDto>> GetAllTimecardLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific TimecardLine.
	/// </summary>
	/// <param name="timecardLineId">The Unique Id of the TimecardLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the TimecardLine DTO.</returns>
	Task<ERPTimecardLineInformationDto> GetTimecardLine(Guid timecardLineId);

	/// <summary>
	/// Saves the provided ERP timecardLine.
	/// </summary>
	/// <param name="timecardLine">The ERP timecardLine to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveTimecardLine(ERPTimecardLineDto timecardLine);
}
