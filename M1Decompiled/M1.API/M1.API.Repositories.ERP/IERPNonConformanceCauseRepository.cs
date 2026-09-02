using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPNonConformanceCauseRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a NonConformanceCause with the specified Unique Id exists.
	/// </summary>
	/// <param name="nonConformanceCauseId">The Unique Id of the NonConformanceCause to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the NonConformanceCause exists or not.</returns>
	Task<bool> DoesNonConformanceCauseExist(Guid nonConformanceCauseId);

	/// <summary>
	/// Retrieves all NonConformanceCauses with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of NonConformanceCauses to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of NonConformanceCauses DTOs.</returns>
	Task<ICollection<ERPNonConformanceCauseInformationDto>> GetAllNonConformanceCauses(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific NonConformanceCause.
	/// </summary>
	/// <param name="nonConformanceCauseId">The Unique Id of the NonConformanceCause to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the NonConformanceCause DTO.</returns>
	Task<ERPNonConformanceCauseInformationDto> GetNonConformanceCause(Guid nonConformanceCauseId);

	/// <summary>
	/// Saves the provided ERP nonConformanceCause.
	/// </summary>
	/// <param name="nonConformanceCause">The ERP nonConformanceCause to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveNonConformanceCause(ERPNonConformanceCauseDto nonConformanceCause);
}
