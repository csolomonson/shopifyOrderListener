using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPNonConformanceRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a NonConformance with the specified Unique Id exists.
	/// </summary>
	/// <param name="nonConformanceId">The Unique Id of the NonConformance to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the NonConformance exists or not.</returns>
	Task<bool> DoesNonConformanceExist(Guid nonConformanceId);

	/// <summary>
	/// Retrieves all NonConformances with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of NonConformances to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of NonConformances DTOs.</returns>
	Task<ICollection<ERPNonConformanceInformationDto>> GetAllNonConformances(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific NonConformance.
	/// </summary>
	/// <param name="nonConformanceId">The Unique Id of the NonConformance to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the NonConformance DTO.</returns>
	Task<ERPNonConformanceInformationDto> GetNonConformance(Guid nonConformanceId);

	/// <summary>
	/// Saves the provided ERP nonConformance.
	/// </summary>
	/// <param name="nonConformance">The ERP nonConformance to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveNonConformance(ERPNonConformanceDto nonConformance);
}
