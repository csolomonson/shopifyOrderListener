using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPRMAClaimLineRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a RMAClaimLine with the specified Unique Id exists.
	/// </summary>
	/// <param name="rMAClaimLineId">The Unique Id of the RMAClaimLine to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the RMAClaimLine exists or not.</returns>
	Task<bool> DoesRMAClaimLineExist(Guid rMAClaimLineId);

	/// <summary>
	/// Retrieves all RMAClaimLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of RMAClaimLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of RMAClaimLines DTOs.</returns>
	Task<ICollection<ERPRMAClaimLineInformationDto>> GetAllRMAClaimLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific RMAClaimLine.
	/// </summary>
	/// <param name="rMAClaimLineId">The Unique Id of the RMAClaimLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the RMAClaimLine DTO.</returns>
	Task<ERPRMAClaimLineInformationDto> GetRMAClaimLine(Guid rMAClaimLineId);

	/// <summary>
	/// Saves the provided ERP rMAClaimLine.
	/// </summary>
	/// <param name="rMAClaimLine">The ERP rMAClaimLine to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveRMAClaimLine(ERPRMAClaimLineDto rMAClaimLine);
}
