using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPInspectionComponentRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a InspectionComponent with the specified Unique Id exists.
	/// </summary>
	/// <param name="inspectionComponentId">The Unique Id of the InspectionComponent to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the InspectionComponent exists or not.</returns>
	Task<bool> DoesInspectionComponentExist(Guid inspectionComponentId);

	/// <summary>
	/// Retrieves all InspectionComponents with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of InspectionComponents to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of InspectionComponents DTOs.</returns>
	Task<ICollection<ERPInspectionComponentInformationDto>> GetAllInspectionComponents(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific InspectionComponent.
	/// </summary>
	/// <param name="inspectionComponentId">The Unique Id of the InspectionComponent to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the InspectionComponent DTO.</returns>
	Task<ERPInspectionComponentInformationDto> GetInspectionComponent(Guid inspectionComponentId);

	/// <summary>
	/// Saves the provided ERP inspectionComponent.
	/// </summary>
	/// <param name="inspectionComponent">The ERP inspectionComponent to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveInspectionComponent(ERPInspectionComponentDto inspectionComponent);
}
