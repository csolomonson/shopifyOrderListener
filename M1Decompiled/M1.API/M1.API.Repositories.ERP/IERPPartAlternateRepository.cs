using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPPartAlternateRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a PartAlternate with the specified Unique Id exists.
	/// </summary>
	/// <param name="partAlternateId">The Unique Id of the PartAlternate to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the PartAlternate exists or not.</returns>
	Task<bool> DoesPartAlternateExist(Guid partAlternateId);

	/// <summary>
	/// Retrieves all PartAlternates with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartAlternates to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PartAlternates DTOs.</returns>
	Task<ICollection<ERPPartAlternateInformationDto>> GetAllPartAlternates(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific PartAlternate.
	/// </summary>
	/// <param name="partAlternateId">The Unique Id of the PartAlternate to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the PartAlternate DTO.</returns>
	Task<ERPPartAlternateInformationDto> GetPartAlternate(Guid partAlternateId);

	/// <summary>
	/// Saves the provided ERP partAlternate.
	/// </summary>
	/// <param name="partAlternate">The ERP partAlternate to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SavePartAlternate(ERPPartAlternateDto partAlternate);
}
