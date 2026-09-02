using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPMRPSessionRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a MRPSession with the specified Unique Id exists.
	/// </summary>
	/// <param name="mRPSessionId">The Unique Id of the MRPSession to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the MRPSession exists or not.</returns>
	Task<bool> DoesMRPSessionExist(Guid mRPSessionId);

	/// <summary>
	/// Retrieves all MRPSessions with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of MRPSessions to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of MRPSessions DTOs.</returns>
	Task<ICollection<ERPMRPSessionInformationDto>> GetAllMRPSessions(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific MRPSession.
	/// </summary>
	/// <param name="mRPSessionId">The Unique Id of the MRPSession to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the MRPSession DTO.</returns>
	Task<ERPMRPSessionInformationDto> GetMRPSession(Guid mRPSessionId);

	/// <summary>
	/// Saves the provided ERP mRPSession.
	/// </summary>
	/// <param name="mRPSession">The ERP mRPSession to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveMRPSession(ERPMRPSessionDto mRPSession);
}
