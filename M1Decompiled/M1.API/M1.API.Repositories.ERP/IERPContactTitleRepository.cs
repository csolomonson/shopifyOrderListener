using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPContactTitleRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a ContactTitle with the specified Unique Id exists.
	/// </summary>
	/// <param name="contactTitleId">The Unique Id of the ContactTitle to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the ContactTitle exists or not.</returns>
	Task<bool> DoesContactTitleExist(Guid contactTitleId);

	/// <summary>
	/// Retrieves all ContactTitles with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ContactTitles to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ContactTitles DTOs.</returns>
	Task<ICollection<ERPContactTitleInformationDto>> GetAllContactTitles(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific ContactTitle.
	/// </summary>
	/// <param name="contactTitleId">The Unique Id of the ContactTitle to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the ContactTitle DTO.</returns>
	Task<ERPContactTitleInformationDto> GetContactTitle(Guid contactTitleId);

	/// <summary>
	/// Saves the provided ERP contactTitle.
	/// </summary>
	/// <param name="contactTitle">The ERP contactTitle to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveContactTitle(ERPContactTitleDto contactTitle);
}
