using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPRMAClaimRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a RMAClaim with the specified Unique Id exists.
	/// </summary>
	/// <param name="rMAClaimId">The Unique Id of the RMAClaim to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the RMAClaim exists or not.</returns>
	Task<bool> DoesRMAClaimExist(Guid rMAClaimId);

	/// <summary>
	/// Retrieves all RMAClaims with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of RMAClaims to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of RMAClaims DTOs.</returns>
	Task<ICollection<ERPRMAClaimInformationDto>> GetAllRMAClaims(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific RMAClaim.
	/// </summary>
	/// <param name="rMAClaimId">The Unique Id of the RMAClaim to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the RMAClaim DTO.</returns>
	Task<ERPRMAClaimInformationDto> GetRMAClaim(Guid rMAClaimId);

	/// <summary>
	/// Saves the provided ERP rMAClaim.
	/// </summary>
	/// <param name="rMAClaim">The ERP rMAClaim to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveRMAClaim(ERPRMAClaimDto rMAClaim);
}
