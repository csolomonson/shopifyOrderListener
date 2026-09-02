using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPInspectionRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a Inspection with the specified Unique Id exists.
	/// </summary>
	/// <param name="inspectionId">The Unique Id of the Inspection to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the Inspection exists or not.</returns>
	Task<bool> DoesInspectionExist(Guid inspectionId);

	/// <summary>
	/// Retrieves all Inspections with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Inspections to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of Inspections DTOs.</returns>
	Task<ICollection<ERPInspectionInformationDto>> GetAllInspections(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific Inspection.
	/// </summary>
	/// <param name="inspectionId">The Unique Id of the Inspection to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the Inspection DTO.</returns>
	Task<ERPInspectionInformationDto> GetInspection(Guid inspectionId);

	/// <summary>
	/// Saves the provided ERP inspection.
	/// </summary>
	/// <param name="inspection">The ERP inspection to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveInspection(ERPInspectionDto inspection);
}
