using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPNonConformanceCodeRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a NonConformanceCode with the specified Unique Id exists.
	/// </summary>
	/// <param name="nonConformanceCodeId">The Unique Id of the NonConformanceCode to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the NonConformanceCode exists or not.</returns>
	Task<bool> DoesNonConformanceCodeExist(Guid nonConformanceCodeId);

	/// <summary>
	/// Retrieves all NonConformanceCodes with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of NonConformanceCodes to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of NonConformanceCodes DTOs.</returns>
	Task<ICollection<ERPNonConformanceCodeInformationDto>> GetAllNonConformanceCodes(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific NonConformanceCode.
	/// </summary>
	/// <param name="nonConformanceCodeId">The Unique Id of the NonConformanceCode to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the NonConformanceCode DTO.</returns>
	Task<ERPNonConformanceCodeInformationDto> GetNonConformanceCode(Guid nonConformanceCodeId);

	/// <summary>
	/// Saves the provided ERP nonConformanceCode.
	/// </summary>
	/// <param name="nonConformanceCode">The ERP nonConformanceCode to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveNonConformanceCode(ERPNonConformanceCodeDto nonConformanceCode);
}
