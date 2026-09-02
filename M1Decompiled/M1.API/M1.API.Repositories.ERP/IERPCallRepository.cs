using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPCallRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a Call with the specified Unique Id exists.
	/// </summary>
	/// <param name="callId">The Unique Id of the Call to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the Call exists or not.</returns>
	Task<bool> DoesCallExist(Guid callId);

	/// <summary>
	/// Retrieves all Calls with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Calls to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of Calls DTOs.</returns>
	Task<ICollection<ERPCallInformationDto>> GetAllCalls(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific Call.
	/// </summary>
	/// <param name="callId">The Unique Id of the Call to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the Call DTO.</returns>
	Task<ERPCallInformationDto> GetCall(Guid callId);

	/// <summary>
	/// Saves the provided ERP call.
	/// </summary>
	/// <param name="call">The ERP call to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveCall(ERPCallDto call);
}
