using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPJobMaterialComponentRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a JobMaterialComponent with the specified Unique Id exists.
	/// </summary>
	/// <param name="jobMaterialComponentId">The Unique Id of the JobMaterialComponent to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the JobMaterialComponent exists or not.</returns>
	Task<bool> DoesJobMaterialComponentExist(Guid jobMaterialComponentId);

	/// <summary>
	/// Retrieves all JobMaterialComponents with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of JobMaterialComponents to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of JobMaterialComponents DTOs.</returns>
	Task<ICollection<ERPJobMaterialComponentInformationDto>> GetAllJobMaterialComponents(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific JobMaterialComponent.
	/// </summary>
	/// <param name="jobMaterialComponentId">The Unique Id of the JobMaterialComponent to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the JobMaterialComponent DTO.</returns>
	Task<ERPJobMaterialComponentInformationDto> GetJobMaterialComponent(Guid jobMaterialComponentId);

	/// <summary>
	/// Saves the provided ERP jobMaterialComponent.
	/// </summary>
	/// <param name="jobMaterialComponent">The ERP jobMaterialComponent to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveJobMaterialComponent(ERPJobMaterialComponentDto jobMaterialComponent);
}
