using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPCountyCodeRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a CountyCode with the specified Unique Id exists.
	/// </summary>
	/// <param name="countyCodeId">The Unique Id of the CountyCode to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the CountyCode exists or not.</returns>
	Task<bool> DoesCountyCodeExist(Guid countyCodeId);

	/// <summary>
	/// Retrieves all CountyCodes with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of CountyCodes to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of CountyCodes DTOs.</returns>
	Task<ICollection<ERPCountyCodeInformationDto>> GetAllCountyCodes(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific CountyCode.
	/// </summary>
	/// <param name="countyCodeId">The Unique Id of the CountyCode to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the CountyCode DTO.</returns>
	Task<ERPCountyCodeInformationDto> GetCountyCode(Guid countyCodeId);

	/// <summary>
	/// Saves the provided ERP countyCode.
	/// </summary>
	/// <param name="countyCode">The ERP countyCode to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveCountyCode(ERPCountyCodeDto countyCode);
}
