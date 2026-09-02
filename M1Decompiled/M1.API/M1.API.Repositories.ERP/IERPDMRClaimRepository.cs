using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPDMRClaimRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a DMRClaim with the specified Unique Id exists.
	/// </summary>
	/// <param name="dMRClaimId">The Unique Id of the DMRClaim to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the DMRClaim exists or not.</returns>
	Task<bool> DoesDMRClaimExist(Guid dMRClaimId);

	/// <summary>
	/// Retrieves all DMRClaims with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of DMRClaims to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of DMRClaims DTOs.</returns>
	Task<ICollection<ERPDMRClaimInformationDto>> GetAllDMRClaims(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific DMRClaim.
	/// </summary>
	/// <param name="dMRClaimId">The Unique Id of the DMRClaim to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the DMRClaim DTO.</returns>
	Task<ERPDMRClaimInformationDto> GetDMRClaim(Guid dMRClaimId);

	/// <summary>
	/// Saves the provided ERP dMRClaim.
	/// </summary>
	/// <param name="dMRClaim">The ERP dMRClaim to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveDMRClaim(ERPDMRClaimDto dMRClaim);
}
