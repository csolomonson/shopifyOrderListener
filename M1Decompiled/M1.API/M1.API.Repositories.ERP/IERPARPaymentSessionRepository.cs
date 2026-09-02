using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPARPaymentSessionRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a ARPaymentSession with the specified Unique Id exists.
	/// </summary>
	/// <param name="aRPaymentSessionId">The Unique Id of the ARPaymentSession to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the ARPaymentSession exists or not.</returns>
	Task<bool> DoesARPaymentSessionExist(Guid aRPaymentSessionId);

	/// <summary>
	/// Retrieves all ARPaymentSessions with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ARPaymentSessions to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ARPaymentSessions DTOs.</returns>
	Task<ICollection<ERPARPaymentSessionInformationDto>> GetAllARPaymentSessions(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific ARPaymentSession.
	/// </summary>
	/// <param name="aRPaymentSessionId">The Unique Id of the ARPaymentSession to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the ARPaymentSession DTO.</returns>
	Task<ERPARPaymentSessionInformationDto> GetARPaymentSession(Guid aRPaymentSessionId);

	/// <summary>
	/// Saves the provided ERP aRPaymentSession.
	/// </summary>
	/// <param name="aRPaymentSession">The ERP aRPaymentSession to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveARPaymentSession(ERPARPaymentSessionDto aRPaymentSession);
}
