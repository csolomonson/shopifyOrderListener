using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPAPPaymentSessionRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a APPaymentSession with the specified Unique Id exists.
	/// </summary>
	/// <param name="aPPaymentSessionId">The Unique Id of the APPaymentSession to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the APPaymentSession exists or not.</returns>
	Task<bool> DoesAPPaymentSessionExist(Guid aPPaymentSessionId);

	/// <summary>
	/// Retrieves all APPaymentSessions with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of APPaymentSessions to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of APPaymentSessions DTOs.</returns>
	Task<ICollection<ERPAPPaymentSessionInformationDto>> GetAllAPPaymentSessions(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific APPaymentSession.
	/// </summary>
	/// <param name="aPPaymentSessionId">The Unique Id of the APPaymentSession to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the APPaymentSession DTO.</returns>
	Task<ERPAPPaymentSessionInformationDto> GetAPPaymentSession(Guid aPPaymentSessionId);

	/// <summary>
	/// Saves the provided ERP aPPaymentSession.
	/// </summary>
	/// <param name="aPPaymentSession">The ERP aPPaymentSession to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveAPPaymentSession(ERPAPPaymentSessionDto aPPaymentSession);
}
